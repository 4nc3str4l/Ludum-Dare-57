using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerZone : MonoBehaviour
{
    public UnityEvent Trigger;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<PlayerMovement>() != null)
        {
            Debug.Log("Is it firing??");
           Trigger?.Invoke(); 
        }
    }
}