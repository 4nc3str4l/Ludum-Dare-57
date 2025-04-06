using UnityEngine;

public class HangingBucket : MonoBehaviour
{
    
    private Rigidbody m_Rigidbody;
    public SpringJoint Handle;
    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void PushRandomly()
    {
        var randomHandlePos = UnityEngine.Random.insideUnitSphere * 50.0f;
        randomHandlePos.y = 0;
        Handle.transform.position += randomHandlePos;
    }
}
