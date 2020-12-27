using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SonicBloom.Koreo;

/// <summary>
/// 敌人雪人
/// </summary>
public class EnemySnowman : NoteObject
{
    public bool ifWhite;

    private Animator animator;

    private Tween jumpTween;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetHitOffset();
        UpdatePosition();
        if (transform.position.x<=laneController.targetBottomTrans.position.x&&!willDestory)
        {
            laneController.ClearInvalidNote();
            OnHit(0);
        }
    }

    protected override void UpdatePosition()
    {
        if (willDestory)
        {
            return;
        }
        Vector3 pos = laneController.transform.position;
        pos.x -= (level.CurrentSampleTime() - trackedEvent.StartSample) / (float)level.SampleRate() * level.noteSpeed;
        transform.position = new Vector3(pos.x,transform.position.y,transform.position.z);
    }

    public override void Initialize(KoreographyEvent evt, int noteNum, LaneController laneController, Level level)
    {     
        base.Initialize(evt, noteNum, laneController, level);
        InitJumpAnimation();
        InvokeRepeating("JumpUp",0,0.5f);
    }

    private void InitJumpAnimation()
    {
        jumpTween = transform.DOLocalMoveY(0,0.25f);
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

    private void Attack()
    {
        animator.SetTrigger("attack");
    }

    private void Die()
    {
        DOTween.To(
            () => transform.position, r
               => transform.position = r,
            new Vector3(transform.position.x,-3.79f,transform.position.z),
            0.583f
            );
        animator.SetTrigger("die");
    }

    protected override void ReturnToPool()
    {
        CancelInvoke();
        willDestory = false;
        if (ifWhite)
        {
            GameManager.Instance.RecycleObj(StringManager.enemyWhite,gameObject,ResetNote);
        }
        else
        {
            GameManager.Instance.RecycleObj(StringManager.enemyBlack,gameObject,ResetNote);
        }
    }

    public override void OnHit(int hitLevel)
    {
        willDestory = true;
        jumpTween.Pause();
        jumpTween.Kill();
        if (hitLevel>0)
        {
            Die();
            Invoke("ReturnToPool",0.583f);
            GameManager.Instance.PlaySound(StringManager.swordSound);
        }
        else
        {
            GameManager.Instance.PlayHurtAnim();
            Attack();
            Invoke("ReturnToPool",1.083f);
            GameManager.Instance.PlaySound(StringManager.jumpSound);
        }
    }
}
