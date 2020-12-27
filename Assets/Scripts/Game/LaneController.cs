using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
/// <summary>
/// 音轨控制者,负责检测，生成音符，事件匹配等等
/// </summary>
public class LaneController : MonoBehaviour
{
    //当前关卡管理的引用
    protected Level level;
    //此音轨对应的音符ID
    protected int laneID;
    //移动时间，提前生成音符的时间
    public float moveTime;
    //上下边界
    public Transform targetTopTrans;
    public Transform targetBottomTrans;
    //包含此音轨中的所有事件的列表
    protected List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();
    //包含此音轨当前活动的所有音符对象的队列
    protected Queue<NoteObject> trackedNotes = new Queue<NoteObject>();
    //检测此音轨中的生成下一个事件的索引
    protected int pendingEventIdx = 0;
    // 管理类
    private GameManager gameManager;
    private bool canReleaseSkill = true;
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    /// <summary>
    /// 音轨初始化
    /// </summary>
    public virtual void Initialize(Level level,int laneID,Transform topTrans,Transform bottomTrans)
    {
        this.level = level;
        this.laneID = laneID;
        targetTopTrans = topTrans;
        targetBottomTrans = bottomTrans;
        moveTime = (targetTopTrans.position.x - transform.position.x) / level.noteSpeed;
        GameManager.Instance.Register("CheckNoteHit", CheckNoteHit);
    }
    /// <summary>
    /// 检测是否生成下一个新音符
    /// </summary>
    public virtual void CheckSpawnNext()
    {
        //音符在音谱上产生的位置偏移量
        int samplesToTarget = (int)(level.SampleRate() * moveTime);
        //当前的采样时间
        int currentTime = level.CurrentSampleTime();
        while (pendingEventIdx<laneEvents.Count&&
            laneEvents[pendingEventIdx].StartSample<currentTime+samplesToTarget)
        {
            KoreographyEvent evt = laneEvents[pendingEventIdx];
            int noteNum = evt.GetIntValue();
            NoteObject newObj = GameManager.Instance.GetObj(StringManager.noteObj).GetComponent<NoteObject>();
            newObj.Initialize(evt,noteNum,this,level);
            newObj.setEventIndex(pendingEventIdx);
            trackedNotes.Enqueue(newObj);
            pendingEventIdx++;
        }
    }
    /// <summary>
    /// 检测是否击中音符对象
    /// </summary>
    /// <param name="mouseIDObj"></param>
    public virtual void CheckNoteHit(object Obj)
    {
        foreach (NoteObject noteObject in trackedNotes)
        {
            //NoteObject noteObject = trackedNotes.Peek();
            if (noteObject.hitOffset > -6000)
            {
                //trackedNotes.Dequeue();
                if (noteObject.IsNoteHittable() && noteObject.GetNoteValid())
                {

                    if (canReleaseSkill)
                    {
                        noteObject.SetNoteValid(false);
                        gameManager.SetPlayerSkillType((PlayerSkillType)laneID);
                        canReleaseSkill = false;
                        gameManager.SetPlayerCanInput(true);
                        //Debug.Log("击中:" + noteObject.evtIndex);
                    }

                    //命中
                    //GameManager.Instance.Send("ShowTip","流批!");
                }
                else
                {
                    if (!canReleaseSkill)
                        canReleaseSkill = true;
                }
                if (!noteObject.IsNoteHittable())
                { 
                    gameManager.SetPlayerCanInput(false);
                //未命中
                //GameManager.Instance.Send("ShowTip","辣鸡!");
                }
                //noteObject.OnHit(hitLevel);
            }
        }
        if (trackedNotes.Count>0)
        {

            
        }
    }

    /// <summary>
    /// 检测事件负荷上音符ID是否与当前音轨ID匹配
    /// </summary>
    /// <param name="noteID"></param>
    /// <returns></returns>
    public bool DoesMatch(int noteID)
    {
        return noteID == laneID;
    }
    /// <summary>
    /// 如果匹配，则把当前事件添加进音轨所持有的事件列表
    /// </summary>
    /// <param name="evt"></param>
    public void AddEventToLane(KoreographyEvent evt)
    {
        laneEvents.Add(evt);
    }
    /// <summary>
    /// 清除无效音符的方法
    /// </summary>
    public void ClearInvalidNote()
    {
        trackedNotes.Dequeue();
    }
    /// <summary>
    /// 重置音轨状态
    /// </summary>
    public void ResetLane()
    {
        pendingEventIdx = 0;
        for (int i = 0; i < trackedNotes.Count; i++)
        {
            trackedNotes.Dequeue().OnHit(0);
        }
    }
    /// <summary>
    /// 回收音轨控制者
    /// </summary>
    public void Recycle()
    {
        GameManager.Instance.UnRegister("CheckNoteHit", CheckNoteHit);
        GameManager.Instance.RecycleObj(StringManager.laneController,gameObject);
        Destroy(this);
    }
}
