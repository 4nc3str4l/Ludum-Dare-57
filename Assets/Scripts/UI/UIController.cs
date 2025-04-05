using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIProgressBar StaminaProgressBar;

    private void OnEnable()
    {
        GrapplingStamina.OnStaminaChanged += GrapplingStaminaOnOnStaminaChanged;
    }

    private void OnDisable()
    {
        GrapplingStamina.OnStaminaChanged -= GrapplingStaminaOnOnStaminaChanged;
    }
    
    private void GrapplingStaminaOnOnStaminaChanged(float stamina, float ratio)
    {   
        StaminaProgressBar.SetFilling(ratio);
    }

}
