using UnityEngine;

[RequireComponent(typeof(VerticalMovementController),typeof(HorizontalMovementController),typeof(RotationController))]
public class PlayerLocomotionController : MonoBehaviour
{
    [SerializeField] private LocomotionSO movementData;
    private VerticalMovementController verticalController;
    private HorizontalMovementController horizontalController;
    private RotationController rotationController;

    private Ray ray;
    private Rigidbody rb;

    private Vector3 lookDirection;
    private Quaternion upRight;
    private Camera cam;
    void Awake()
    {

        verticalController = GetComponent<VerticalMovementController>();
        horizontalController = GetComponent<HorizontalMovementController>();
        rotationController = GetComponent<RotationController>();
        rb = GetComponent<Rigidbody>();

        SetMovementData();
      
        cam = Camera.main;
    }
    private void SetMovementData()
    {
        verticalController.rideHeight = movementData.RideHeight;
        verticalController.rideSpringDamper = movementData.RideSpringDamper;
        verticalController.rideSpringStrength = movementData.RideSpringStrength;

        horizontalController.maxForce = movementData.MaxForce;
        horizontalController.acceleration = movementData.Acceleration;
        horizontalController.accelerationFactorFromDot = movementData.AccelerationFactorFromDot;
        horizontalController.maxAccelForce = movementData.MaxAccelForce;
        horizontalController.MaxAccelerationForceFactorFromDot = movementData.MaxAccelerationForceFactorFromDot;
        horizontalController.forceScale = movementData.ForceScale;

        rotationController.uprightCorrectionDamper = movementData.UprightCorrectionDamper;
        rotationController.uprightCorrectionStrength = movementData.UprightCorrectionStrength;

        rb.mass = movementData.Mass;
        rb.interpolation = movementData.Interpolate;
        rb.collisionDetectionMode = movementData.CollisionDetection;
        rb.constraints = movementData.Constraints;
        rb.isKinematic = movementData.IsKinematic;
        rb.linearDamping = movementData.LinearDamping;
    }
    void Update()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        Vector3 right = cam.transform.right;
        right.y = 0;
        lookDirection = right * InputManager.Instance.MovementVector().x + forward * InputManager.Instance.MovementVector().y;
        lookDirection.y = 0;
        lookDirection.Normalize();
        if (InputManager.Instance.MovementVector().magnitude > 0.001f)
        {
            upRight = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
        else
            upRight = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    void FixedUpdate()
    {
        verticalController.VerticalMovement(rb, verticalController.rideHeight);
        horizontalController.HorizontalMovement(lookDirection, rb);
        rotationController.UpdateUprightForce(upRight);

        verticalController.ApplyLogic(rb);
        horizontalController.ApplyLogic(rb);
        rotationController.ApplyLogic(rb);
    }



}
