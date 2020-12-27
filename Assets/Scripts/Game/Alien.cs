using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SonicBloom.Koreo;

public class Alien : MonoBehaviour
{
    private Animator animator;
    private Tween jumpTween;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        InitJumpAnimation();
        Koreographer.Instance.RegisterForEvents("Attack", PlayAnim);
    }
    private void PlayAnim(KoreographyEvent koreographyEvent)
    {
        if (koreographyEvent.HasIntPayload())
        {
            int animNum = koreographyEvent.GetIntValue();
            if (animNum==1)
            {
                animator.SetTrigger("attack");
            }
            else
            {
                animator.SetTrigger("jump");
                JumpUp();
            }
        }
    }

    private void InitJumpAnimation()
    {
        jumpTween = transform.DOLocalMoveY(transform.position.y+2.9f,0.083f);
        jumpTween.SetEase(Ease.OutCubic);
        jumpTween.SetAutoKill(false);
        jumpTween.Pause();
        jumpTween.OnComplete(JumpDown);
    }

    private void JumpUp()
    {
        jumpTween.PlayForward();
    }

    private void JumpDown()
    {
        jumpTween.PlayBackwards();
    }
}
