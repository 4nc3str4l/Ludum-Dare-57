using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    public static GameController Instance;
    
    public Transform PlayerSpawnPosition;
    public Transform MoneySpawnPosition;

    private void Awake()
    {
        Instance = this;
        PlayerSpawnPosition = CheckpointManager.Instance.GetPlayerPositionForCheckpointIdx(CheckpointManager.Instance.CurrentCheckpoint);
        MoneySpawnPosition = CheckpointManager.Instance.GetMoneyForCheckpointIdx(CheckpointManager.Instance.CurrentCheckpoint);
    }

    private void OnEnable()
    {
        CanyonFloor.OnPlayerCollision += CanyonFloorOnonPlayerCollision;
        EndBankNote.OnFinalBanknoteTaken += EndBankNoteOnOnFinalBanknoteTaken;

    }

    private void EndBankNoteOnOnFinalBanknoteTaken()
    {
        CheckpointManager.Instance.ResetCheckpoints();
        JukeBox.Instance.PlaySound(JukeBox.Instance.Hellyeah, 0.2f);
        StartCoroutine(
            WaitAndExecute(() => {SceneManager.LoadScene("WinScene"); }, JukeBox.Instance.Hellyeah.length)
        );

    }

    private void OnDisable()
    {
        CanyonFloor.OnPlayerCollision -= CanyonFloorOnonPlayerCollision;
        EndBankNote.OnFinalBanknoteTaken += EndBankNoteOnOnFinalBanknoteTaken;
    }

    private void Start()
    {
        JukeBox.Instance.EnableAudioSource(JukeBox.Instance.WindAudioSource, 0.5f);
    }

    void CanyonFloorOnonPlayerCollision()
    {
        JukeBox.Instance.PlaySound(JukeBox.Instance.Hit, 0.8f);
        JukeBox.Instance.PlaySound(JukeBox.Instance.Death, 0.5f);
        
        StartCoroutine(
            WaitAndExecute(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }, 0.5f)
        );
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            CheckpointManager.Instance.ResetCheckpoints();
        }   
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(
                WaitAndExecute(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }, 0.2f)
            );
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    IEnumerator WaitAndExecute(Action _toExecute , float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _toExecute?.Invoke();
    }
}
