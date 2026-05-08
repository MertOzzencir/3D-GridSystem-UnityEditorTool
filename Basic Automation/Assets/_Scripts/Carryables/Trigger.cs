using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public override void Drop(out GridDictData sc)
    {
        sc = null;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
        {
            sc = GridManager.Instance.GetGridData(hit.point);
            if (sc == null) return;
            if (sc.Placeable == null)
            {
                base.Drop(out sc);
                return;
            }
            if (sc.Placeable.TryGetComponent(out ElectricGenerator generator))
            {
                generator.ChargeTrigger(this);
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
    public void ChargeSelf()
    {
        for (int i = electricVisuals.Length - 1; i >= 0; i--)
        {
            if (electricVisuals[i].activeSelf == false)
            {
                electricVisuals[i].SetActive(true);
                break;
            }
        }
    }
    public GameObject[] ReturnElectricAmount()
    {
        return electricVisuals;
    }
}