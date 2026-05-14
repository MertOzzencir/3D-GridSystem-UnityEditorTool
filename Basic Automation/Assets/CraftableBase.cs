using UnityEngine;

public class CraftableBase : Placeable, ICollectable
{
    [SerializeField] private SourcesSO data;
    [SerializeField] private Transform visual;
    [SerializeField] private float duration;
    [SerializeField] private AnimationCurve yOffset;

    private Vector3 localVisual;
    void Awake()
    {
        localVisual = visual.localPosition;
    }
    public void Collect()
    {
        DeleteOnGrid();
        InventoryManager.Instance.AddSource(data);
        Destroy(gameObject);
    }
    public override Transform ReturnVisual()
    {
        return visual;
    }
    public void SpawnAnimation()
    {
        StartCoroutine(Animation(visual, localVisual, duration, yOffset));
    }

}
