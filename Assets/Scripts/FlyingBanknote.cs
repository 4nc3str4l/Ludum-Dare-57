using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FlyingBanknote : MonoBehaviour
{

    public Camera PlayerCamera;
    public Transform InitialFallPosition;

    public BanknotePathPoint[] GoToCheckpoint2Path;
    public BanknotePathPoint[] GoToCheckpoint3Path;

    private void Start()
    {
        int currentCheckpoint = CheckpointManager.Instance.CurrentCheckpoint;
        if (currentCheckpoint > 0)
        {
            transform.position = CheckpointManager.Instance
                .GetMoneyForCheckpointIdx(CheckpointManager.Instance.CurrentCheckpoint).position;

            if (currentCheckpoint > 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void FallFromPlayer(Action onComplete)
    {
        transform.position = PlayerCamera.transform.position;
        transform.DOMove(InitialFallPosition.position, 1.5f)
            .OnComplete(() => onComplete());
    }

    public void ExecuteSecondPath()
    {
        ExecutePath(GoToCheckpoint2Path);
    }
    
    public void ExecuteGoTo3()
    {
        ExecutePath(GoToCheckpoint3Path, 0, () =>
        {
            gameObject.SetActive(false);
        });
    }

    private void ExecutePath(BanknotePathPoint[] pathPoints, int index = 0, Action onComplete = null)
    {
        BanknotePathPoint info = pathPoints[index];
        transform.DOMove(info.transform.position, info.timeToReach).OnComplete(()=>
        {
            if (index < pathPoints.Length - 1)
            {
                if (info.waitAfterReaching == 0.0f)
                {
                    ExecutePath(pathPoints, ++index);
                }
                else
                {
                    Delay(() =>
                    {
                        ExecutePath(pathPoints, ++index);
                    }, info.waitAfterReaching);
                }
            }
            else
            {
                onComplete?.Invoke();
            }
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
    
    IEnumerator WaitAndExecute(Action _toExecute , float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _toExecute?.Invoke();
    }
}
