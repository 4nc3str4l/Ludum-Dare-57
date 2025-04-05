using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIProgressBar StaminaProgressBar;
    public FullScreenColorFader Screenfader;
    public Color DeathColor;

    private void OnEnable()
    {
        GrapplingStamina.OnStaminaChanged += GrapplingStaminaOnOnStaminaChanged;
        CanyonFloor.OnPlayerCollision += CanyonFloorOnPlayerCollision;
    }
    
    private void OnDisable()
    {
        GrapplingStamina.OnStaminaChanged -= GrapplingStaminaOnOnStaminaChanged;
        CanyonFloor.OnPlayerCollision -= CanyonFloorOnPlayerCollision;
    }
    
    private void CanyonFloorOnPlayerCollision()
    {
        Screenfader.FadeIn(DeathColor, 0.35f);
    }
    
    private void GrapplingStaminaOnOnStaminaChanged(float stamina, float ratio)
    {   
        StaminaProgressBar.SetFilling(ratio);
    }

}
