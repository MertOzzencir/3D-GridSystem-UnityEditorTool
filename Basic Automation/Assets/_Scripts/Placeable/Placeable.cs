using System.Collections;
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
    public virtual void AlternativeCarry()
    {

    }
    public virtual Transform ReturnVisual()
    {
        return null;
    }
    
    public IEnumerator Animation(Transform visual, Vector3 localVisual, float duration, AnimationCurve yOffset)
    {
        Vector3 startPosition = visual.localPosition;
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
