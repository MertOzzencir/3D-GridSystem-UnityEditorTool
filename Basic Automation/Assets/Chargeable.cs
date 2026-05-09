using UnityEngine;
using UnityEngine.InputSystem;

public class Chargeable : CarryablePlaceable
{
    [SerializeField] private GameObject[] electricVisuals;

    public override void Drop(out GridDictData sc)
    {
        FindBatteryPlace(out sc);
    }

    public void FindBatteryPlace(out GridDictData sc)
    {
        sc = null;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
        {
            sc = GridManager.Instance.GetGridData(hit.point);
            if (sc == null) return;
            if (sc.Placeable != null)
            {
                if (sc.Placeable.TryGetComponent(out ElectricGenerator generator))
                {
                    generator.ChargeTrigger(this);
                    Visual.localEulerAngles = RotationManager.GetRotation(Visual);
                    return;
                }

            }
        }
        base.Drop(out sc);

    }
    public void ElectricVisualHandle()
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
