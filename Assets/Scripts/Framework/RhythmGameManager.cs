using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 推动游戏进程，管理游戏相关关卡数据以及关卡对象
/// </summary>
class RhythmGameManager
{
    private GameManager gameManager;

    public RhythmGameManager()
    {
        gameManager = GameManager.Instance;
        currentLevelIndex = 1;
        InitStringList();
    }

    #region 游戏剧情
    public List<string[]> stringList;
    private string[] levelOne =
    {
        "长官：\n我等你很久了，孩子，你已经251354965岁了，是时候为国家效力了。",
        "我：\n好的，首长！",
        "长官：\n接下来由我亲自训练你。",
        "长官：\n按下左键发射激光，按下右键跳起，要根据音乐的节奏。为了让你快速适应，我会提前喊口令，你按照我的口令执行命令。",
        "我：\n好的，首长！",
        "长官：\n你已经掌握了基本技能了，接下来进入射击实战吧，我会向你丢靶子，你来消灭他们，按下左键进行射击。",
        "孩子你真棒！可以独挡一面了。",
        "还是不行，再练练吧。"
    };
    private string[] levelTwo =
    {
        "队长：\n侦察兵发现，异军已将他们的实验体播种进我们家园的大地",
        "队长：\n长出来的怪物雪人克隆体如果不马上清除，马上就会有攻击意识，杀害我们的同胞，上边马上派你去处理一下。",
        "我：\n好的，队长！",
        "同志，你真强，多亏了你的帮助！",
        "连基本任务都做不好。"
    };
    private string[] levelThree =
    {
        "吊炸天:\n这里进行剧情测试！",
        "我：\nA,S,D键控制！",
        "在大家一起努力的帮助下，我们成功歼灭了异军，保卫了家园！",
        "我们的家园被起司尼掠略掠长官和它的异军占领了"
    };
    private string[] levelFour =
    {
        "大家一起保卫了我们的家园，准备庆祝一起跳舞，每个人都要参加这快乐的聚会。",
        "外星人们快乐幸福的生活了下去！",
        "大家：\n你真的很不擅长跳舞呢！",
    };
    private string[] levelFive =
    {
        "成功啦！成功啦！成功啦！\n回到过去。\n还是熟悉的古森林，宁静又美好。",
        "拯救！部落！古兽！\n古兽们被邪恶生物附体控制。\n阻止它们毁坏家园！",
        "古兽:\n  破坏！破坏！破坏！\n  暴躁的要毁坏一切！",
        "火！木！水！\n火带来能量！ 木带来新生！水带来活力！\n寻找遗失的元素！",
        "胜利的曙光就在前方",
        "怎么就失败了呢？"
    };
    private string[] levelSix =
    {
        "前面是古兽的味道！前进！古兽正在咆哮。",
        "古兽:\n天啊，我差点毁坏了大片森林！",
        "你被邪恶生物附体，还好没有酿成大错！",
        "古兽:\n谢谢你，精灵朋友，究竟是谁对我们下毒手，他到底想要做什么。",
        "接下来我们需要调查事情的幕后黑手！",
        "怎么就失败了呢？",
        "怎么就失败了呢？"
    };
    //true代表玩家顺序，false代表NPC顺序
    public List<bool[]> npcTalkOrder;
    private bool[] levelOneTalkOrder= { false,true,false,false,true,false,false,false};
    private bool[] levelTwoTalkOrder = { false,false,true,false,false};
    private bool[] levelThreeTalkOrder = { false, true, false, false };
    private bool[] levelFourTalkOrder = { false, false, false };
    private bool[] levelFiveTalkOrder = { false, false, true, false, false , false };
    private bool[] levelSixTalkOrder = { false, true, false, true, false, true, false };
    /// <summary>
    /// 初始化剧本相关内容
    /// </summary>
    private void InitStringList()
    {
        stringList = new List<string[]>
        {
            levelOne,
            levelTwo,
            levelThree,
            levelFour,
            levelFive,
            levelSix
        };
        npcTalkOrder = new List<bool[]>
        {
            levelOneTalkOrder,
            levelTwoTalkOrder,
            levelThreeTalkOrder,
            levelFourTalkOrder,
            levelFiveTalkOrder,
            levelSixTalkOrder
        };
    }
    #endregion

