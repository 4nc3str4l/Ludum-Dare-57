using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{

    public bool Cheat = true;
    public int CheatOverrideCheckpoint = 0;

    public static CheckpointManager Instance;
    
    public delegate void OnCheckpointTriggeredHandler(int checkpoint);
    
    public static event OnCheckpointTriggeredHandler OnCheckpointTriggered;
    
    private List<Checkpoint> m_Checkpoints = new List<Checkpoint>();

    public int CurrentCheckpoint { get; private set; } = 0;

    private void Awake()
    {
        Instance = this;

        if (Cheat)
        {
            CurrentCheckpoint = CheatOverrideCheckpoint;
        }
        else
        {
            if(!PlayerPrefs.HasKey("checkpoint"))
            {
                PlayerPrefs.SetFloat("checkpoint", CurrentCheckpoint);
            }
            CurrentCheckpoint = PlayerPrefs.GetInt("checkpoint");
        }
    }
    
    // Note: Call this when the game is won
    public void ResetCheckpoints()
    {
        CurrentCheckpoint = 0;
        PlayerPrefs.DeleteKey("checkpoint");
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey("checkpoint");
    }

    public void FireOnCheckpointTriggered(int checkpoint)
    {
        Debug.Log($"FireOnCheckpointTriggered {checkpoint}");
        OnCheckpointTriggered?.Invoke(checkpoint);
        CurrentCheckpoint = checkpoint;
        PlayerPrefs.SetInt("checkpoint", checkpoint);
    }

    public void RegisterCheckpoint(Checkpoint _checkpoint)
    {
        m_Checkpoints.Add(_checkpoint);
    }

    public Transform GetPlayerPositionForCheckpointIdx(int _checkpointIdx)
    {
        Transform transformToReturn =null;
        foreach(Checkpoint c in m_Checkpoints)
        {
            if (c.Index == _checkpointIdx)
            {
                transformToReturn = c.PlayerTransformPosition;
                break;
            }
        }
        return transformToReturn;
    }

    public Transform GetMoneyForCheckpointIdx(int _checkpointIdx)
    {
        Transform transformToReturn =null;
        foreach(Checkpoint c in m_Checkpoints)
        {
            transformToReturn = c.MoneyTransformPosition;
            break;
        }
        return transformToReturn;
    }
}
