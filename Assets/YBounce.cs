using UnityEngine;

public class YBounce : MonoBehaviour
{
    
    private Vector3 m_StartingPosition;
    public float Amplitude = 5;
    public float Speed = 1;

    private void Awake()
    {
        m_StartingPosition = transform.position;
    }

    private void Update()
    {
        transform.position = m_StartingPosition +  Vector3.up * (Mathf.Sin(Time.time * Speed) * Amplitude);
    }
}
