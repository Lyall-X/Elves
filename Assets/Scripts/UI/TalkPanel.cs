using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 对话框面板
/// </summary>
public class TalkPanel : MonoBehaviour
{
    //引用
    //对话框头像
    private GameObject[] imageAliens;//predator caption
    private Text[] texts;//left right
    private GameObject[] empTalkGos;//left right
    private GameObject imgTalkBG;
    private GameObject imgEvaluation;
    private Text texScore;
    private Text texEvaluation;
    private Button btnNextTalk;
    private GameManager gameManager;
    //对话信息
    private string[] currentTalks;
    private bool[] currentTalkOrder;
    private int talkIndex;
    private int enterGameIndex;
    //其他成员变量
    private int currentLevelIndex;//当前关卡索引
    private bool gameStart;//防止游戏多次开始
    private bool finishExecised;//防止训练模式多次开始

    private void Awake()
    {
        imageAliens = new GameObject[2];
        texts = new Text[2];
        empTalkGos = new GameObject[2];
        imgTalkBG = transform.Find("Img_TalkBG").gameObject;
        imgEvaluation = transform.Find("Img_Evaluation").gameObject;      
        empTalkGos[0] = imgTalkBG.transform.Find("Emp_Left").gameObject;
        empTalkGos[1]= imgTalkBG.transform.Find("Emp_Right").gameObject;
        imageAliens[0] = empTalkGos[0].transform.Find("Img_Predator").gameObject;
        imageAliens[1] = empTalkGos[0].transform.Find("Img_Caption").gameObject;
        texts[0] = empTalkGos[0].transform.Find("Tex_Talk").GetComponent<Text>();
        texts[1] = empTalkGos[1].transform.Find("Tex_Talk").GetComponent<Text>();
        texScore = imgEvaluation.transform.Find("Tex_Score").GetComponent<Text>();
        texEvaluation = imgEvaluation.transform.Find("Tex_Evaluation").GetComponent<Text>();
        btnNextTalk = GetComponent<Button>();
        gameManager = GameManager.Instance;
        btnNextTalk.onClick.AddListener(UpdateTalk);
        gameManager.Register("InitPanel", InitPanel);
        gameManager.Register("ShowPanel", ShowPanel);
        gameManager.Register("HidePanel", HidePanel);
        gameManager.Register("ShowCurrentLevelInformation", ShowCurrentLevelInformation);
    }
    /// <summary>
    /// 初始化面板
    /// </summary>
    private void InitPanel(object talkPanelMsgObj)
    {
        TalkPanelMsg talkPanelMsg = (TalkPanelMsg)talkPanelMsgObj;
        currentTalks = new string[talkPanelMsg.talks.Length];
        currentTalkOrder = new bool[talkPanelMsg.talkOrder.Length];
        enterGameIndex = talkPanelMsg.enterGame;
        for (int i = 0; i < talkPanelMsg.talks.Length; i++)
        {
            currentTalks[i] = talkPanelMsg.talks[i];
        }
        for (int i = 0; i < talkPanelMsg.talkOrder.Length; i++)
        {
            currentTalkOrder[i] = talkPanelMsg.talkOrder[i];
        }
        talkIndex = 0;
        if (talkPanelMsg.captionBetrayal)
        {
            imageAliens[1].SetActive(true);
            imageAliens[0].SetActive(false);
        }
        else
        {
            imageAliens[1].SetActive(false);
            imageAliens[0].SetActive(true);
        }
        currentLevelIndex = gameManager.GetCurrentLevelIndex();
        UpdateTalk();
    }
    /// <summary>
    /// 更新对话框提示以及推动游戏进程
    /// </summary>
    private void UpdateTalk()
    {
        UpdateText();
        if (talkIndex>=enterGameIndex&&!gameStart)
        {
            //进入当前游戏
            gameStart = true;
            HidePanel();
            gameManager.Send("EnterLevel");
            return;
        }
        //游戏结算内容显示
        if (talkIndex>=currentTalks.Length-2)
        {
            imgTalkBG.SetActive(false);
            imgEvaluation.SetActive(true);
            btnNextTalk.interactable = false;
            return;
        }
        talkIndex++;
    }

    /// <summary>
    /// 更新对话文本
    /// </summary>
    private void UpdateText()
    {
        if (currentTalkOrder[talkIndex])
        {
            empTalkGos[0].SetActive(false);
            empTalkGos[1].SetActive(true);
            texts[1].text = currentTalks[talkIndex];
        }
        else
        {
            empTalkGos[0].SetActive(true);
            empTalkGos[1].SetActive(false);
            texts[0].text = currentTalks[talkIndex];
        }
    }
    /// <summary>
    /// 隐藏对话框面板
    /// </summary>
    private void HidePanel(object obj=null)
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 显示对话框面板
    /// </summary>
    private void ShowPanel(object obj=null)
    {
        if (obj!=null)
        {
            TalkIndexMsg talkIndexMsg = (TalkIndexMsg)obj;
            if (talkIndexMsg.ifAdd)
            {
                talkIndex += talkIndexMsg.addCount;
            }
        }
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 显示结算面板以及更新相关信息
    /// </summary>
    private void ShowCurrentLevelInformation(object score)
    {
        UpdateTalk();
        //int playerScore = (int)score;
        //switch (playerScore)
        //{
        //    case 0:
        //        GameManager.Instance.PlaySound(StringManager.loseSound);
        //        break;
        //    case 1:
        //        texScore.text = "平凡";
        //        GameManager.Instance.PlaySound(StringManager.goodSound);
        //        break;
        //    case 2:
        //        texScore.text = "优秀";
        //        GameManager.Instance.PlaySound(StringManager.perfectSound);
        //        break;
        //    default:
        //        break;
        //}
        //texScore.gameObject.SetActive(true);
        //texEvaluation.gameObject.SetActive(false);
        //if (playerScore>0)
        //{
        //    texEvaluation.text = currentTalks[currentTalks.Length - 2];
        //}
        //else
        //{
        //    texEvaluation.text = currentTalks[currentTalks.Length - 1];
        //    //gameManager.ReplayGame();
        //}
        Invoke("ShowEvaliuation", 2);
    }
    /// <summary>
    /// 显示评价
    /// </summary>
    private void ShowEvaliuation()
    {
        texEvaluation.gameObject.SetActive(true);
        Invoke("ShowMask",2);
    }
    /// <summary>
    /// 显示遮罩
    /// </summary>
    private void ShowMask()
    {
        gameManager.ShowMask(ExitLevel);
    }
    /// <summary>
    /// 退出当前关卡
    /// </summary>
    private void ExitLevel()
    {
        talkIndex = 0;
        imgTalkBG.SetActive(true);
        imgEvaluation.SetActive(false);
        btnNextTalk.interactable = true;
        gameStart = false;
        finishExecised = false;
        gameManager.ExitCurrentLevel();
    }
}
