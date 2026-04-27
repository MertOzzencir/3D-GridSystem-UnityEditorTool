using UnityEngine;

public class HorizontalMovementController : MonoBehaviour
{
    [Header("Locomotion")]
    public float maxForce { get; set; }
    public float acceleration { get; set; }
    public AnimationCurve accelerationFactorFromDot { get; set; }
    public float maxAccelForce { get; set; }
    public AnimationCurve MaxAccelerationForceFactorFromDot { get; set; }
    public Vector3 forceScale { get; set; }
    private Vector3 mGoalVel;
    private Vector3 LocoMovement;

    public void HorizontalMovement(Vector3 directionMovement, Rigidbody rb)
    {
        Vector3 currentVelocity = mGoalVel.normalized;

        float velDot = Vector3.Dot(directionMovement, currentVelocity);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velDot);

        Vector3 goalVel = directionMovement * maxForce;
        mGoalVel = Vector3.MoveTowards(mGoalVel, goalVel, accel * Time.fixedDeltaTime);

        LocoMovement = (mGoalVel - rb.linearVelocity) / Time.fixedDeltaTime;

        float maxAccel = maxAccelForce * MaxAccelerationForceFactorFromDot.Evaluate(velDot);
        LocoMovement = Vector3.ClampMagnitude(LocoMovement, maxAccel);
    }
    public void ApplyLogic(Rigidbody rb)
    {
        rb.AddForce(Vector3.Scale(LocoMovement * rb.mass, forceScale));
    }

}
