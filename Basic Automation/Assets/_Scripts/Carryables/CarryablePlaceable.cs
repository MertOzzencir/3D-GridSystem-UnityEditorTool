using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CarryablePlaceable : Placeable, ICarryable
{
    [SerializeField] protected Transform visual;
    public Transform Visual => visual;
    public GridRotationManager RotationManager { get; set; }
    protected Collider c;
    protected Vector3 visualSavedPosition;

    public virtual void Awake()
    {
        c = GetComponent<Collider>();
        RotationManager = new GridRotationManager();
        RotationManager.Initialize(Visual);
        visualSavedPosition = Visual.transform.localPosition;
    }

    public virtual void Carry()
    {

        c.enabled = false;
        DeleteOnGrid();
        CarryableEvents.Invoke(this);
        OnCarryDrop();
    }

    public virtual void Drop(out GridDictData sc)
    {
        AddOnGridWithMouse(out sc);
        OnGridDrop(sc);
    }

    public void OnGridDrop(GridDictData sc)
    {
        if (sc != null)
        {
            c.enabled = true;
            Visual.localEulerAngles = RotationManager.GetRotation(Visual);
        }
        else
            OnCarryDrop();
    }

    public virtual void Rotate()
    {
        RotationManager.HandleRotate();
        Visual.localEulerAngles = RotationManager.GetRotation(Visual);
    }

    public Transform GetTransform() => transform;

    public void ResetRotation()
    {
        StartCoroutine(RotationCorrectionFromUpdateMain());
    }
    public virtual void OnCarryDrop()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        //Visual.eulerAngles = Vector3.zero;
    }

    public virtual void UpdateMain()
    {
        if (visual.rotation == Quaternion.LookRotation(RotationManager.GetDirection())) return;

        Visual.rotation = Quaternion.Lerp(Visual.rotation, Quaternion.LookRotation(RotationManager.GetDirection()), 32 * Time.deltaTime);
    }

    IEnumerator RotationCorrectionFromUpdateMain()
    {
        yield return new WaitForNextFrameUnit();
        RotationManager.ResetRotation();
        Visual.localEulerAngles = Vector3.zero;
        Visual.transform.localEulerAngles = RotationManager.GetRotation(Visual);
    }
    public Transform GetVisual()
    {
        return Visual;
    }

    public virtual void AlternativeCarry(Transform t)
    {
    }
}