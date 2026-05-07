using System.Collections;
using UnityEngine;

public abstract class MachineBlueprintBase : CarryablePlaceable, ITriggerInput
{
    private MachineSlots[] slots;
    private Coroutine toolLogicC;

    public override void Start()
    {
        base.Start();
        slots = GetComponentsInChildren<MachineSlots>();

    }
    public override void Carry()
    {
        foreach (var a in slots)
        {
            if (a.GetTool() != null)
            {
                a.GetTool().CarryFromMachine();
                a.RemoveTool();
                return;
            }
        }
        base.Carry();
    }
    public override void AlternativeCarry()
    {
        base.Carry();
    }
    public override void Drop(out GridDictData sc)
    {
        base.Drop(out sc);
        foreach (var a in slots)
        {
            if (a.GetTool() != null)
                a.ToolUpdateGridPosition(GridPosition);
        }
    }
    public void AddToolOnSlot(MachineTools t, out bool suc)
    {
        suc = false;
        foreach (var a in slots)
        {
            if (a.GetTool() == null)
            {
                a.SetupTool(t, this);
                suc = true;
                break;
            }
        }
    }
    public override void AddOnGrid(Vector3 addPosition, out GridDictData currentGrid)
    {
        currentGrid = GridManager.Instance.AddMachineOnGrid(addPosition, Size, this);
        if (currentGrid != null)
            GridPosition = GridManager.Instance.SnappedPosition(currentGrid.Grid.transform.position);
    }
    public override void AddOnGridWithMouse(out GridDictData currentGrid)
    {
        currentGrid = GridManager.Instance.AddMachineOnGridWithMousePosition(Size, this);
        if (currentGrid != null)
            GridPosition = GridManager.Instance.SnappedPosition(currentGrid.Grid.transform.position);
    }
    public override void DeleteOnGrid()
    {
        GridManager.Instance.DeleteMachineOnGrid(GridPosition, Size);
        GridPosition = default;
    }

    public virtual void GetSignal(out bool sc)
    {
        sc = true;
        if (toolLogicC != null)
        {
            sc = false;
            return;
        }


        toolLogicC = StartCoroutine(HandleTools());
    }
    private IEnumerator HandleTools()
    {
        bool firstEnter = true;
        MachineSlots lastUsedTool = null;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetTool() == null) continue;

            if (firstEnter)
            {
                slots[i].UseTool();
                lastUsedTool = slots[i];
                firstEnter = false;
                continue;
            }
            yield return new WaitForSeconds(lastUsedTool.ToolTimer());
            slots[i].UseTool();
            lastUsedTool = slots[i];
        }
        toolLogicC = null;
    }
}
