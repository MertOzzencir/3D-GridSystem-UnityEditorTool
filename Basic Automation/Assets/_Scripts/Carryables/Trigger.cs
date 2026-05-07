using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Trigger : CarryablePlaceable, IInteractable
{
    [SerializeField] private GameObject[] electricVisuals;
    public void Interact()
    {
        bool send = false;
        foreach (var a in electricVisuals)
        {
            if (a.activeSelf)
                send = true;
        }
        if (!send) return;
        Vector3 lookDirection = RotationManager.GetDirection();
        MachineBlueprintBase aim = GridManager.Instance.GetOneMachineInGrid(GridPosition + lookDirection);
        if (aim == null) return;
        if (aim.TryGetComponent(out ITriggerInput input))
        {
            input.GetSignal(out bool success);
            if (success)
            {
                ElectricVisualHandle();
            }
        }
    }

    private void ElectricVisualHandle()
    {
        for (int i = 0; i < electricVisuals.Length; i++)
        {
            if (electricVisuals[i].activeSelf)
            {
                electricVisuals[i].SetActive(false);
                break;
            }
        }
    }
}