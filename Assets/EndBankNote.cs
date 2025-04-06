using System;
using DG.Tweening;
using UnityEngine;

public class EndBankNote : MonoBehaviour
{
    public LayerMask layerMask;

    public float MinPlayerDistance = 1.5f;

    private bool m_EnableInteraction = false;
    
    private Camera m_PlayerCamera;
    
    private bool m_WasBeingRaycasted = false;

    public delegate void FinalBanknoteRaycastedHandler(bool raycasted);
    public static event FinalBanknoteRaycastedHandler OnFinalBanknoteRaycasted;

    public delegate void HandleFinalBanknoteTaken();
    public static event HandleFinalBanknoteTaken OnFinalBanknoteTaken;

    private void Start()
    {
        if (CheckpointManager.Instance.CurrentCheckpoint == 4)
        {
            EnableInteraction();
        }
    }

    private void EnableInteraction()
    {
        m_EnableInteraction = true;
        m_PlayerCamera = Camera.main;
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
        if (checkpoint == 4)
        {
            EnableInteraction();
        }
    }
    
    public void Update()
    {
        if (!m_EnableInteraction)
        {
            return;
        }
        
        RaycastHit hit;
        bool beingRaycasted = false;
        if (Physics.Raycast(m_PlayerCamera.transform.position, m_PlayerCamera.transform.forward, out hit,
                MinPlayerDistance, layerMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                beingRaycasted = true;
            }
        }
        
        if (m_WasBeingRaycasted != beingRaycasted)
        {
            OnFinalBanknoteRaycasted?.Invoke(beingRaycasted);
        }
        
        if (beingRaycasted && Input.GetKeyDown(KeyCode.E))
        {
            MoveToPlayer();
            OnFinalBanknoteTaken?.Invoke();
        }   
    }

    private void MoveToPlayer()
    {
        m_EnableInteraction = false;
        transform.DOMove(Camera.main.transform.position, 1.2f);
        transform.DOShakeScale(0.2f, 0.1f);
    }
}
