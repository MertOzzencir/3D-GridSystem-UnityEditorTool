using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Trigger : Placeable, IInteractable, ICarryable
{
    Collider c;
    void Awake()
    {
        c = GetComponent<Collider>();
    }
    public void Carry()
    {
        c.enabled = false;
        DeleteOnGrid();
        CarryableEvents.Invoke(this);
        OnCarryDrop();
    }

    public void Drop(out bool sc)
    {
        sc = true;
        Transform parent = transform.parent;
        GridManager.Instance.AddOnGrid(parent.position + parent.forward, Size, this, out sc);
        if (sc)
            c.enabled = true;
        else
            OnCarryDrop();

    }

    private void OnCarryDrop()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public void Interact()
    {
        Debug.Log("Interacted with Trigger");
        foreach (var a in GridManager.Instance.GetPlaceableInRange(transform.position, Size))
        {
            if (a.Placeable == this) continue;
            Debug.Log("Found Object in area");
            if (a.Placeable.TryGetComponent(out ITriggerInput input))
            {

                input.GetSignal();
            }
        }
    }
}
