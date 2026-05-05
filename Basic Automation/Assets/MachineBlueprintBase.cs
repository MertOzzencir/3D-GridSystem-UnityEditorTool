using UnityEngine;

public abstract class MachineBlueprintBase : CarryablePlaceable, ITriggerInput
{
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

    public void GetSignal()
    {
        Placeable aim = GridManager.Instance.GetOnePlaceableInGrid(GridPosition);
        if (aim == null) return;
        if (aim.TryGetComponent(out ITriggerOutput output)) output.ProcessSignal();
    }
}
