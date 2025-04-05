using DG.Tweening;
using UnityEngine;

public class GrapplingGunEffects : MonoBehaviour
{
    private GrapplingGun m_GrapplingGun;
    private Vector3 m_OriginalScale;
    private Quaternion m_OriginalRotation;

    public ShotgunShell ShotgunShellPrefab;

    private void Awake()
    {
        m_OriginalScale = transform.localScale;
        m_OriginalRotation = transform.localRotation;
        m_GrapplingGun = GetComponent<GrapplingGun>();
    }

    private void OnEnable()
    {
        m_GrapplingGun.OnShoot += GrapplingGunOnOnShoot;
        GrapplingGun.OnGrappling += GrapplingGunOnOnGrappling;
    }
    
    private void OnDisable()
    {
        m_GrapplingGun.OnShoot -= GrapplingGunOnOnShoot;
        GrapplingGun.OnGrappling -= GrapplingGunOnOnGrappling;
    }

    private void GrapplingGunOnOnShoot()
    {
        JukeBox.Instance.PlaySound(JukeBox.Instance.Shoot, 0.5f);
        ShotgunShell shotgunShell = Instantiate(ShotgunShellPrefab);
        shotgunShell.transform.parent = ShotgunShellPrefab.transform.parent;
        shotgunShell.transform.localPosition = ShotgunShellPrefab.transform.localPosition;
        shotgunShell.Init();
        
        transform.DOShakeScale(0.1f, 0.1f).OnComplete(() =>
        {
            transform.localScale = m_OriginalScale;
        });
    }
    
    private void GrapplingGunOnOnGrappling(bool isgrappling)
    {
        if (isgrappling)
        {
            JukeBox.Instance.EnableAudioSource(JukeBox.Instance.Hanging, 0.4f);
        }
        else
        {
            JukeBox.Instance.DisableAudioSource(JukeBox.Instance.Hanging);
        }
    }
}
