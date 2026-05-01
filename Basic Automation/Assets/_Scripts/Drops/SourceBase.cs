using UnityEngine;

public class SourceBase : Placeable, ICollectable
{
    [SerializeField] private SourcesSO data;
    [ContextMenu("collect")]
    public void Collect()
    {
        DeleteOnGrid();
        InventoryManager.Instance.AddSource(data);
        Destroy(gameObject);
    }
}
