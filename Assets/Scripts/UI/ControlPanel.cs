using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 对话框面板
/// </summary>
public class ControlPanel : MonoBehaviour
{
    //引用
    private GameManager gameManager;
    // UI
    private Text textCombo;
    private Text tipText;
    private GameObject commandTipGo;
    private GameObject ControlList;
    private GameObject playerObj;
    private GameObject MonsterObj;

    private GameObject PlayerTips;
    private GameObject MonsterTips;

    private GameObject Weapon;

    private List<GameObject> ControlViews;

    private int times;
    private bool playerRun;

    private Transform playerPoint;
    private int count = 0;

    private void Awake()
    {
        textCombo = transform.Find("Text_Combo").GetComponent<Text>();
        textCombo.gameObject.SetActive(false);
        ControlList = transform.Find("Control_List").gameObject;

        commandTipGo = transform.Find("CommandTip").gameObject;
        tipText = commandTipGo.transform.Find("Img_Tip").Find("Text").GetComponent<Text>();
        PlayerTips = transform.Find("Player").Find("PlayerTips").gameObject;
        MonsterTips = transform.Find("Monster").Find("MonsterTips").gameObject;
        Weapon = transform.Find("Weapon").gameObject;

        playerObj = transform.Find("Player").gameObject;
        MonsterObj = transform.Find("Monster").gameObject;

        playerPoint = transform.Find("PlayerPoint");

        ControlViews = new List<GameObject>();

        gameManager = GameManager.Instance;
        gameManager.Register("InitPanel", InitPanel);
        gameManager.Register("EnterLevel", EnterLevel);
        gameManager.Register("ShowPanel", ShowPanel);
        gameManager.Register("PlayerControl", PlayerControl);
        gameManager.Register("ComboFinish", ComboFinish);
        gameManager.Register("PlayerHealth", PlayerHealth);
        gameManager.Register("ShowGuide", ShowGuide);
        gameManager.Register("monsterHealth", MonsterHealth);
        gameManager.Register("PlayerRun", PlayerRun);

        //特殊事件发送
        gameManager.Register("ShowControlPanelExtra", ShowControlPanelExtra);


    }
    private void InitPanel(object talkPanelMsgObj)
    {
        gameObject.SetActive(false);
    }
    private void EnterLevel(object EnterLevelObj)
    {
        gameObject.SetActive(true);
    }

    private void ShowPanel(object ShowPanelObj)
    {
        gameObject.SetActive(false);
    }
    private void PlayerControl(object ControlBtnObj)
    {
        int skilltype = Convert.ToInt32(ControlBtnObj);
        GameObject view = GameManager.Instance.GetObj(StringManager.controlView);
        Image skillImage = view.transform.Find("Image").GetComponent<Image>();
        skillImage.sprite = Resources.Load("Sprites/Control/" + skilltype, typeof(Sprite)) as Sprite; ;
        DOTween.To(() => skillImage.color, toColor => skillImage.color = toColor, new Color(skillImage.color.r, skillImage.color.g, skillImage.color.b, 1), 0.5f);

        view.transform.SetParent(ControlList.transform);
        view.transform.localPosition = new Vector3(ControlViews.Count * 150, 0, 0);
        view.transform.localScale = Vector3.one;
        ControlViews.Add(view);
    }

    private void ComboFinish(object boSuccessObj)
    {
        bool boSuccess = Convert.ToBoolean(boSuccessObj);
        int times = gameManager.GetComboTimes();
        textCombo.gameObject.SetActive(times != 0);
        textCombo.text = times.ToString();
        foreach (GameObject obj in ControlViews)
        {
            Image skillImage = obj.transform.Find("Image").GetComponent<Image>();
            Destroy(skillImage);
        }

        ControlViews.Clear();
        Invoke("ComboFinishClear", 1f);
    }
    private void ComboFinishClear()
    {
        textCombo.gameObject.SetActive(false);
    }

    private void PlayerHealth(object HealthObg)
    {
        int health = Convert.ToInt32(HealthObg);
        playerObj.transform.Find("Player_Health_Text").GetComponent<Text>().text = health.ToString();
        playerObj.transform.Find("Player_Health_Slider").GetComponent<Slider>().value = (float)(health) / 100;
    }
    private void ShowGuide(object tip)
    {
        string tips = tip.ToString();
        string head = tips.Substring(0, 3);
        string msg = tips.Substring(3, tips.Length - 3);
        msg = msg.Replace("\\n", "\n");
        switch (head)
        {
            case "玩家 ":
                PlayerTips.SetActive(true);
                PlayerTips.transform.Find("Img_Tip").Find("Text").GetComponent<Text>().text = msg;
                Invoke("HidePlayerTips", 3f);
                break;
            case "怪物 ":
                GameManager.Instance.Send("ShowTip", msg);
                //MonsterTips.SetActive(true);
                //MonsterTips.transform.Find("Img_Tip").Find("Text").GetComponent<Text>().text = msg;
                //Invoke("HideMonsterTips", 2f);
                break;
            default:
                commandTipGo.SetActive(true);
                tipText.text = tips;
                Invoke("HideTalkTip", 3f);
                break;
        }
    }
    private void HidePlayerTips()
    {
        PlayerTips.SetActive(false);
    }
    private void HideMonsterTips()
    {
        MonsterTips.SetActive(false);
    }
    private void HideTalkTip()
    {
        commandTipGo.SetActive(false);
    }
    private void MonsterHealth(object HealthObg)
    {
        int health = Convert.ToInt32(HealthObg);
        if (health == 100 || health == 10)
        {
            MonsterObj.gameObject.SetActive(true);

        }
        else if (health == 0)
        {
            MonsterObj.gameObject.SetActive(false);
        }
        MonsterObj.transform.Find("Monster_Health_Text").GetComponent<Text>().text = health.ToString();
        MonsterObj.transform.Find("Monster_Health_Slider").GetComponent<Slider>().value = (float)(health) / 100;
    }

    private void ShowControlPanelExtra(object MeathonObg)
    {
        string meathon = MeathonObg.ToString();
        switch (meathon)
        {
            case "pickUpWeapon":
                pickUpWeapon();
                break;
            default:
                break;
        }

    }

    private void Update()
    {
        // 武器
        if (playerRun && Weapon.activeSelf)
        {
            CancelInvoke("UpdateWeaponPos");
            InvokeRepeating("UpdateWeaponPos", 0f, 0.01f);
        }
        else
        {
            CancelInvoke("UpdateWeaponPos");
        }
        // Boss
    }
    private void PlayerRun(object boRun)
    {
        playerRun = (bool)boRun;
    }
    private void UpdateWeaponPos()
    {
        if (Weapon.transform.localPosition.x >= -452)
            Weapon.transform.position -= new Vector3(1.5f, 0, 0);
        else
        {
            gameManager.Send("ShowGuide", "获得武器");
            Weapon.SetActive(false);
            gameManager.SetWeaponPlayer();
            gameManager.Send("TaskFinish", true);
            gameManager.SetControlBtnVisible("A", true);
        }

    }

    private void pickUpWeapon()
    {
        if (count == 0)
        {
            Weapon.SetActive(true);
            count++;
        }
    }


}
