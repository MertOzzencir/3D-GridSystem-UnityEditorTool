using UnityEngine;

public class WoodCuteTool : MachineTools
{
    public override void ToolLogic()
    {
        Placeable aim = GridManager.Instance.GetOnePlaceableInGrid(GridPosition);
        if (aim == null) return;
        if (aim.TryGetComponent(out ITriggerOutput output)) output.ProcessSignal();
        Debug.Log("Allah");
    }

}
