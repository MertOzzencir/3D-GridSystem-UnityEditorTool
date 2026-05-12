using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Transform pickupTransform;
    public event Action<InteractState> OnInteract;

    private PlayerLocomotionController movementController;
    private GlobalAnimationTrigger SubscribeAnimation;
    InteractState currentState;
    ICarryable currentCarryable;
    IHoverable hoverable;

    void Awake()
    {
        currentState = InteractState.Idle;
        SubscribeAnimation = GetComponentInChildren<GlobalAnimationTrigger>();
        SubscribeAnimation.OnAnimationTrigger += OnInteractAnimation;
        movementController = GetComponent<PlayerLocomotionController>();
        CarryableEvents.OnCarryable += CarryObject;
    }
    void Update()
    {
        if (currentCarryable != null)
            currentCarryable.UpdateMain();
        if (currentState == InteractState.InteractReady)
            movementController.LogicOnInteractReady();

        Hover();
    }
    private void Hover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out IHoverable hover))
            {
                hoverable = hover;
                hover.Hover();
            }
            else if (hoverable != null)
            {
                hoverable.UnHover();
                hoverable = null;
            }
        }
       
    }
    private void CarryObject(ICarryable t)
    {
        GridDictData sc = null;
        if (currentCarryable != null)
        {
            currentCarryable.Drop(out sc);
        }
        if (sc == null)
            currentCarryable = t;

        currentCarryable.GetTransform().parent = pickupTransform;
    }
    private void RotateCarryable()
    {
        if (currentCarryable == null) return;

        currentCarryable.Rotate();
    }

    private void ReadyToInteract(bool state)
    {
        if (state && currentState == InteractState.Idle)
        {
            currentState = InteractState.InteractReady;
            OnInteract?.Invoke(InteractState.InteractReady);
            movementController.HorizontalMovementStateChange(false);
        }
        else
        {
            currentState = InteractState.Idle;
            OnInteract?.Invoke(InteractState.InteractReadyNegative);
            movementController.HorizontalMovementStateChange(true);
        }

    }
    private void Interact(bool obj)
    {
        if (obj && currentState == InteractState.InteractReady)
        {
            OnInteract?.Invoke(InteractState.InteractCharge);

        }
        else
        {
            OnInteract?.Invoke(InteractState.InteractChargeNegative);
        }

    }

    private void Pickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Grid")) && currentCarryable == null)
        {
            GridDictData grid = GridManager.Instance.GetGridData(hit.point);
            if (grid == null) return;
            if (grid.Placeable != null)
            {
                if (grid.Placeable.TryGetComponent(out CarryablePlaceable carryable))
                {
                    carryable.Carry();
                    return;
                }
            }
            if (grid.Machine != null)
            {
                if (grid.Machine.TryGetComponent(out CarryablePlaceable c))
                {
                    if (currentState == InteractState.InteractReady)
                    {
                        c.AlternativeCarry();
                        return;
                    }
                    c.Carry();
                }
            }

            if (grid.Placeable != null)
            {
                if (grid.Placeable.TryGetComponent(out Placeable c))
                {
                    if (currentState == InteractState.InteractReady)
                    {
                        c.AlternativeCarry();
                        return;
                    }
                }
            }
        }
        else
        {
            if (currentCarryable != null)
            {
                currentCarryable.Drop(out GridDictData sc);
                if (sc != null)
                {
                    currentCarryable = null;
                }
                else
                {
                    currentCarryable.GetTransform().parent = pickupTransform;
                }
            }
        }
    }
    private void OnInteractAnimation(string animname)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 3f);

        System.Array.Sort(hits, (a, b) =>
            Vector3.Distance(a.transform.position, transform.position)
            .CompareTo(Vector3.Distance(b.transform.position, transform.position)));

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].TryGetComponent(out IInteractable interact))
            {
                Vector3 lookDirection = (interact.GetTransform().position - transform.position).normalized;
                lookDirection.y = 0;
                if (Vector3.Dot(transform.forward, lookDirection) > 0f)
                {
                    interact.Interact();
                }
                break;
            }
        }
    }
    void OnEnable()
    {
        InputManager.OnLeftClick += Interact;
        InputManager.OnRightClick += ReadyToInteract;
        InputManager.OnPickup += Pickup;
        InputManager.OnRotation += RotateCarryable;
    }


    void OnDisable()
    {
        InputManager.OnLeftClick -= Interact;
        InputManager.OnRightClick -= ReadyToInteract;
        InputManager.OnPickup -= Pickup;
        InputManager.OnRotation -= RotateCarryable;
    }
    void OnDestroy()
    {
        InputManager.OnLeftClick -= Interact;
        InputManager.OnRightClick -= ReadyToInteract;
        InputManager.OnPickup -= Pickup;
        InputManager.OnRotation -= RotateCarryable;
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
