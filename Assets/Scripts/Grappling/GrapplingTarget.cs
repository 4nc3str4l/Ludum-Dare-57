using UnityEngine;

public class GrapplingTarget : MonoBehaviour
{ 
    private Material m_Material;
    public Material HighLightMaterial;
    public Renderer MyRenderer;
    
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
}
