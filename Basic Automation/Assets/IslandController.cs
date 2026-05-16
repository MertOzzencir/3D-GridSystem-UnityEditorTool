using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IslandController : Placeable
{
    [SerializeField] private MainIsland targetIsland;
    [SerializeField] private IslandReqs[] activeReqs;

    [ContextMenu("Open Island")]
    public void OpenIsland()
    {
        foreach (var a in activeReqs)
        {
            a.Amount = 0;
        }
        CheckReqs();
    }
    public void CheckReqs()
    {
        bool canOpen = true;
        foreach (var a in activeReqs)
        {
            if (a.Amount <= 0)
                continue;
            else
                canOpen = false;
        }
        if (canOpen)
            targetIsland.gameObject.SetActive(true);
    }
}
[Serializable]
public class IslandReqs
{
    public SourcesSO Source;
    public int Amount;
}
