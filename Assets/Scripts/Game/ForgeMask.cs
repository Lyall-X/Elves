using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 雾效
/// </summary>
public class ForgeMask : MonoBehaviour
{
    private Image imgForge;
    private Image imgMask;
    private Tween maskTween;
    private Tween forgeTween;

    // Start is called before the first frame update
    void Start()
    {
        imgForge = transform.Find("Forge").GetComponent<Image>();
        imgMask = transform.Find("Mask").GetComponent<Image>();
        maskTween = DOTween.To
            (() => imgMask.color, a => imgMask.color = a,
            new Color(imgMask.color.r, imgMask.color.g, imgMask.color.b,1),
            2
            );
        maskTween.SetAutoKill(false);
        maskTween.Pause();
        forgeTween = DOTween.To
            (() => imgForge.color, a => imgForge.color = a,
            new Color(imgForge.color.r, imgForge.color.g, imgForge.color.b, 1),
            2
            );
        forgeTween.SetAutoKill(false);
        forgeTween.Pause();
    }

    public void ShowForgeMask()
    {
        maskTween.PlayForward();
    }

    public void ShowForge()
    {
        forgeTween.PlayForward();
    }

    public void HideForgeAndMask()
    {
        gameObject.SetActive(false);
        maskTween.PlayBackwards();
        forgeTween.PlayBackwards();
    }
}
