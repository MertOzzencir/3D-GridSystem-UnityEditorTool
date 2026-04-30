using System;
using UnityEngine;

public class InteractionController : MonoBehaviour
{

    public event Action<InteractState> OnInteract;

    private Collider[] FreeAllocSphereCheck = new Collider[15];
    private DistanceComparer distanceComparer = new DistanceComparer();
    private PlayerLocomotionController movementController;
    void Awake()
    {
        movementController = GetComponent<PlayerLocomotionController>();
    }
    private void ReadyToInteract(bool state)
    {
        if (state)
        {
            OnInteract?.Invoke(InteractState.InteractReady);
            movementController.HorizontalMovementStateChange(false);
        }
        else
        {
            OnInteract?.Invoke(InteractState.InteractReadyNegative);
            movementController.HorizontalMovementStateChange(true);
        }

    }
    private void Interact(bool obj)
    {
        if (obj)
        {
            OnInteract?.Invoke(InteractState.InteractCharge);
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, 3f, FreeAllocSphereCheck, LayerMask.GetMask("Interactable"));
            distanceComparer.origin = transform.position;
            System.Array.Sort(FreeAllocSphereCheck, 0, hitCount, distanceComparer);

            for (int i = 0; i < hitCount; i++)
            {

                Debug.Log(FreeAllocSphereCheck[i].name);
                if (FreeAllocSphereCheck[i].TryGetComponent(out IInteractable interact))
                {
                    Vector3 lookDirection = (interact.GetTransform().position - transform.position).normalized;
                    lookDirection.y = 0;
                    if (Vector3.Dot(transform.forward, lookDirection) > .2f)
                    {
                        interact.Interact();
                    }
                    break;
                }
            }
        }
        else
        {
            OnInteract?.Invoke(InteractState.InteractChargeNegative);
        }

    }
    void OnEnable()
    {
        InputManager.OnLeftClick += Interact;
        InputManager.OnRightClick += ReadyToInteract;
    }
    void OnDisable()
    {
        InputManager.OnLeftClick -= Interact;
        InputManager.OnRightClick -= ReadyToInteract;
    }
}
public enum InteractState
{
    Idle,
    InteractReady,
    InteractReadyNegative,
    InteractCharge,
    InteractChargeNegative,
    Interact
}
