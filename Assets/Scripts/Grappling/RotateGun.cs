using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grappling;

    private Quaternion desiredRotation;
    private readonly float rotationSpeed = 5f;

    private void Update()
    {
        if (!grappling.IsGrappling())
        {
            
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(grappling.GetTargetInSight().AttachPoint.position - transform.position);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}