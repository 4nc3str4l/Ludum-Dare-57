using UnityEngine;

public class MoveToTransform : MonoBehaviour
{
    public Transform TargetParent;
    
    void Start()
    {
        transform.SetParent(TargetParent);
    }
}
