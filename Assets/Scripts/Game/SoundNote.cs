using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundNote : NoteObject
{
    protected override void ReturnToPool()
    {
        willDestory = false;
        GameManager.Instance.RecycleObj(StringManager.soundObj,gameObject,ResetNote);
    }

    protected override void Update()
    {
        GetHitOffset();
        UpdatePosition();
        if (transform.position.x <= laneController.targetBottomTrans.position.x&&!willDestory)
        {
            laneController.ClearInvalidNote();
            OnHit(0);
        }
    }

    public override void OnHit(int hitLevel)
    {
        if (hitLevel>0)
        {
            if (noteNum==1)
            {
                GameManager.Instance.PlaySound(StringManager.clapSound);
            }
            else
            {
                GameManager.Instance.PlaySound(StringManager.stampSound);
            }
        }
        else
        {
            GameManager.Instance.PlaySound(StringManager.missSound);
            GameManager.Instance.PlayHurtAnim();
        }
        willDestory = true;
        ReturnToPool();
    }
}
