using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using System;

public interface ILevel
{
    void InitLevel();
    void HandleLevelLogic();
    void ExitLevel();
}

public abstract class Level : ILevel
{
    protected GameManager gameManager;
    //玩家得分
    protected int score;
    //目标得分
    protected int goalScore;
    //音符速度
    public int noteSpeed = 3;
    //当前关卡使用的Koreography对象
    protected Koreography playingKoreo;
    //音轨控制列表
    public List<LaneController> noteLanes;
    //每个关卡的背景图片
    public GameObject gameBGObj;
    //当前关卡播放音乐的MusicPlayer
    protected SimpleMusicPlayer musicPlayer;
    //是否执行游戏相关逻辑的开关
    protected bool hasEnter;

    // 音节产生位置
    private GameObject bottomGo;
    private GameObject topGo;
    // 记录是否失败重来
    private bool restart;
    // 当前关卡索引
    public int currentMusicIndex;

    //public 统计是否完成
    public bool taskFinish;

    //当前

    /// <summary>
    /// 在音乐样本中的命中窗口宽度
    /// </summary>
    public int hitWindowRangeInSample
    {
        get
        {
            //音符命中窗口（音符被命中的难度判定,单位：ms）
            float hitWindowRangeInMS = 300;
            return (int)(hitWindowRangeInMS * SampleRate() * 0.001f);
        }
    }

    /// <summary>
    /// 当前音乐样本采样率
    /// </summary>
    /// <returns></returns>
    public int SampleRate()
    {
        return playingKoreo.SampleRate;
    }
    /// <summary>
    /// 当前的采样时间
    /// </summary>
    /// <returns></returns>
    public int CurrentSampleTime()
    {
        return playingKoreo.GetLatestSampleTime();
    }
    /// <summary>
    /// 初始化当前关卡
    /// </summary>
    public virtual void InitLevel()
    {
        //    gameBGObj = gameManager.GetObj(StringManager.gameBG);
        //    gameBGObj.GetComponent<SpriteRenderer>().sprite = gameManager.GetSpriteRes(StringManager.spriteBG+gameManager.GetCurrentLevelIndex().ToString());
        gameManager.ShowPlayer();
        bottomGo = gameManager.GetObj(StringManager.targetTransform);
        topGo = gameManager.GetObj(StringManager.targetTransform);
        bottomGo.transform.position = new Vector3(-10f, 3.5f, 0);
        topGo.transform.position = new Vector3(10f, 3.5f, 0);
        //初始化摄像机
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.AddComponent<CameraMove>();
        mainCamera.transform.SetParent(gameManager.GetPlayerTrans());


        musicPlayer = gameManager.GetMusicPlayer();
        gameManager.Register("EnterLevel", EnterLevel);
        gameManager.Register("ComboFinish", ComboFinish);

        gameManager.Register("TaskFinish", TaskFinish);
        gameManager.Register("PlayerHealth", PlayerHealth);

    }
    /// <summary>
    /// 当前关卡的游戏执行逻辑
    /// </summary>
    public virtual void HandleLevelLogic()
    {
        if (hasEnter)
        {
            //判断当前游戏是否结束
            //musicPlayer.Stop();
            if (!musicPlayer.IsPlaying)
            {

                //如果音乐结束，但是可以重来
                if (taskFinish || restart)
                {
                    gameManager.DoPlayerDamage(1000);
                    currentMusicIndex++;
                    score = 0;
                    EnterLevel(null);
                }
                else
                {

                    EnterLevel(null);
                }

                //if (restart)
                //{
                //    //---------------------------------打boss
                //    // 不加载旁白的冲完
                //    EnterLevel(null);
                //    // gameManager.ReplayGame(); 加载旁白的重玩
                //}
                //else
                //{
                //    //当前游戏结束，音乐停止了
                //    // 一次挑战成功，不会播放提示，失败才会播放
                //    //---------------------------------新手引导
                //    if (score < goalScore)
                //    {
                //        gameManager.ShowMask(CompleteLevel);
                //        hasEnter = false;
                //    }
                //    //---------------------------------过场动画，需要一次
                //    //下一部分音乐
                //    currentMusicIndex++;
                //    score = 0;
                //    EnterLevel(null);
                //}
            }
            //当前得分已满足条件，进入第一关
            //if (!restart && score >= goalScore)
            //{
            //    gameManager.ShowMask(CompleteLevel);
            //    hasEnter = false;
            //}
            //}else (restart && sc)
            //是否生成新音符
            for (int i = 0; i < noteLanes.Count; i++)
            {
                noteLanes[i].CheckSpawnNext();
            }
        }
    }

