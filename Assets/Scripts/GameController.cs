using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        CanyonFloor.OnPlayerCollision += CanyonFloorOnonPlayerCollision;

    }

    private void OnDisable()
    {
        CanyonFloor.OnPlayerCollision -= CanyonFloorOnonPlayerCollision;
    }
    
    void CanyonFloorOnonPlayerCollision()
    {
        JukeBox.Instance.PlaySound(JukeBox.Instance.Hit, 0.8f);
        JukeBox.Instance.PlaySound(JukeBox.Instance.Death, 0.5f);
        
        StartCoroutine(
            WaitAndExecute(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }, 0.5f)
        );
    }
    
    IEnumerator WaitAndExecute(Action _toExecute , float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _toExecute?.Invoke();
    }
}
