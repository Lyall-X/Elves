using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedNoteObject : NoteObject
{
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

    public override void OnHit(int hitLevel)
    {
        GameManager.Instance.Send("DesorySeed");
        if (hitLevel>0)
        {
            ReturnToPool();
            GameManager.Instance.PlaySound(StringManager.seedSound);
        }
        else
        {
            willDestory = true;
            GameManager.Instance.PlayHurtAnim();
            GameManager.Instance.PlaySound(StringManager.missSound);
            Invoke("ReturnToPool",3);
        }
    }

    protected override void ReturnToPool()
    {
        willDestory = false;
        GameManager.Instance.RecycleObj(StringManager.seedNoteObject,gameObject,ResetNote);
    }
}