    public virtual void GoNextLevel()
    {
        //重置所有事件
        Koreographer.Instance.FlushDelayQueue(playingKoreo);
        //重置Koreography对象状态
        playingKoreo.ResetTimings();

        //重置音轨状态
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].ResetLane();
        };
    }

    /// <summary>
    /// 退出当前关卡
    /// </summary>
    public virtual void ExitLevel()
    {
        gameManager.RecycleObj(StringManager.gameBG,gameBGObj);
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].ResetLane();
            noteLanes[i].Recycle();
        }
        gameManager.RecycleObj(StringManager.targetTransform, topGo);
        gameManager.RecycleObj(StringManager.targetTransform, bottomGo);

        noteLanes.Clear();
        score = 0;
        gameManager.UnRegister("EnterLevel", EnterLevel);
        gameManager.Send("ResetCamera");
        gameManager.HideControlPanel();
    }
    /// <summary>
    /// 进入关卡，加载当前关卡的所有相关数据（做准备工作）
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void EnterLevel(object obj)
    {
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].ResetLane();
            noteLanes[i].Recycle();
        }
        noteLanes.Clear();
        for (int i = 0; i < 1; i++)
        {
            noteLanes.Add(gameManager.GetObj(StringManager.laneController).AddComponent<LaneController>());
            noteLanes[i].Initialize(this, i, topGo.transform, bottomGo.transform);
            noteLanes[i].transform.position = new Vector3(0, 3.5f, 0);
        }
        taskFinish = false;

        gameManager.ShowControlPanel();
        GoNextLevel();
    }
    /// <summary>
    /// 进入当前关卡,播放音乐开始游戏（正式开始当前关卡，进入游戏）
    /// music 播放的背景音乐，需要菜点的音乐
    /// EventID 展示音符的事件
    /// </summary>
    protected void EnterCurrentLevel(bool restart, string music, string EventID)
    {
        //设置这首音乐是否可以循环
        this.restart = restart;

        gameManager.PlayMusic(music, false);
        //加载Koreography对象到musicPlayer
        musicPlayer.LoadSong(gameManager.GetKoreoRes(music), 0, false);
        
        //获取加载到musicPlayer身上Koreographer单例里的Koreography对象
        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);
        //获取事件轨迹
        KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(EventID);
        //获取轨迹上的所有事件
        List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();
        //把对应事件匹配到音轨上
        for (int i = 0; i < rawEvents.Count; i++)
        {
            KoreographyEvent evt = rawEvents[i];
            MatchEvt(evt);
        }
        hasEnter = true;
    }
    /// <summary>
    /// 事件音轨匹配方法
    /// </summary>
    /// <param name="evt"></param>
    public virtual void MatchEvt(KoreographyEvent evt)
    {
        int noteID = evt.GetIntValue();
        for (int i = 0; i < noteLanes.Count; i++)
        {
            LaneController lane = noteLanes[i];
            if (lane.DoesMatch(noteID))
            {
                lane.AddEventToLane(evt);
                break;
            }
        }
    }
    /// <summary>
    /// 完成当前关卡
    /// </summary>
    protected void CompleteLevel()
    {
        gameManager.HideMask();
        gameManager.Send("ShowPanel",new TalkIndexMsg { ifAdd=true,addCount=1});
        gameManager.Send("ShowCurrentLevelInformation",Summarize());
    }
    /// <summary>
    /// 计算玩家在当前关卡得分的评定等级
    /// </summary>
    /// <returns></returns>
    protected int Summarize()
    {
        int scoreLevel = 0;
        if (score>=goalScore)
        {
            scoreLevel = 1;
            if (score>=goalScore*1.5f)
            {
                scoreLevel = 2;
            }
        }
        //return scoreLevel;
        return score;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="goalScore"></param>
    public Level(int goalScore)
    {
        gameManager = GameManager.Instance;
        noteLanes = new List<LaneController>();
        this.goalScore = goalScore;

    }
    /// <summary>
    /// 增加分数
    /// </summary>
    /// <param name="getScore"></param>
    private void UpdateScore(int getScore)
    {
        score += getScore;
    }
    /// <summary>
    /// 玩家技能释放成功
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void ComboFinish(object boSuccessObj)
    {
        if( Convert.ToBoolean(boSuccessObj))
            UpdateScore(1);
    }

    protected virtual void TaskFinish(object Obj)
    {
        taskFinish = Convert.ToBoolean(Obj);
    }

    protected void UpdateMethod(KoreographyEvent koreographyEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreographyEvent.HasTextPayload())
        {
            string methonName = koreographyEvent.GetTextValue();
            switch (methonName)
            {
                case "Monster_3":
                    gameManager.CreateMonster(methonName, 100);
                    break;
                case "Monster_2":
                    gameManager.CreateMonster(methonName, 50);
                    break;
                case "pickUpWeapon":
                    gameManager.Send("ShowControlPanelExtra", methonName);
                    break;
                case "Monster_zhangaiwu":
                    gameManager.CreateMonster(methonName, 20);
                    break;
                default:
                    gameManager.Send("ShowGuide", methonName);
                    break;
            }
        }
    }
    private void PlayerHealth(object HealthObg)
    {

        int health = Convert.ToInt32(HealthObg);
        if (health == 0)
        {
            gameManager.ShowMask(CompleteLevel);
            hasEnter = false;
        }
    }
}