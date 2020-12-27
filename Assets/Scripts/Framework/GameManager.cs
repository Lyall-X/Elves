using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using System;
using SonicBloom.Koreo.Players;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private ResourceManager resourceManager;
    private EventManager eventManager;
    private UIManager uiManager;
    private PlayerManger playerManager;
    private MonsterManager monsterManager;
    private RhythmGameManager rhythmGameManager;
    private AudioSouceManager audioSouceManager;
    private ControlManager controlManager;

    private void Awake()
    {
        Instance = this;
        InitManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        InputMonitor();
        HandleGameLogic();
    }

    private void InitManager()
    {
        resourceManager = new ResourceManager();
        eventManager = new EventManager();
        uiManager = new UIManager();
        playerManager = new PlayerManger();
        monsterManager = new MonsterManager();
        rhythmGameManager = new RhythmGameManager();
        audioSouceManager = new AudioSouceManager();
        controlManager = new ControlManager();
    }

    #region 资源管理
    public Sprite GetSpriteRes(string resName)
    {
        return resourceManager.GetSpriteRes(resName);
    }
    public AudioClip GetAudioClipRes(string resName)
    {
        return resourceManager.GetAudioclipRes(resName);
    }
    public GameObject GetPrefabRes(string resName)
    {
        return resourceManager.GetPrefabRes(resName);
    }
    public Koreography GetKoreoRes(string resName)
    {
        return resourceManager.GetKoreoRes(resName);
    }
    public GameObject GetObj(string objName,int initCount=0)
    {
        return resourceManager.GetObj(objName,initCount);
    }
    public void RecycleObj(string objName,GameObject obj,Action resetMethod=null)
    {
        resourceManager.RecycleObj(objName,obj,resetMethod);
    }
    #endregion

    #region 事件管理
    /// <summary>
    /// 事件注册
    /// </summary>
    /// <param name="msgName"></param>
    /// <param name="onMsgReceived"></param>
    public void Register(string msgName,Action<object> onMsgReceived)
    {
        eventManager.Register(msgName, onMsgReceived);
    }
    /// <summary>
    /// 事件注销
    /// </summary>
    /// <param name="msgName"></param>
    /// <param name="onMsgReceived"></param>
    public void UnRegister(string msgName, Action<object> onMsgReceived)
    {
        eventManager.UnRegister(msgName, onMsgReceived);
    }
    /// <summary>
    /// 事件发送
    /// </summary>
    /// <param name="msgName"></param>
    /// <param name="data"></param>
    public void Send(string msgName,object data=null)
    {
        eventManager.Send(msgName,data);
    }
    #endregion

    #region UI管理
    public GameObject LoadUI(string uiName)
    {
        return uiManager.LoadUI(uiName);
    }
    public void LoadPanel(string panelName)
    {
        uiManager.LoadPanel(panelName);
    }
    public void RecyclePanel(string panelName)
    {
        uiManager.RecyclePanel(panelName);
    }
    public void ShowMask(TweenCallback tweenCallback)
    {
        uiManager.ShowMask(tweenCallback);
    }
    public void HideMask()
    {
        uiManager.HideMask();
    }

    public void ShowControlPanel()
    {
        controlManager.ShowControlPanel();
    }

    public void HideControlPanel()
    {
        controlManager.HideControlPanel();
    }
    public void RecycleControlPanel()
    {
        controlManager.RecycleControlPanel();
    }

    public void DownButton(int btn_index)
    {
        controlManager.DownButton(btn_index);
    }

    public void UpButton(int btn_index)
    {
        controlManager.UpButton(btn_index);
    }

    public bool CheckButtonPressed(int btn_index)
    {
        return controlManager.CheckButtonPressed(btn_index);
    }
    public void SetControlBtnVisible(string BtnName, bool visible)
    {
        controlManager.SetControlBtnVisible(BtnName, visible);
    }
    #endregion

    #region 音频管理
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySound(string soundName)
    {
        audioSouceManager.PlaySound(soundName);
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="musicName"></param>
    /// <param name="loop"></param>
    public void PlayMusic(string musicName,bool loop)
    {
        audioSouceManager.PlayMusic(musicName,loop);
    }
    /// <summary>
    /// 停止音乐
    /// </summary>
    public void StopMusic()
    {
        audioSouceManager.StopMusic();
    }
    /// <summary>
    /// 获取MusicPlayer
    /// </summary>
    /// <returns></returns>
    public SimpleMusicPlayer GetMusicPlayer()
    {
        return audioSouceManager.GetMusicPlayer();
    }
    /// <summary>
    /// 获取当前音乐播放到样本点位置
    /// </summary>
    /// <returns></returns>
    public int GetMusicSample()
    {
        return audioSouceManager.GetMusicSample();
    }

    #endregion

    #region 玩家管理
    /// <summary>
    /// 显示玩家游戏物体
    /// </summary>
    public void ShowPlayer()
    {
        playerManager.ShowPlayer();
    }
    /// <summary>
    /// 获取到玩家的组件引用
    /// </summary>
    /// <returns></returns>
    public Transform GetPlayerTrans()
    {
        return playerManager.GetPlayerTrans();
    }
    /// <summary>
    /// 设置玩家技能类型
    /// </summary>
    public void SetPlayerSkillType(PlayerSkillType playerSkillType)
    {
        playerManager.SetPlayerSkillType(playerSkillType);
    }

    /// <summary>
    /// 设置玩家是否可以输入
    /// </summary>
    public void SetPlayerCanInput(bool canInput)
    {
        playerManager.SetPlayerCanInput(canInput);
    }

    /// <summary>
    /// 设置玩家伤害
    /// </summary>
    /// <param name="hurt"></param>
    public void DoPlayerDamage(float hurt)
    {
        playerManager.DoPlayerDamage(hurt);
    }
    /// <summary>
    /// 输入监听
    /// </summary>
    private void InputMonitor()
    {
        playerManager.InputMonitor();
    }
    /// <summary>
    /// 播放受伤害动画
    /// </summary>
    public void PlayHurtAnim()
    {
        playerManager.PlayHurtAnim();
    }
    /// <summary>
    /// 重置玩家状态
    /// </summary>
    public void InitPlayer()
    {
        playerManager.InitPlayer();
    }

    public void RecyclePlayer()
    {
        playerManager.RecyclePlayer();
    }
    public List<PlayerAttackType> GetPlayerCombo()
    {
        return playerManager.GetPlayerCombo();
    }

    public void SetWeaponPlayer()
    {
        playerManager.SetWeaponPlayer();
    }

    public Vector3 GetPlayerPos(bool canvens)
    {
        if (canvens)
            return new Vector3(-528, -197, 0);
        else
            return new Vector3(-5, -2, 0);
    }
    public void SetPlayerCanRun(bool canrun)
    {
        playerManager.SetPlayerCanRun(canrun);
    }
    public void PlayerStopRun()
    {
        playerManager.StopRun();
    }
    public int GetComboTimes()
    {
        return playerManager.GetCombo();
    }
    #endregion


    #region 怪物管理
    /// <summary>
    /// 显示怪物游戏物体
    /// </summary>
    public void CreateMonster(string monsterName, int heal)
    {
        monsterManager.CreateMonster(monsterName, heal);
    }
    /// <summary>
    /// 显示怪物游戏物体
    /// </summary>
    public void ShowMonster()
    {
        monsterManager.ShowMonster();
    }
    /// <summary>
    /// 获取到怪物的组件引用
    /// </summary>
    /// <returns></returns>
    public Transform GetMonsterTrans()
    {
        return monsterManager.GetMonsterTrans();
    }
    /// <summary>
    /// 设置怪物伤害
    /// </summary>
    /// <param name="hurt"></param>
    public void DoMonsterDamage(float hurt)
    {
        monsterManager.DoMonsterDamage(hurt);
    }
    /// <summary>
    /// 怪物开始奔跑
    /// </summary>
    public void MonsterRun()
    {
        monsterManager.MonsterRun();
    }
    /// <summary>
    /// 播放怪物受伤害动画
    /// </summary>
    public void MonsterHurtAnim()
    {
        monsterManager.PlayHurtAnim();
    }
    /// <summary>
    /// 重置怪物状态
    /// </summary>
    public void InitMonster()
    {
        monsterManager.InitMonster();
    }

    public void RecycleMonster()
    {
        monsterManager.RecycleMonster();
    }
    public void PlayAttackAnim()
    {
        monsterManager.PlayAttackAnim();
    }
    public Vector3 GetMonsterPos()
    {
        return monsterManager.GetMonsterPos();
    }
    #endregion

    #region 游戏启动
    public void InitGame()
    {
        //为了视频提前加载
        //LoadPanel(StringManager.panelStart);
        PlayMusic(StringManager.mainBG,true);
    }

    public void EnterGame()
    {
        // 临时
        rhythmGameManager.currentLevelIndex = 5;
        RecyclePanel(StringManager.panelStart); 
        HideMask();
        LoadPanel(StringManager.panelTalk);
        LoadPanel(StringManager.panelControl);
        rhythmGameManager.StartGame();
    }

    #endregion

    #region 游戏管理
    /// <summary>
    /// 获取当前关卡索引
    /// </summary>
    /// <returns></returns>
    public int GetCurrentLevelIndex()
    {
        return rhythmGameManager.currentLevelIndex;
    }
    /// <summary>
    /// 获取当前关卡对象
    /// </summary>
    /// <returns></returns>
    public ILevel GetCurrentLevel()
    {
        return rhythmGameManager.currentLevel;
    }
    /// <summary>
    /// 执行游戏相关逻辑
    /// </summary>
    public void HandleGameLogic()
    {
        rhythmGameManager.HandleGameLogic();
    }
    /// <summary>
    /// 退出当前关卡
    /// </summary>
    public void ExitCurrentLevel()
    {
        rhythmGameManager.ExitCurrentLevel();
    }
    /// <summary>
    /// 重玩当前关卡
    /// </summary>
    public void ReplayGame()
    {
        rhythmGameManager.ReplayGame();
    }
    #endregion
}
