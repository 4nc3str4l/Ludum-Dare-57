using System;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;

    private Vector3 grapplePoint;
    private SpringJoint joint;
    private readonly float maxDistance = 100f;

    public delegate void OnGrapplingHandler(bool isGrappling);
    public static event OnGrapplingHandler OnGrappling;

    private GrapplingTarget m_TargetInSight = null;
    private RaycastHit m_LastHit;
    
    public delegate void OnShootHandler();

    public event OnShootHandler OnShoot;

    private void OnEnable()
    {
        GrapplingStamina.OnStaminaEmpty += GrapplingStaminaOnOnStaminaEmpty;
    }
    
    
    private void OnDisable()
    {
        GrapplingStamina.OnStaminaEmpty -= GrapplingStaminaOnOnStaminaEmpty;
    }

    private void GrapplingStaminaOnOnStaminaEmpty()
    {
        if(IsGrappling())
        {
            StopGrapple();
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GrapplingStamina.HasSomeStamina)
        {
            StartGrapple();
        }
        
        if (Input.GetMouseButtonUp(0) && IsGrappling()) 
        {
           StopGrapple();
        }
    }

    private void FixedUpdate()
    {
        if (IsGrappling())
        {
            joint.connectedAnchor = m_TargetInSight.AttachPoint.position;
            Debug.Log(joint.connectedAnchor);
            return;
        }
        
        if (Physics.Raycast(camera.position, camera.forward, out m_LastHit, maxDistance, whatIsGrappleable))
        {
            m_TargetInSight = m_LastHit.collider.gameObject.GetComponent<GrapplingTarget>();
            m_TargetInSight?.Highlight();
        }
        else
        {
            if (m_TargetInSight != null)
            {
                m_TargetInSight.Unhighlight();
            }
            m_TargetInSight = null;
        }
        
    }

    /// <summary>
    ///     Call whenever we want to start a grapple
    /// </summary>
    private void StartGrapple()
    {
        if (m_TargetInSight == null)
        {
            return;
        }
        
        m_TargetInSight.Unhighlight();
        grapplePoint = m_LastHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        var distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
        
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = 10f;
        joint.damper = 7f;
        joint.massScale = 4.5f;
        
        OnGrappling?.Invoke(true);
        OnShoot?.Invoke();
        m_TargetInSight.SetAttachPoint(m_LastHit.point);
    }
    
    private void StopGrapple()
    {
        RemoveJoin();
        OnGrappling?.Invoke(false);
    }

    private void RemoveJoin()
    {
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return m_TargetInSight.AttachPoint.position;
    }
}