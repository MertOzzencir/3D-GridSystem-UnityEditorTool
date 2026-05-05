using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    public Vector2 Size;
    public Vector3Int GridPosition { get; set; }

    public virtual void Start()
    {
        AddOnGrid(transform.position, out GridDictData s);
        if (s == null)
            Destroy(gameObject);
    }
    public virtual void AddOnGrid(Vector3 addPosition, out GridDictData currentGrid)
    {
        currentGrid = GridManager.Instance.AddPlaceableOnGrid(addPosition, Size, this);
        if (currentGrid != null)
            GridPosition = GridManager.Instance.SnappedPosition(currentGrid.Grid.transform.position);
    }
    public virtual void AddOnGridWithMouse(out GridDictData currentGrid)
    {
        currentGrid = GridManager.Instance.AddPlaceableOnGridWithMousePosition(Size, this);
        if (currentGrid != null)
            GridPosition = GridManager.Instance.SnappedPosition(currentGrid.Grid.transform.position);
    }
    public virtual void DeleteOnGrid()
    {
        GridManager.Instance.DeletePlaceableOnGrid(GridPosition, Size);
        GridPosition = default;
    }

}
