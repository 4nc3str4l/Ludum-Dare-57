using DG.Tweening;
using UnityEngine;

public class GrapplingTarget : MonoBehaviour
{ 
    private Material m_Material;
    public Material HighLightMaterial;
    public Renderer MyRenderer;
    
    private Transform m_AttachPoint;

    private Vector3 m_InitialScale;

    public SpringJoint AttachedSpring = null;
    
    public Transform AttachPoint => m_AttachPoint;
    
    private bool m_IsAttached = false;
    
    private void Awake()
    {
        m_InitialScale = transform.localScale;
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

    public void SetAttachPoint(Vector3 point, Vector3 playerDirection)
    {
        m_IsAttached = false;
        if (m_AttachPoint == null)
        {
            GameObject go = new GameObject();
            go.name = "AttachPoint";
            m_AttachPoint = go.transform;
        }
        m_AttachPoint.SetParent(null);
        m_AttachPoint.position = point;
        m_AttachPoint.SetParent(transform);


        if (AttachedSpring != null)
        {
            AttachedSpring.GetComponent<Rigidbody>().AddForce(playerDirection * 500);
        }
    }

    public void OnAttached()
    {
        if (m_IsAttached)
        {
            return;
        }
        m_IsAttached = true;
        JukeBox.Instance.PlaySound(JukeBox.Instance.Hit, 0.3f);
        transform.DOShakeScale(0.2f, 0.5f).OnComplete(() => transform.localScale = m_InitialScale);
    }
}
