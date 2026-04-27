using UnityEngine;

public class RotationController : MonoBehaviour
{
    [Header("Rotation")]

    public float uprightCorrectionStrength{get;set;}
    public float uprightCorrectionDamper{get;set;}
    private Vector3 rotAxis;
    private float rotRadians;
   
    public void UpdateUprightForce(Quaternion uprightsCorrection)
    {
        Quaternion characterRotation = transform.rotation;
        Quaternion toGoal = uprightsCorrection * Quaternion.Inverse(characterRotation);
        if (toGoal.w < 0)
        {
            toGoal.x = -toGoal.x;
            toGoal.y = -toGoal.y;
            toGoal.z = -toGoal.z;
            toGoal.w = -toGoal.w;
        }

        float rogDegress;
        toGoal.ToAngleAxis(out rogDegress, out rotAxis);
        rotAxis.Normalize();
        rotRadians = rogDegress * Mathf.Deg2Rad;
    }

    public void ApplyLogic(Rigidbody rb)
    {
        rb.AddTorque((rotAxis * (rotRadians * uprightCorrectionStrength)) - (rb.angularVelocity * uprightCorrectionDamper));
    }
}
