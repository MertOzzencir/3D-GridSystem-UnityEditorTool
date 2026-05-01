using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    public Vector2 Size;


    public virtual void Start()
    {
        AddOnGrid();
    }
    public void AddOnGrid()
    {
        GridManager.Instance.AddOnGrid(transform.position, Size, this, out bool s);
        if (!s)
            Destroy(this.gameObject);
    }
    public void DeleteOnGrid()
    {
        GridManager.Instance.DeleteOnGrid(transform.position, Size);
    }
}
