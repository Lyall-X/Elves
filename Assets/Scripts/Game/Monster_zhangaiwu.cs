using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_zhangaiwu : MonoBehaviour
{

    private GameManager gameManager;
    private Text tipText;
    private GameObject commandTipGo;
    private Animator animatorPredator;
    private bool beginMove;
    private Vector3 initPos;
    private bool playerRun;

    void Awake()
    {
        gameManager = GameManager.Instance;
        commandTipGo = transform.Find("CommandTip").gameObject;
        commandTipGo.SetActive(false);
        tipText = commandTipGo.transform.Find("Img_Tip").Find("Text").GetComponent<Text>();
        HideTalkTip();
        animatorPredator = GetComponent<Animator>();

        GameManager.Instance.Register("ShowTip", ShowTip);
        GameManager.Instance.Register("BeginMove", BeginMove);
        GameManager.Instance.Register("monsterHealth", MonsterHealth);
        gameManager.Register("PlayerRun", PlayerRun);
        initPos = transform.position;

    }
    void Start()
    {
        //animatorPredator.SetBool("run", true);
        //transform.eulerAngles = new Vector3(0, 180, 0);
        //HideTalkTip();
    }

    private void ShowTip(object tip)
    {
        commandTipGo.SetActive(true);
        tipText.text = tip.ToString();
        Invoke("HideTalkTip", 3f);
    }
    private void HideTalkTip()
    {
        commandTipGo.SetActive(false);
    }
    private void RunOutOfWindow(object obj = null)
    {
        transform.eulerAngles = Vector3.zero;
        animatorPredator.SetBool("run", true);
        transform.DOMoveX(10, 3).OnComplete(
            () =>
            {
                animatorPredator.SetBool("run", false);
                GameManager.Instance.Send("EnterLevel");
            }
        );
    }


    public void OnHit(int hitLevel)
    {
        HideTalkTip();
        if (hitLevel > 0)
        {
            DOTween.To(
            () => transform.position, r
               => transform.position = r,
            new Vector3(transform.position.x, -3.79f, transform.position.z),
            0.583f
            );
            animatorPredator.SetBool("die", true);
            GameManager.Instance.PlaySound(StringManager.swordSound);
            Invoke("ReturnToPool", 0.667f);
        }
        else
        {
            GameManager.Instance.Send("PlayHurtAnim");
            GameManager.Instance.PlaySound(StringManager.missSound);
            animatorPredator.SetBool("run", false);
            animatorPredator.SetTrigger("attack");
            Invoke("ReturnToPool", 1.083f);
        }
    }
    private void BeginMove(object obj = null)
    {
        beginMove = true;
    }
    private void MonsterHealth(object HealthObg)
    {
        int health = Convert.ToInt32(HealthObg);
        if (health != 100 && health != 10)
        {
            if (health == 0)
            {
                transform.Find("Monster_zhangaiwu").gameObject.SetActive(false);
                transform.Find("guang").gameObject.SetActive(false);
                transform.Find("wood").gameObject.SetActive(true);
                gameManager.SetPlayerCanRun(true);
                gameManager.Send("TaskFinish", true);
                return;
            }
            ShowTip("受到伤害");
        }
    }
    void Update()
    {
        if (playerRun)
        {
            CancelInvoke("UpdateWeaponPos");
            InvokeRepeating("UpdateWeaponPos", 0f, 0.01f);
        }
        else
        {
            CancelInvoke("UpdateWeaponPos");
        }
    }
    private void PlayerRun(object boRun)
    {
        playerRun = (bool)boRun;
    }
    private void UpdateWeaponPos()
    {
        if (transform.localPosition.x >= -3)
            transform.position -= new Vector3(0.0125f, 0, 0);
        else
        {
            if (!transform.Find("guang").gameObject.activeSelf)
            {
                transform.position -= new Vector3(0.0125f, 0, 0);
                if (transform.localPosition.x <= -4.5 && transform.Find("wood").gameObject.activeInHierarchy)
                {
                    transform.Find("wood").gameObject.SetActive(false);
                    gameManager.Send("ShowGuide", "获得水元素");
                    gameManager.SetControlBtnVisible("S", true);
                }
            }
            else
            {
                gameManager.SetPlayerCanRun(false);
                gameManager.PlayerStopRun();
            }
        }

    }
}
