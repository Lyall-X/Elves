using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 雪人靶子
/// </summary>
public class TargetSnowman : NoteObject
{
    private Tween destoryTween;
    
    public override void Initialize(KoreographyEvent evt, int noteNum, LaneController laneController, Level level)
    {
        base.Initialize(evt, noteNum, laneController, level);
        LevelOne levelOne = level as LevelOne;
        transform.position = laneController.targetTopTrans.position;
        transform.DOPath(levelOne.movePath,laneController.moveTime,PathType.CatmullRom).SetEase(Ease.InQuart);
        destoryTween = transform.DOPath(levelOne.destoryPath, laneController.moveTime/4, PathType.CatmullRom).SetEase(Ease.OutQuart);
        destoryTween.SetAutoKill(false);
        destoryTween.Pause();
        Invoke("PlayThrowSound",2);
    }

    private void PlayThrowSound()
    {
        GameManager.Instance.PlaySound(StringManager.throwSound);
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetHitOffset();
        if (transform.position.x-laneController.targetBottomTrans.position.x<=0.7f&&!willDestory)
        {
            GameManager.Instance.PlayHurtAnim();
            laneController.ClearInvalidNote();
            OnHit(0);
        }
    }

    public override void OnHit(int hitLevel)
    {
        if (hitLevel>0)
        {
            destoryTween.Play();
            willDestory = true;
            Invoke("ReturnToPool",laneController.moveTime/4);
            GameManager.Instance.PlaySound(StringManager.hitSound);
        }
        else
        {
            GameManager.Instance.PlayHurtAnim();
            ReturnToPool();
            GameManager.Instance.PlaySound(StringManager.missSound);
        }    
    }

    protected override void ReturnToPool()
    {
        willDestory = false;
        GameManager.Instance.RecycleObj(StringManager.targetSnowman,gameObject,ResetNote);
    }
}
