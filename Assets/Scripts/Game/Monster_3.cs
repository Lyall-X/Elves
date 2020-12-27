using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_3 : MonoBehaviour
{

    private GameManager gameManager;
    private Text tipText;
    private GameObject commandTipGo;
    private Animator animatorPredator;
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
        //HideTalkTip();
        //if (hitLevel > 0)
        //{
        //    DOTween.To(
        //    () => transform.position, r
        //       => transform.position = r,
        //    new Vector3(transform.position.x, -3.79f, transform.position.z),
        //    0.583f
        //    );
        //    animatorPredator.SetBool("die", true);
        //    GameManager.Instance.PlaySound(StringManager.swordSound);
        //    Invoke("ReturnToPool", 0.667f);
        //}
        //else
        //{
        //    GameManager.Instance.Send("PlayHurtAnim");
        //    GameManager.Instance.PlaySound(StringManager.missSound);
        //    animatorPredator.SetBool("run", false);
        //    animatorPredator.SetTrigger("attack");
        //    Invoke("ReturnToPool", 1.083f);
        //}
    }
    private void MonsterHealth(object HealthObg)
    {
        int health = Convert.ToInt32(HealthObg);
        if (health != 100 )
        {
            if (health == 0)
            {
                gameObject.SetActive(false);
                gameManager.SetPlayerCanRun(true);
                gameManager.Send("TaskFinish", true);
                return;
            }
            ShowTip("受到伤害");
        }
    }
    private float tempTime;
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

        tempTime += Time.deltaTime;

        //if (tempTime >= 3f && transform.localPosition.x <= 5.5)//想间隔的时间
        if (tempTime >= 5f)//想间隔的时间
        {
            PlaySkill();
            tempTime = 0;
        }
    }
    private void PlayerRun(object boRun)
    {
        playerRun = (bool)boRun;
    }

    private void UpdateWeaponPos()
    {

        if (transform.localPosition.x >= 5.5)
        {
            transform.position -= new Vector3(0.0125f, 0, 0);
        }

        else
        {
            gameManager.SetPlayerCanRun(false);
            gameManager.PlayerStopRun();
        }

    }

    private void PlaySkill()
    {
        gameManager.PlayAttackAnim();
        Invoke("PlaySkillEffect", 1.5f);
    }

    private void PlaySkillEffect()
    {
        transform.Find("skill1").GetComponent<Animator>().SetTrigger("skill1");
        Invoke("PlaySkillEffect2", 1f);
    }
    private void PlaySkillEffect2()
    {
        transform.Find("skill2").GetComponent<Animator>().SetTrigger("skill2");
        gameManager.DoPlayerDamage(-10);
        transform.Find("skill2").transform.position = new Vector3(gameManager.GetPlayerPos(false).x - 1, transform.Find("skill2").transform.position.y, 0);
    }

}
