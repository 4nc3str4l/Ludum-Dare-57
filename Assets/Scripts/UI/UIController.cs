using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIProgressBar StaminaProgressBar;
    public FullScreenColorFader Screenfader;
    public Color DeathColor;
    public GameObject PressETograb;
    
    private void OnEnable()
    {
        GrapplingStamina.OnStaminaChanged += GrapplingStaminaOnOnStaminaChanged;
        CanyonFloor.OnPlayerCollision += CanyonFloorOnPlayerCollision;
        EndBankNote.OnFinalBanknoteRaycasted += EndBankNoteOnOnFinalBanknoteRaycasted;
        PressETograb.SetActive(false);
    }

    private void OnDisable()
    {
        GrapplingStamina.OnStaminaChanged -= GrapplingStaminaOnOnStaminaChanged;
        CanyonFloor.OnPlayerCollision -= CanyonFloorOnPlayerCollision;
        EndBankNote.OnFinalBanknoteRaycasted -= EndBankNoteOnOnFinalBanknoteRaycasted;
    }
    
    private void EndBankNoteOnOnFinalBanknoteRaycasted(bool raycasted)
    {
        PressETograb.SetActive(raycasted);
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
