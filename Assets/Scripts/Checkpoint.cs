using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int Index = 0;
    
    private bool IsTriggered = false;

    public Transform PlayerTransformPosition;
    public Transform MoneyTransformPosition;

    private void Awake()
    {
        PlayerTransformPosition.GetComponent<Renderer>().enabled = false;
        MoneyTransformPosition.GetComponent<Renderer>().enabled = false; 
        gameObject.GetComponent<Renderer>().enabled = false;
        CheckpointManager.Instance.RegisterCheckpoint(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<PlayerMovement>() != null)
        {
            if (Index > CheckpointManager.Instance.CurrentCheckpoint)
            {
                CheckpointManager.Instance.FireOnCheckpointTriggered(Index);
            }
        }
    }
}
