using UnityEngine;

public class Trigger : CarryablePlaceable, IInteractable
{
    public void Interact()
    {
        Vector3 lookDirection = RotationManager.GetDirection();
        MachineBlueprintBase aim = GridManager.Instance.GetOneMachineInGrid(GridPosition + lookDirection);
        if (aim == null) return;
        if (aim.TryGetComponent(out ITriggerInput input))
            input.GetSignal();
    }
}