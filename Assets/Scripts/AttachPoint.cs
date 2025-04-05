using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AttachPoint : MonoBehaviour
{
    public Transform HandlingObject;
    public LineRenderer LineR;
    
    void Awake()
    {
        if (LineR != null)
        {
            LineR.positionCount = 2;
        }
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (LineR != null && HandlingObject != null)
        {
            LineR.SetPosition(0, transform.position);
            LineR.SetPosition(1, HandlingObject.position);
        }
    }
}