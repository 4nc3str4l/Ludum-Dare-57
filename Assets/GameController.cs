using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
