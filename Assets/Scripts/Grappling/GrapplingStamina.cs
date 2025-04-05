using System;
using UnityEngine;

public class GrapplingStamina : MonoBehaviour
{
    [Header("Stamina Factors")]
    public float BaseWhileGrapplingFactor = -1f;     
    public float BaseWhileNotGrapplingFactor = 2f;   
    
    [Header("Acceleration Settings")]
    public float MaxAccelerationMultiplier = 3f;     
    public float AccelerationRate = 0.5f;            
    
    [Header("Stamina Settings")]
    public float MaxStamina = 100f;
    public float CurrentStamina = 100f;

    private float m_LastFrameStamina = 100f;
    private bool m_IsGrappling = false;
    

    private float m_CurrentAcceleration = 1f;   
    private float m_TimeInCurrentState = 0f;  
    
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
        m_CurrentAcceleration = 1f;
        m_TimeInCurrentState = 0f;
    }

    private void OnEnable()
    {
        GrapplingGun.OnGrappling += GrapplingGunOnOnGrappling;
    }
    
    private void OnDisable()
    {
        GrapplingGun.OnGrappling -= GrapplingGunOnOnGrappling;
    }

    private void GrapplingGunOnOnGrappling(bool isGrappling)
    {
        if (m_IsGrappling != isGrappling)
        {
            m_IsGrappling = isGrappling;
            m_CurrentAcceleration = 1f;
            m_TimeInCurrentState = 0f;
        }
    }
    
    private void Update()
    {
        m_TimeInCurrentState += Time.deltaTime;
        
        m_CurrentAcceleration = Mathf.Min(1f + (m_TimeInCurrentState * AccelerationRate), MaxAccelerationMultiplier);
        
        float baseStaminaDelta = m_IsGrappling ? BaseWhileGrapplingFactor : BaseWhileNotGrapplingFactor;
        
        float staminaDelta = baseStaminaDelta * m_CurrentAcceleration;
        
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
            
            if (Math.Abs(CurrentStamina - m_LastFrameStamina) > 0.01f)
            {
                OnStaminaChanged?.Invoke(CurrentStamina, CurrentStamina / MaxStamina);
            }
        }

        HasSomeStamina = CurrentStamina > 0.0f;
        m_LastFrameStamina = CurrentStamina;
    }
    
    public float GetCurrentAccelerationMultiplier()
    {
        return m_CurrentAcceleration;
    }
}