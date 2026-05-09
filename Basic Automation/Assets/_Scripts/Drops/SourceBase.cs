using System;
using System.Collections;
using UnityEngine;

public class SourceBase : Placeable, ICollectable
{
    [SerializeField] private SourcesSO data;
    [SerializeField] private Transform visual;
    [SerializeField] private AnimationCurve yOffset;
    [SerializeField] private float duration = 0.5f;

    Vector3 localVisual;
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
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        Vector3 startPosition = visual.localPosition; // şu anki yer
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = yOffset.Evaluate(t);

            Vector3 horizontalPos = Vector3.Lerp(startPosition, localVisual, t);

            visual.localPosition = horizontalPos + Vector3.up * curveValue;

            yield return null;
        }
        visual.localPosition = localVisual;
    }
}
