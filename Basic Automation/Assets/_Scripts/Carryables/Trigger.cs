using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trigger : Chargeable, IInteractable
{
    public void Interact()
    {
        bool send = false;
        foreach (var a in ReturnElectricAmount())
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
    public override void Drop(out GridDictData sc)
    {
        FindBatteryPlace(out sc);
    }

}