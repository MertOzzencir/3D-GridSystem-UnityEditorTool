using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Dictionary<SourcesSO, int> sources = new Dictionary<SourcesSO, int>();

    private InventoryUIManager inventoryManager;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
        }
        inventoryManager = GetComponent<InventoryUIManager>();
    }
    public void AddSource(SourcesSO source)
    {
        if (sources.ContainsKey(source))
        {
            sources[source] += 1;
        }
        else
        {
            sources.Add(source, 1);
        }
        inventoryManager.Refresh(this);
    }
    [ContextMenu("Show Inventory")]
    public void ShowInventory()
    {
        foreach (var a in sources)
        {
            Debug.Log("Source Name: " + a.Key + " Source Amount: " + a.Value);
        }
    }




}
