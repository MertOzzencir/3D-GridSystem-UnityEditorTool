using UnityEngine;

public class RotateTool : MachineTools
{
    public override void ToolLogic()
    {
        Placeable aim = GridManager.Instance.GetOnePlaceableInGrid(GridPosition);
        if (aim == null) return;
        if (aim.TryGetComponent(out Spawner output))
        {
            output.Rotate();
            output.SpawnPackage();
        }
    }


}
