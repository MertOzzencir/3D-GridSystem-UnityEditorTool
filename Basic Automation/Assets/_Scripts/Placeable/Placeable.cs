using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    public Vector2 Size;
    public Vector3Int GridPosition { get; private set; }

    public virtual void Start()
    {
        AddOnGrid(transform.position, out GridDictData s);
        if (s == null)
            Destroy(gameObject);
    }
    public void AddOnGrid(Vector3 addPosition, out GridDictData currentGrid)
    {
        currentGrid = GridManager.Instance.AddOnGrid(addPosition, Size, this);
        if (currentGrid != null)
            GridPosition = GridManager.Instance.SnappedPosition(currentGrid.Grid.transform.position);
    }
    public void AddOnGridWithMouse(out GridDictData currentGrid)
    {
        currentGrid = GridManager.Instance.AddOnGridWithMousePosition(Size, this);
        if (currentGrid != null)
            GridPosition = GridManager.Instance.SnappedPosition(currentGrid.Grid.transform.position);
    }
    public void DeleteOnGrid()
    {
        GridManager.Instance.DeleteOnGrid(GridPosition, Size);
        GridPosition = default;
    }

}
