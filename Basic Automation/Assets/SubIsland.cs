using System.Collections;
using UnityEngine;

public class SubIsland : MainIsland
{
    [SerializeField] private Transform[] bridge;
    void Start()
    {
        StartCoroutine(BridgeAnimation());
    }
    private IEnumerator BridgeAnimation()
    {
        foreach (Transform path in bridge)
        {
            Transform visual = path.GetChild(0);
            float cachedHeight = visual.localPosition.y;
            Vector3 targetPosition = visual.localPosition;
            Vector3 targetScale = visual.localScale;
            visual.localPosition = new Vector3(visual.localPosition.x, targetPosition.y + 2f, visual.localPosition.z);
            path.gameObject.SetActive(true);
            while (Mathf.Abs(visual.localPosition.y - targetPosition.y) > .1f)
            {
                visual.localPosition = Vector3.Lerp(visual.localPosition, targetPosition, 25 * Time.deltaTime);
                yield return null;
            }
            while (Vector3.Distance(visual.localScale, targetScale / 2f) > 0.1f)
            {
                visual.localScale = Vector3.Lerp(visual.localScale, targetScale / 2f, 15f * Time.deltaTime);
                yield return null;
            }
            while (Vector3.Distance(visual.localScale, targetScale) > 0.1f)
            {
                visual.localScale = Vector3.Lerp(visual.localScale, targetScale, 20f * Time.deltaTime);
                yield return null;
            }
            visual.localPosition = targetPosition;
            visual.localScale = targetScale;
        }
    }
}
