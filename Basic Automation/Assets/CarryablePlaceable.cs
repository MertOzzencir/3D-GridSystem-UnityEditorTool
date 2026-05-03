using UnityEngine;

public abstract class CarryablePlaceable : Placeable, ICarryable
{
    [SerializeField] protected Transform visual;
    public Transform Visual => visual;
    public GridRotationManager RotationManager { get; set; }
    protected Collider c;

    protected virtual void Awake()
    {
        c = GetComponent<Collider>();
        RotationManager = new GridRotationManager();
        RotationManager.Initialize();
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
        if (sc != null)
        {
            c.enabled = true;
            Visual.eulerAngles = Vector3.zero;
        }
        else
            OnCarryDrop();
    }

    public virtual void Rotate()
    {
        RotationManager.HandleRotate();
        Visual.GetChild(0).transform.localEulerAngles = RotationManager.GetRotation();
    }

    public Transform GetTransform() => transform;

    protected virtual void OnCarryDrop()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visual.eulerAngles = Vector3.zero;
    }

    public virtual void UpdateMain()
    {
        Visual.rotation = Quaternion.Lerp(Visual.rotation, Quaternion.LookRotation(Vector3.forward), 32 * Time.deltaTime);
    }
}