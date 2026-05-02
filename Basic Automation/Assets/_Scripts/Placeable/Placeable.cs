using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    public Vector2 Size;
    public Vector3Int GridPosition { get; private set; }

    public virtual void Start()
    {
        AddOnGrid(transform.position, out bool s);
        if (!s)
            Destroy(gameObject);
    }
    public void AddOnGrid(Vector3 addPosition, out bool s)
    {
        GridManager.Instance.AddOnGrid(addPosition, Size, this, out s);
        if (s)
            GridPosition = GridManager.Instance.SnappedPosition(addPosition);
    }
    public void DeleteOnGrid()
    {
        GridManager.Instance.DeleteOnGrid(GridPosition, Size);
        GridPosition = default;
    }

}
