using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    public Image ForegroundImage;
    
    public void SetFilling(float fill)
    {
        ForegroundImage.fillAmount = fill;
    }
}
