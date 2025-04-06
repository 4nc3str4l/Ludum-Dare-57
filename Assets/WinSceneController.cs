using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinSceneController : MonoBehaviour
{
    public TMP_Text HappyText;
    public TMP_Text WoopsText;
    public Button MainMenu;
    
    private void Start()
    {
        MainMenu.gameObject.SetActive(false);
        WoopsText.enabled = false;
        TMPTypewriter.Instance.TypeBySpeed(HappyText, HappyText.text, 20.0f, OnHappyTextComplete);
    }

    private void OnHappyTextComplete()
    {
     
        StartCoroutine(
            WaitAndExecute(() =>
            {
                WoopsText.enabled = true;
                TMPTypewriter.Instance.TypeBySpeed(WoopsText, WoopsText.text, 20.0f, OnUnHappyTextComplete);
            }, 1.2f)
        );   
        
    }

    private void OnUnHappyTextComplete()
    {
        MainMenu.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    
    IEnumerator WaitAndExecute(Action _toExecute , float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _toExecute?.Invoke();
    }
}
