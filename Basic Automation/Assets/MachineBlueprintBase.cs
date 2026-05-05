using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public abstract class MachineBlueprintBase : CarryablePlaceable, ITriggerInput
{
    private MachineSlots[] slots;

    public override void Start()
    {
        base.Start();
        slots = GetComponentsInChildren<MachineSlots>();

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

    public virtual void GetSignal()
    {
        foreach (var a in slots)
        {
            if (a.GetTool() != null)
            {
                a.UseTool();
            }
        }

    }
}
