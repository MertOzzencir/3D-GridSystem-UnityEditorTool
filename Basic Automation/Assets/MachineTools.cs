using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MachineTools : CarryablePlaceable
{
    private MachineBlueprintBase machineBase;

    public abstract void ToolLogic();
    public void Setup(MachineBlueprintBase m)
    {
        machineBase = m;
    }
    public override void Drop(out GridDictData sc)
    {
        sc = null;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
        {
            MachineBlueprintBase goalMachine = GridManager.Instance.GetOneMachineInGrid(hit.point);
            if (goalMachine != null)
            {
                GridPosition = goalMachine.GridPosition;
                sc = GridManager.Instance.GetGridData(goalMachine.GridPosition);
                ResetRotation();
                goalMachine.AddToolOnSlot(this, out bool s);
                if (s)
                    return;
            }
        }
        base.Drop(out sc);

    }
    public MachineBlueprintBase GetBase()
    {
        return machineBase;
    }
}
