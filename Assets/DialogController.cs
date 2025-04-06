using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public string WoopsIDropped50Bucks = "Woops, I dropped 50 bucks";
    public string NoooComeBack = "Nooo!!! Come Back, I need them to have lunch after the hike!!!";

    public string CheckpointReached = "Checkpoint Reached";
    public string OhhManThisWind = "Ohhh man this wind... :(";

    public string IThoughtIWouldDie = "Wow I thought I would die!!! All for 50 bucks, where are them???!!";
    
    public string WTFIsThisPlace = "Wtf is this place? And where is my ***** money!!! I am hungry!!";
    
    
    public string YesHereTheyAre = "Yayyy here you are! I am hungry!";

    public TMP_Text TxtDialog;
    
    public FlyingBanknote Banknote;

    private int m_CheckpointAlreadyProcessed = -1;
    
    private void Awake()
    {
        TxtDialog.enabled = false;
        TxtDialog.text = "";
    }

    private void Start()
    {
        ProcessCheckpoint(CheckpointManager.Instance.CurrentCheckpoint, true);
    }

    private void OnEnable()
    {
        CheckpointManager.OnCheckpointTriggered += CheckpointManagerOnOnCheckpointTriggered;
    }

    private void OnDisable()
    {
        CheckpointManager.OnCheckpointTriggered -= CheckpointManagerOnOnCheckpointTriggered;
    }
    
    private void CheckpointManagerOnOnCheckpointTriggered(int checkpoint)
    {
        ProcessCheckpoint(checkpoint, false);
    }

    private void ProcessCheckpoint(int checkpoint, bool artificial)
    {
        Debug.Log($"Processing checkpoint: {checkpoint} artificial: {artificial} lastProceesed: {m_CheckpointAlreadyProcessed}");
        if (checkpoint == m_CheckpointAlreadyProcessed)
        {
            return;
        }

        if (!artificial)
        {
            TMPTypewriter.Instance.TypeBySpeed(TxtDialog, CheckpointReached, 20.0f, () =>
            {
                ExecuteCheckpointLogic(checkpoint);
            });
        }
        else
        {
            ExecuteCheckpointLogic(checkpoint);
        }
    }

    private void ExecuteCheckpointLogic(int checkpoint)
    {
        switch (checkpoint)
        {
            case 0:
                InitialDialog();
                break;
            case 1:
                SecondDialog();
                break;
            case 2:
                ThirdDialog();
                break; 
            case 3:
                ForthDialog();
                break;
            case 4:
                FithDialog();
                break;
        }
        m_CheckpointAlreadyProcessed = checkpoint;
    }

    private void InitialDialog()
    {
        Banknote.FallFromPlayer((() =>
        {
            Delay(
                ()=>TMPTypewriter.Instance.TypeBySpeed(TxtDialog, WoopsIDropped50Bucks, 20.0f, () =>
                {
                    Delay(
                        () =>
                        {
                            Banknote.ExecuteSecondPath();
                            JukeBox.Instance.PlaySound(JukeBox.Instance.Noo, 0.5f);
                            TMPTypewriter.Instance.TypeBySpeed(TxtDialog, NoooComeBack, 20.0f, () =>
                            {
                                HideTextAfterDelay(2.0f);
                            });
                        }, 2.0f);
                })
                , 1.2f);
        }));
    }

    private void SecondDialog()
    {
        Delay(()=>
        {
            Banknote.ExecuteGoTo3();
            TMPTypewriter.Instance.TypeBySpeed(TxtDialog, OhhManThisWind, 20.0f, () =>
            {
                HideTextAfterDelay(3.0f);
            });
        }, 2.0f);   
    }

    private void ThirdDialog()
    {
        TMPTypewriter.Instance.TypeBySpeed(TxtDialog, IThoughtIWouldDie, 20.0f, () =>
        {
            HideTextAfterDelay(3.0f);
        });
    }
    
    private void ForthDialog()
    {
        TMPTypewriter.Instance.TypeBySpeed(TxtDialog, WTFIsThisPlace, 20.0f, () =>
        {
            HideTextAfterDelay(3.0f);
        });
    }

    private void FithDialog()
    {
        TMPTypewriter.Instance.TypeBySpeed(TxtDialog, YesHereTheyAre, 20.0f, () =>
        {
            HideTextAfterDelay(3.0f);
        });
    }

    
    private void Delay(Action action, float delay)
    {
        StartCoroutine(
            WaitAndExecute(() =>
            {
                action?.Invoke();
            }, delay)
        );   
    }

    private void HideTextAfterDelay(float delay)
    {
        Delay(() => TxtDialog.enabled = false, delay);
    }
    
    IEnumerator WaitAndExecute(Action _toExecute , float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _toExecute?.Invoke();
    }
}
