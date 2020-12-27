using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

/// <summary>
/// 音符对象
/// </summary>
public class NoteObject : MonoBehaviour
{
    //当前关卡对象
    protected Level level;
    //当前音符携带的事件
    protected KoreographyEvent trackedEvent;
    //命中偏移量
    public int hitOffset = 0;
    //偏移量是否达到峰值
    public bool hasMaxOffset = false;
    //音符事件编号
    protected int noteNum;
    //音轨控制者
    protected LaneController laneController;
    //防止二次销毁或者动画播放不完整的开关
    protected bool willDestory;
    //是否已经处理过点击
    public bool valid;
    //音符序号
    public int evtIndex = 0;

    private TextMesh textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponentsInChildren<TextMesh>()[0];

    }
    // Start is called before the first frame update
    void Start()
    {

        transform.SetSiblingIndex(10);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetHitOffset();
        UpdatePosition();
        // 状态切换
        if (IsNoteHittable())
        {
            //GameManager.Instance.Send("ShowTip", "辣鸡!");
            //valid = true;
        }
        // 消失在边界
        else if (transform.position.x <= -10)
        {
            laneController.ClearInvalidNote();
            OnHit(0);
        }
        else
        {
            //valid = false;
        }
        if (hitOffset > 10000)
            hasMaxOffset = true;
    }
    public bool GetNoteValid()
    {
        return valid;
    }
    public void SetNoteValid(bool boCheck)
    {
        this.valid = boCheck;
    }

    public void setEventIndex(int index)
    {
        evtIndex = index;
    }
    /// <summary>
    /// 更新位置
    /// </summary>
    protected virtual void UpdatePosition()
    {
        if (laneController)
        {
            Vector3 pos = laneController.transform.position;
            pos.x -= (level.CurrentSampleTime() - trackedEvent.StartSample) / (float)level.SampleRate() * level.noteSpeed;
            transform.position = pos;
        }
    }
    /// <summary>
    /// 是否命中
    /// </summary>
    /// <param name="hitLevel"></param>
    public virtual void OnHit(int hitLevel)
    {
        if (hitLevel > 0)
        {
            if (noteNum == 1)
            {
                GameManager.Instance.PlaySound(StringManager.shootSound);
            }
            else
            {
                GameManager.Instance.PlaySound(StringManager.jumpSound);
            }
        }
        else
        {
            //未命中处理
            //GameManager.Instance.PlayHurtAnim();
            //GameManager.Instance.PlaySound(StringManager.missSound);
        }
        ReturnToPool();
    }
    /// <summary>
    /// 音符初始化
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="noteNum"></param>
    /// <param name="laneController"></param>
    /// <param name="level"></param>
    public virtual void Initialize(KoreographyEvent evt, int noteNum, LaneController laneController, Level level)
    {
        trackedEvent = evt;
        this.laneController = laneController;
        this.level = level;
        this.noteNum = noteNum;
        SetNoteValid(true);

        switch (noteNum)
        {
            //case 0:
            //    textMesh.text = "O";
            //    break;
            //default:
            //    textMesh.text = "-";
            //    break;
        }
    }
    /// <summary>
    /// 重置音符
    /// </summary>
    public void ResetNote()
    {
        trackedEvent = null;
        laneController = null;
        level = null;
    }
    /// <summary>
    /// 返回对象池
    /// </summary>
    protected virtual void ReturnToPool()
    {
        GameManager.Instance.RecycleObj(StringManager.noteObj, gameObject, ResetNote);
    }
    /// <summary>
    /// 获取样本偏移量
    /// </summary>
    protected void GetHitOffset()
    {
        //获取当前音乐执行到的时间节点
        int curTime = level.CurrentSampleTime();
        //当前此事件开始样本的时间节点
        int noteTime = trackedEvent.StartSample;
        //在音乐样本中的命中窗口
        int hitWindow = level.hitWindowRangeInSample;
        //偏移量 = 命中窗口宽度 - 两个时间的差
        hitOffset = hitWindow - Mathf.Abs(noteTime - curTime);
    }
    /// <summary>
    /// 获取当前音符的命中等级
    /// </summary>
    /// <returns></returns>
    public bool IsNoteHittable()
    {
        GetHitOffset();
        bool hitLevel = false;
        if (hitOffset >= 0)
        {
            if (hasMaxOffset)
            {
                if (hitOffset >= 5000 && hitOffset <= 15000)
                {
                    hitLevel = true;
                }
            }
            else
            {
                if (hitOffset >= 9000)
                {
                    hitLevel = true;
                }
            }
        }
        return hitLevel;
    }
}
