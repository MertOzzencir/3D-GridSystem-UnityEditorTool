using UnityEngine;
using UnityEngine.Rendering;

public class ElectricGenerator : Placeable
{
    private Trigger currentTrigger;
    public void ChargeTrigger(Trigger t)
    {
        if (currentTrigger != null) return;
        currentTrigger = t;
        currentTrigger.transform.parent = transform;
        currentTrigger.transform.localPosition = Vector3.zero;
        currentTrigger.GridPosition = GridPosition;
        currentTrigger.transform.eulerAngles = Vector3.zero;
        currentTrigger.Visual.localEulerAngles = Vector3.zero;
        InvokeRepeating("ChargeObject", .5f, .5f);

    }
    public override void AlternativeCarry()
    {
        Debug.Log("Try");
        if (currentTrigger == null) return;
        CarryableEvents.Invoke(currentTrigger);
        currentTrigger.OnCarryDrop();
        currentTrigger = null;
        CancelInvoke();
    }
    public void ChargeObject()
    {
        if (currentTrigger == null) return;
        for (int i = currentTrigger.ReturnElectricAmount().Length - 1; i >= 0; i--)
        {
            if (!currentTrigger.ReturnElectricAmount()[i].activeSelf)
            {
                currentTrigger.ReturnElectricAmount()[i].SetActive(true);
                break;
            }
        }
    }
}