    #region 游戏管理
    //当前关卡索引
    public int currentLevelIndex;
    //当前关卡对象（接口对象）
    public ILevel currentLevel;
    //游戏是否开始的开关
    private bool gameStart;
    //当前关卡是否重玩
    private bool replay;
    /// <summary>
    /// 加载当前关卡对象，开始游戏
    /// </summary>
    public void StartGame()
    {
        if (currentLevelIndex>=100)
        {
            ResetGame();
            return;
        }
        NewLevel();
        currentLevel.InitLevel();
        gameStart = true;
    }

    private void ResetGame()
    {
        gameManager.InitGame();
        currentLevelIndex = 1;
        gameManager.Send("HidePanel");
        gameManager.RecyclePanel(StringManager.panelTalk);
        gameManager.RecyclePanel(StringManager.panelControl);
        gameManager.RecyclePlayer();
        gameManager.RecycleControlPanel();
    }

    /// <summary>
    /// 退出当前关卡
    /// </summary>
    public void ExitCurrentLevel()
    {
        currentLevel.ExitLevel();
        if (!replay)
        {
            currentLevelIndex++;
        }
        gameManager.HideMask();
        gameManager.Send("ShowPanel");
        StartGame();
        replay = false;
    }
    /// <summary>
    /// 重玩当前关卡
    /// </summary>
    public void ReplayGame()
    {
        replay = true;
    }
    /// <summary>
    /// 执行当前关卡的逻辑
    /// </summary>
    public void HandleGameLogic()
    {
        if (gameStart)
        {
            currentLevel.HandleLevelLogic();
        }
    }
    /// <summary>
    /// 实例化关卡对象
    /// </summary>
    private void NewLevel()
    {
        switch (currentLevelIndex)
        {
            case 1:
                currentLevel = new LevelOne(4);
                gameManager.Send("InitPanel", new TalkPanelMsg() { talks = stringList[currentLevelIndex - 1], talkOrder = npcTalkOrder[currentLevelIndex - 1], captionBetrayal = false, enterGame = 6 });
                break;
            case 2:
                //currentLevel = new LevelTwo(0, StringManager.attackEventID, 3);
                currentLevel = new LevelTwo(55);
                gameManager.Send("InitPanel", new TalkPanelMsg() { talks = stringList[currentLevelIndex - 1], talkOrder = npcTalkOrder[currentLevelIndex - 1], captionBetrayal = true, enterGame = 3 });
                break;
            case 3:
                //currentLevel = new LevelThree(0, StringManager.attackEventID, 3);
                currentLevel = new LevelThree(55);
                gameManager.Send("InitPanel", new TalkPanelMsg() { talks = stringList[currentLevelIndex - 1], talkOrder = npcTalkOrder[currentLevelIndex - 1], captionBetrayal = true, enterGame = 2 });
                break;
            case 4:
                currentLevel = new LevelFour(108);
                gameManager.Send("InitPanel", new TalkPanelMsg() { talks = stringList[currentLevelIndex - 1], talkOrder = npcTalkOrder[currentLevelIndex - 1], captionBetrayal = true, enterGame = 1 });
                break;
            case 5:
                currentLevel = new LevelFive(1);
                gameManager.Send("InitPanel", new TalkPanelMsg() { talks = stringList[currentLevelIndex - 1], talkOrder = npcTalkOrder[currentLevelIndex - 1], captionBetrayal = true, enterGame = 4 });
                break;
            case 6:
                currentLevel = new LevelSix(1);
                gameManager.Send("InitPanel", new TalkPanelMsg() { talks = stringList[currentLevelIndex - 1], talkOrder = npcTalkOrder[currentLevelIndex - 1], captionBetrayal = true, enterGame = 1 });
                break;
            default:
                break;
        }
    }
    #endregion


}

