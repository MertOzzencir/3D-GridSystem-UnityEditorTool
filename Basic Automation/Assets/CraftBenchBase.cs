using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CraftBenchBase<TInput, TOutput> : Placeable, IHoverable
    where TInput : ScriptableObject
    where TOutput : ScriptableObject
{
    [SerializeField] private GameObject craftMenu;
    [SerializeField] protected List<CraftRecipt<TInput, TOutput>> craftRecipts;
    [SerializeField] private Image ReciptImage;
    [SerializeField] private Image OutPutImage;
    [SerializeField] private TextMeshProUGUI LeftedAmountText;

    protected CraftRecipt<TInput, TOutput> currentRecipt;
    protected int leftedAmountRef;

    void Awake()
    {
        ResetRecipt();
    }

    public void Hover() => craftMenu.SetActive(true);
    public void UnHover() => craftMenu.SetActive(false);

    protected abstract void SpawnOutput(Vector3Int spawnPos);

    protected abstract Sprite GetInputIcon(TInput input);
    protected abstract Sprite GetOutputIcon(TOutput output);

    protected abstract bool CheckInventory(TInput input);
    protected abstract void ConsumeFromInventory(TInput input);
    protected abstract void RefundToInventory(TInput input);

    public void Craft()
    {
        GridManager m = GridManager.Instance;
        GameObject gridRef = m.GetGridGameObject(m.GetOneGridInRange(GridPosition, Size));
        if (gridRef != null)
        {
            Vector3Int freeSpawnPosition = new Vector3Int(
                Mathf.FloorToInt(gridRef.transform.position.x),
                Mathf.FloorToInt(gridRef.transform.localScale.y),
                Mathf.FloorToInt(gridRef.transform.position.z)
            );
            SpawnOutput(freeSpawnPosition);
            leftedAmountRef = currentRecipt.Amount;
            Scrool(0);
        }
    }

    public void AddSource()
    {
        if (CheckInventory(currentRecipt.InputRecipt))
        {
            ConsumeFromInventory(currentRecipt.InputRecipt);
            leftedAmountRef -= 1;
            if (leftedAmountRef <= 0) Craft();
            RefreshUI();
        }
    }

    public void Scrool(int horizontalIndex)
    {
        Refund();
        int currentIndex = craftRecipts.IndexOf(currentRecipt);
        int next = currentIndex + horizontalIndex;
        if (next >= 0 && next < craftRecipts.Count)
            SetRecipt(craftRecipts[next]);
        else
            ResetRecipt();
    }

    private void SetUI(CraftRecipt<TInput, TOutput> recipt)
    {
        ReciptImage.sprite = GetInputIcon(recipt.InputRecipt);
        OutPutImage.sprite = GetOutputIcon(recipt.OutputRecipt);
        leftedAmountRef = recipt.Amount;
        LeftedAmountText.text = leftedAmountRef.ToString();
    }

    private void RefreshUI()
    {
        ReciptImage.sprite = GetInputIcon(currentRecipt.InputRecipt);
        OutPutImage.sprite = GetOutputIcon(currentRecipt.OutputRecipt);
        LeftedAmountText.text = leftedAmountRef.ToString();
    }

    private void ResetRecipt()
    {
        if (craftRecipts.Count > 0 && craftRecipts[0] != null)
            SetRecipt(craftRecipts[0]);
    }

    private void SetRecipt(CraftRecipt<TInput, TOutput> current)
    {
        currentRecipt = current;
        SetUI(currentRecipt);
    }

    private void Refund()
    {
        int refundAmount = currentRecipt.Amount - leftedAmountRef;
        for (int i = 0; i < refundAmount; i++)
            RefundToInventory(currentRecipt.InputRecipt);
    }
}

[Serializable]
public class CraftRecipt<TInput, TOutput>
{
    public TInput InputRecipt;
    public TOutput OutputRecipt;
    public int Amount;
}