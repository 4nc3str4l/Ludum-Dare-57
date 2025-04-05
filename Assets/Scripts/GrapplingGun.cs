using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;

    private Vector3 grapplePoint;
    private SpringJoint joint;
    private readonly float maxDistance = 100f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
           StopGrapple();
        }
    }

    /// <summary>
    ///     Call whenever we want to start a grapple
    /// </summary>
    private void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            var distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
    }
    
    private void StopGrapple()
    {
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}