using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionController : MonoBehaviour
{


    [SerializeField] private float tempToolCoolDown = 0.5f;
    private Collider[] FreeAllocSphereCheck = new Collider[10];
    private float lastHitTime;
    private DistanceComparer distanceComparer = new DistanceComparer();
    private void Interact(bool obj)
    {
        if (Time.time <= lastHitTime + tempToolCoolDown) return;
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, 2f, FreeAllocSphereCheck);
        distanceComparer.origin = transform.position;
        System.Array.Sort(FreeAllocSphereCheck, 0, hitCount, distanceComparer);

        for (int i = 0; i < hitCount; i++)
        {
            if (FreeAllocSphereCheck[i].TryGetComponent(out IInteractable interact))
            {
                Vector3 lookDirection = (interact.GetTransform().position - transform.position).normalized;
                lookDirection.y = 0;
                if (Vector3.Dot(transform.forward, lookDirection) > .4f)
                {
                    lastHitTime = Time.time;
                    interact.Interact();
                }
                break;
            }
        }
    }
    void OnEnable()
    {
        InputManager.OnRightClick += Interact;
    }
    void OnDisable()
    {
        InputManager.OnRightClick -= Interact;
    }
}
