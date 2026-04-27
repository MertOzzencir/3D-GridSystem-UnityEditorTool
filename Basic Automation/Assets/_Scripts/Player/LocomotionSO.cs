using UnityEngine;

[CreateAssetMenu(fileName = "Create Movement Data", menuName = "Create Movement Data/New Movement Data")]
public class LocomotionSO : ScriptableObject
{
    [Header("Locomotion")]
    public float MaxForce;
    public float Acceleration;
    public AnimationCurve AccelerationFactorFromDot;
    public float MaxAccelForce;
    public AnimationCurve MaxAccelerationForceFactorFromDot;
    public Vector3 ForceScale;

    [Header("Rotation")]
    public float UprightCorrectionStrength;
    public float UprightCorrectionDamper;

    [Header("Vertical")]
    public float RideHeight;
    public float RideSpringStrength;
    public float RideSpringDamper;
    public float RideHeightMultiplier;

    [Header("Rigidbody")]
    public float Mass;
    public RigidbodyInterpolation Interpolate;
    public CollisionDetectionMode CollisionDetection;
    public RigidbodyConstraints Constraints;
    public bool IsKinematic;
    public float LinearDamping;

}
