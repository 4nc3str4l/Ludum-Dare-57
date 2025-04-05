using System;
using UnityEngine;

public class GrapplingStamina : MonoBehaviour
{
    public float WhileGrapplingFactor = -1;
    public float WhileNotGrapplingFactor = 2;

    public float MaxStamina = 100;
    public float CurrentStamina = 100;

    private float m_LastFrameStamina = 100;
    
    private bool m_IsGrappling = false;
    
    public delegate void OnStaminaChangedHandler(float stamina, float ratio);
    public static event OnStaminaChangedHandler OnStaminaChanged;
    
    public delegate void OnStaminaAtMaxHandler();
    public static event OnStaminaAtMaxHandler OnStaminaAtMax;
    
    public delegate void OnStaminaEmptyHandler();
    public static event OnStaminaEmptyHandler OnStaminaEmpty;

    public static bool HasSomeStamina = true;

    private void Awake()
    {
        HasSomeStamina = true;
        CurrentStamina = MaxStamina;
        m_LastFrameStamina = CurrentStamina;
    }

    private void OnEnable()
    {
        GrapplingGun.OnGrappling += GrapplingGunOnOnGrappling;
    }
    
    private void OnDisable()
    {
        GrapplingGun.OnGrappling -= GrapplingGunOnOnGrappling;
    }

    private void GrapplingGunOnOnGrappling(bool isgrappling)
    {
        m_IsGrappling = isgrappling;
    }
    
    private void Update()
    {
        float staminaDelta = WhileNotGrapplingFactor;
        
        if (m_IsGrappling)
        {
            staminaDelta = WhileGrapplingFactor;
        }    
        
        CurrentStamina += staminaDelta * Time.deltaTime;

        if (CurrentStamina != m_LastFrameStamina)
        {
            if (CurrentStamina > MaxStamina)
            {
                CurrentStamina = MaxStamina;
                OnStaminaAtMax?.Invoke();
            }
            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                OnStaminaEmpty?.Invoke();
            }
            if (Math.Abs(CurrentStamina - m_LastFrameStamina) < 1)
            {
                OnStaminaChanged?.Invoke(CurrentStamina, CurrentStamina / MaxStamina);
            }
        }

        HasSomeStamina = CurrentStamina > 0.0f;
        m_LastFrameStamina = CurrentStamina;
    }
}
