using System;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    
    public Transform ObjectToFollow;
    private Vector3 m_InitialDeltaToTarget;

    private void Start()
    {
        ObjectToFollow.transform.SetParent(null);
        m_InitialDeltaToTarget = transform.position - ObjectToFollow.position;
    }

    private void LateUpdate()
    {
        transform.position = ObjectToFollow.position + m_InitialDeltaToTarget;
    }
}
