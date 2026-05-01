
using System;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private PlayerLocomotionController movementController;
    private InteractionController interactionController;
    void Awake()
    {
        movementController = GetComponent<PlayerLocomotionController>();
        movementController.OnVelocity += Velocity;

        interactionController = GetComponent<InteractionController>();
        interactionController.OnInteract += Interact;
    }

    private void Interact(InteractState state)
    {
        switch (state)
        {
            case InteractState.InteractReady:
                anim.SetBool("interactReady", true);
                break;
            case InteractState.InteractReadyNegative:
                anim.SetBool("interactReady", false);
                break;
            case InteractState.InteractCharge:
                anim.SetBool("interactCharge", true);
                break;
            case InteractState.InteractChargeNegative:
                anim.SetBool("interactCharge", false);
                break;

        }
    }

    private void Velocity(float obj)
    {
        anim.SetFloat("velocity", obj);
    }


}
