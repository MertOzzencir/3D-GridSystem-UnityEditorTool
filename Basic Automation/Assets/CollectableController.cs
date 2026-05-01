using UnityEngine;

public class CollectableController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }
}
