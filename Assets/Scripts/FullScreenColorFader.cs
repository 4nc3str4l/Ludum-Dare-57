using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenColorFader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Image _image;    
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponent<Image>();
    }

    public void FadeIn(Color _color, float _duration)
    {
        _image.color = _color;
        _canvasGroup.DOFade(1, _duration).OnComplete(() => _canvasGroup.alpha = 1);
    }

    public void FadeOut(float _duration)
    {
        _canvasGroup.DOFade(0, _duration).OnComplete(() => _canvasGroup.alpha = 0);
    }
}
