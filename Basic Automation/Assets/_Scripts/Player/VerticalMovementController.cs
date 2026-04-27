using UnityEngine;

public class VerticalMovementController : MonoBehaviour
{
    public float rideHeight { get; set; }
    public float rideSpringStrength { get; set; }
    public float rideSpringDamper { get; set; }
    public Vector3 VerticalDirection { get; private set; }
    public float VerticalOffSetForce { get; private set; } = 1f;

    Ray ray;

    public void VerticalMovement(Rigidbody rb, float rideHeightFeed)
    {
        ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 vel = rb.linearVelocity;
            Vector3 rayDir = -transform.up;

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;

            if (hitBody != null)
                otherVel = hitBody.linearVelocity;

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - rideHeightFeed;
            VerticalOffSetForce = x * rideSpringStrength - relVel * rideSpringDamper;
            VerticalDirection = rayDir;

        }
        else
            VerticalDirection = -Physics.gravity / VerticalOffSetForce;
    }
    public void ApplyLogic(Rigidbody rb)
    {
        rb.AddForce(VerticalDirection * VerticalOffSetForce);

    }

}
