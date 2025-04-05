using UnityEngine;

public class GrapplingTarget : MonoBehaviour
{ 
    private Material m_Material;
    public Material HighLightMaterial;
    public Renderer MyRenderer;
    
    private Transform m_AttachPoint;
    
    public Transform AttachPoint => m_AttachPoint;
    
    private void Awake()
    {
        m_Material = MyRenderer.material;
    }

    public void Highlight()
    {
        MyRenderer.material = HighLightMaterial;
    }

    public void Unhighlight()
    {
        MyRenderer.material = m_Material;
    }

    public void SetAttachPoint(Vector3 point)
    {
        if (m_AttachPoint == null)
        {
            GameObject go = new GameObject();
            go.name = "AttachPoint";
            m_AttachPoint = go.transform;
        }
        m_AttachPoint.SetParent(null);
        m_AttachPoint.position = point;
        m_AttachPoint.SetParent(transform);
    }
}
