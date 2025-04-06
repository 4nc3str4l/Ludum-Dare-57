using UnityEngine;

public class Cristal : MonoBehaviour
{
    private Light m_Light;

    private void Awake()
    {
        m_Light = GetComponentInChildren<Light>(true);
    }

    public void SetHighlighted(bool highlighted)
    {
        m_Light.enabled = highlighted;
        m_Light.gameObject.SetActive(highlighted);
    }
}
