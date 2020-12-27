using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class LevelOne : Level
{
    private GameObject predatorObj;
    //完成训练模式
    private bool finishExecise;
    //是否已经进入第一关（第一关的相关启动方法是否执行）
    private bool enterLevelOne;
    //移动路径
    public Vector3[] movePath;
    //销毁路径
    public Vector3[] destoryPath;

    public LevelOne(int goalScore) : base(goalScore)
    {

    }

    public override void InitLevel()
    {
        base.InitLevel();
        //实例化出起司尼首长
        predatorObj = gameManager.GetObj(StringManager.predator);
        //实例化音轨控制者并添加相应脚本
        noteLanes.Add(gameManager.GetObj(StringManager.laneController).AddComponent<LaneController>());
        noteLanes.Add(gameManager.GetObj(StringManager.laneController).AddComponent<LaneController>());
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].Initialize(this, i + 1, predatorObj.transform, gameManager.GetPlayerTrans());
            noteLanes[i].transform.position = gameManager.GetPlayerTrans().position;
        }
        gameManager.Register("EnterExeciseMode", EnterExeciseMode);
        gameManager.PlayMusic(0.ToString(), false);
        GetPath();
        finishExecise = false;
    }
    /// <summary>
    /// 获取雪人靶子移动的路径
    /// </summary>
    private void GetPath()
    {
        //移动路径获取
        Transform[] noteObjectPathTrans;
        GameObject targetSnowPathGo = gameManager.GetObj(StringManager.targetSnowPath);
        noteObjectPathTrans = targetSnowPathGo.GetComponentsInChildren<Transform>();
        movePath = new Vector3[noteObjectPathTrans.Length - 1];
        for (int i = 0; i < movePath.Length; i++)
        {
            movePath[i] = noteObjectPathTrans[i + 1].position;
        }
        gameManager.RecycleObj(StringManager.targetSnowPath, targetSnowPathGo);
        //销毁路径获取
        Transform[] destoryPathTrans;
        GameObject destoryPathGo = gameManager.GetObj(StringManager.targetSnowDestoryPath);
        destoryPathTrans = destoryPathGo.GetComponentsInChildren<Transform>();
        destoryPath = new Vector3[destoryPathTrans.Length - 1];
        for (int i = 0; i < destoryPath.Length; i++)
        {
            destoryPath[i] = destoryPathTrans[i + 1].position;
        }
        gameManager.RecycleObj(StringManager.targetSnowDestoryPath, destoryPathGo);
    }

    public override void HandleLevelLogic()
    {
        if (finishExecise)//第一关卡的逻辑
        {
            if (!enterLevelOne)
            {
                Koreographer.Instance.UnregisterForEvents(StringManager.commandEventID, UpdateCommandText);
                gameManager.Send("ShowPanel");
                enterLevelOne = true;
            }
            else
            {
                if (hasEnter)
                {
                    //musicPlayer.Stop();
                    if (!musicPlayer.IsPlaying)
                    {
                        gameManager.ShowMask(CompleteLevel);
                        hasEnter = false;
                    }
                    //检测是否生成新音符
                    for (int i = 0; i < noteLanes.Count; i++)
                    {
                        noteLanes[i].CheckSpawnNext();
                    }
                }
            }
        }
        else//训练模式的逻辑
        {
            if (hasEnter)
            {
                //音乐停止则重新播放音乐
                if (!musicPlayer.IsPlaying)
                {
                    ReStart();
                }
                //当前得分已满足条件，进入第一关
                if (score >= goalScore)
                {
                    finishExecise = true;
                    hasEnter = false;
                }
                //检测是否生成新音符
                for (int i = 0; i < noteLanes.Count; i++)
                {
                    noteLanes[i].CheckSpawnNext();
                }
            }
        }
    }

    public override void ExitLevel()
    {
        base.ExitLevel();
        gameManager.UnRegister("EnterExeciseMode", EnterExeciseMode);
        gameManager.RecycleObj(StringManager.predator, predatorObj, () => { predatorObj.GetComponent<Predator>().ResetState(); });
    }

    #region 训练模式
    /// <summary>
    /// 进入训练模式
    /// </summary>
    /// <param name="obj"></param>
    public void EnterExeciseMode(object obj = null)
    {
        Koreographer.Instance.RegisterForEventsWithTime(StringManager.commandEventID, UpdateCommandText);
        hasEnter = true;
    }
    /// <summary>
    /// 更新起司尼命令文本
    /// </summary>
    /// <param name="koreographyEvent"></param>
    /// <param name="sampleTime"></param>
    /// <param name="sampleDelta"></param>
    /// <param name="deltaSlice"></param>
    private void UpdateCommandText(KoreographyEvent koreographyEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreographyEvent.HasTextPayload())
        {
            gameManager.Send("ShowTip", koreographyEvent.GetTextValue());
        }
    }
    /// <summary>
    /// 重玩训练模式
    /// </summary>
    private void ReStart()
    {
        //重置所有事件
        Koreographer.Instance.FlushDelayQueue(playingKoreo);
        //重置Koreography对象状态
        playingKoreo.ResetTimings();
        //重置音轨状态
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].ResetLane();
        }
        musicPlayer.LoadSong(gameManager.GetKoreoRes(0.ToString()), 0, false);
        gameManager.PlayMusic(0.ToString(), false);
    }

    #endregion

    #region 第一关
    protected override void EnterLevel(object obj)
    {
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].ResetLane();
            noteLanes[i].Recycle();
        }
        noteLanes.Clear();
        score = 0;
        goalScore = 44;
        //goalScore = 0;
        noteLanes.Add(gameManager.GetObj(StringManager.laneController).AddComponent<LaneController>());
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].Initialize(this, 1, predatorObj.transform, gameManager.GetPlayerTrans());
            noteLanes[i].transform.position = gameManager.GetPlayerTrans().position + new Vector3(1.04f, 0, 0);
        }
    }

    #endregion
}