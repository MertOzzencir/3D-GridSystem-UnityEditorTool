using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    public Vector2 Size;


    void Start()
    {
        AddOnGrid();
    }
    public void AddOnGrid()
    {
        GridManager.Instance.AddOnGrid(transform.position, Size, this);
    }
    public void DeleteOnGrid()
    {
        GridManager.Instance.DeleteOnGrid(transform.position, Size);
    }
}
