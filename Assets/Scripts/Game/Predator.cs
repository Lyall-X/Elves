using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SonicBloom.Koreo;

/// <summary>
/// 起司尼首长
/// </summary>
public class Predator : EnemySnowman
{
    private Text tipText;
    private GameObject commandTipGo;
    private Animator animatorPredator;
    private bool beginMove;
    private Vector3 initPos;
    private LevelThree levelThree;

    private void Awake()
    {
        commandTipGo = transform.Find("CommandTip").gameObject;
        tipText = commandTipGo.transform.Find("Img_Tip").Find("Text").GetComponent<Text>();
        animatorPredator = GetComponent<Animator>();
        GameManager.Instance.Register("RunOutOfWindow", RunOutOfWindow);
        GameManager.Instance.Register("ShowTip", ShowTip);
        GameManager.Instance.Register("BeginMove", BeginMove);
        initPos = transform.position;
    }

    protected override void Update()
    {
        if (!beginMove)
        {
            return;
        }
        base.Update();
    }

    public override void Initialize(KoreographyEvent evt, int noteNum, LaneController laneController, Level level)
    {
        trackedEvent = evt;
        this.laneController = laneController;
        this.noteNum = noteNum;
        this.level = level;
        levelThree = level as LevelThree;
        animatorPredator.SetBool("run",true);
        transform.eulerAngles = new Vector3(0,180,0);
        HideTalkTip();
    }

    /// <summary>
    /// 显示提示内容
    /// </summary>
    /// <param name="tip"></param>
    private void ShowTip(object tip)
    {
        commandTipGo.SetActive(true);
        tipText.text = tip.ToString();
    }
    private void HideTalkTip()
    {
        commandTipGo.SetActive(false);
    }
    /// <summary>
    /// 跑出屏幕外，调用第一关的EnterLevel方法
    /// </summary>
    private void RunOutOfWindow(object obj=null)
    {
        transform.eulerAngles = Vector3.zero;
        animatorPredator.SetBool("run",true);
        transform.DOMoveX(10, 3).OnComplete(
            () =>
            {
                animatorPredator.SetBool("run", false);
                GameManager.Instance.Send("EnterLevel");
            }
        );
    }

    protected override void ReturnToPool()
    {
        GameManager.Instance.RecycleObj(StringManager.predator,gameObject,ResetNote);
        ResetState();
    }

    /// <summary>
    /// 重置起司尼状态
    /// </summary>
    public void ResetState()
    {
        willDestory = false;
        beginMove = false;
        transform.position = initPos;
        transform.eulerAngles = new Vector3(0,180,0);
        HideTalkTip();
    }

    public override void OnHit(int hitLevel)
    {
        HideTalkTip();
        willDestory = true;
        levelThree.HideMask();
        if (hitLevel>0)
        {
            DOTween.To(
            () => transform.position, r
               => transform.position = r,
            new Vector3(transform.position.x, -3.79f, transform.position.z),
            0.583f
            );
            animatorPredator.SetBool("die",true);
            GameManager.Instance.PlaySound(StringManager.swordSound);
            Invoke("ReturnToPool",0.667f);
        }
        else
        {
            GameManager.Instance.Send("PlayHurtAnim");
            GameManager.Instance.PlaySound(StringManager.missSound);
            animatorPredator.SetBool("run",false);
            animatorPredator.SetTrigger("attack");
            Invoke("ReturnToPool",1.083f);
        }
    }

    private void BeginMove(object obj=null)
    {
        beginMove = true;
    }
}
