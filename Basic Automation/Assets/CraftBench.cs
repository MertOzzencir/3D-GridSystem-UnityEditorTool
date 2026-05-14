using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftBench : Placeable, IHoverable
{
    [SerializeField] private GameObject craftMenu;
    [SerializeField] private List<CraftRecipts> craftRecipts;
    [SerializeField] private Image ReciptImage;
    [SerializeField] private Image OutPutImage;
    [SerializeField] private TextMeshProUGUI LeftedAmountText;
    private CraftRecipts currentRecipt;
    private int leftedAmountRef;
    void Awake()
    {
        ResetRecipt();
    }
    public void Hover()
    {
        craftMenu.SetActive(true);
    }

    public void UnHover()
    {
        craftMenu.SetActive(false);
    }
    public void Craft()
    {
        GridManager m = GridManager.Instance;
        GameObject gridRef = m.GetGridGameObject(m.GetOneGridInRange(GridPosition, Size));
        if (gridRef != null)
        {
            Vector3Int freeSpawnPosition = new Vector3Int(Mathf.FloorToInt(gridRef.transform.position.x),
                                                          Mathf.FloorToInt(gridRef.transform.localScale.y),
                                                         Mathf.FloorToInt(gridRef.transform.position.z)
            );
            CraftableBase spawnedDrop = Instantiate(currentRecipt.OutputRecipt.Prefab.GetComponent<CraftableBase>(), freeSpawnPosition, Quaternion.identity);
            Transform dropVisual = spawnedDrop.ReturnVisual();
            dropVisual.position = transform.position + new Vector3(0.5f, 0.0f, 0.5f);
            if (spawnedDrop != null)
                spawnedDrop.SpawnAnimation();
            Scrool(0);
        }
    }
    public void AddSource()
    {
        InventoryManager currentInventory = InventoryManager.Instance;
        bool isAvaliableInInventory = currentInventory.CheckSource(currentRecipt.InputRecipt);
        if (isAvaliableInInventory)
        {
            float amountInInventory = currentInventory.SourceAmount(currentRecipt.InputRecipt);
            if (amountInInventory > 0)
            {
                currentInventory.ReduceSource(currentRecipt.InputRecipt);
                leftedAmountRef -= 1;
                if (leftedAmountRef <= 0)
                    Craft();
                RefreshUI();
            }
        }
    }
    public void Scrool(int horizontalIndex)
    {
        int currentIndex = craftRecipts.IndexOf(currentRecipt);
        if (currentIndex + horizontalIndex < craftRecipts.Count && currentIndex + horizontalIndex >= 0)
        {
            CraftRecipts next = craftRecipts[currentIndex + horizontalIndex];
            if (next != null)
            {
                SetRecipt(next);
            }
        }
        else ResetRecipt();

    }
    private void SetUI(CraftRecipts recipt)
    {
        ReciptImage.sprite = recipt.InputRecipt.Icon;
        OutPutImage.sprite = recipt.OutputRecipt.Icon;
        leftedAmountRef = recipt.Amount;
        LeftedAmountText.text = leftedAmountRef.ToString();
    }
    private void RefreshUI()
    {
        ReciptImage.sprite = currentRecipt.InputRecipt.Icon;
        OutPutImage.sprite = currentRecipt.OutputRecipt.Icon;
        LeftedAmountText.text = leftedAmountRef.ToString();
    }
    private void ResetRecipt()
    {
        if (craftRecipts[0] != null)
        {
            SetRecipt(craftRecipts[0]);
        }
    }
    private void SetRecipt(CraftRecipts current)
    {
        currentRecipt = current;
        SetUI(currentRecipt);
    }
}

[Serializable]
public class CraftRecipts
{
    public SourcesSO InputRecipt;
    public SourcesSO OutputRecipt;
    public int Amount;
}