using UnityEngine;

public class CanyonFloor : MonoBehaviour
{

    public delegate void OnPlayerCollisionHandler();
    public static event OnPlayerCollisionHandler OnPlayerCollision;
    
    private void OnCollisionEnter(Collision other)
    {
        OnPlayerCollision?.Invoke();
    }
}
