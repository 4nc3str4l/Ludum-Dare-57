using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerZone : MonoBehaviour
{
    public UnityEvent Trigger;
    
    private bool HasAlreadyTriggered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!HasAlreadyTriggered && other.GetComponentInChildren<PlayerMovement>() != null)
        {
            HasAlreadyTriggered = true;
            Trigger?.Invoke(); 
        }
    }
}