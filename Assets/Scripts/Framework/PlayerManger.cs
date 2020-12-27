using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 负责存贮玩家引用，播放相应动画，监听玩家的输入
/// </summary>
class PlayerManger
{
    private GameManager gameManager;
    private Transform playerTrans;
    private Vector3 initPos;
    private Tween jumpTween;
    private bool ifRun;
    private PlayerSkillType playerSkillType = PlayerSkillType.OVER;

    private List<PlayerAttackType> playerCombo;

    // 玩家血量
    private float playerHealth = 0;
    // 输入球的个数
    private const int INPUT = 4;
    // 音符越界数量
    private int FinishCount = 0;

    private bool playerCanRun = true;
    private bool playerCanInput = false;

    private int comboTimes = 0;

    public PlayerManger()
    {
        gameManager = GameManager.Instance;
        playerTrans = gameManager.GetObj(StringManager.player).transform;
        playerTrans.Find("Player").gameObject.SetActive(true);
        playerTrans.Find("Player2").gameObject.SetActive(false);
        playerCombo = new List<PlayerAttackType>();
        ShowPlayer();
        initPos = playerTrans.position;
        InitJumpTween();
    }

    public int GetCombo()
    {
        return comboTimes;
    }


    #region 监听玩家输入
    // 定时器
    private float tempTime;
    public void InputMonitor()
    {
        gameManager.Send("CheckNoteHit");
        if (playerCombo.Count < INPUT && playerCanInput)
        {
            if (Input.GetKeyDown(KeyCode.A) || gameManager.CheckButtonPressed(0))
            {
                AddPlayerCombo(PlayerAttackType.FIRE);
                GameManager.Instance.PlaySound(StringManager.playerSkillSound);
                SetPlayerCanInput(false);
            }
            else if (Input.GetKeyDown(KeyCode.S) || gameManager.CheckButtonPressed(1))
            {
                AddPlayerCombo(PlayerAttackType.ICE);
                GameManager.Instance.PlaySound(StringManager.playerSkillSound);
                SetPlayerCanInput(false);
            }
            else if (Input.GetKeyDown(KeyCode.D) || gameManager.CheckButtonPressed(2)) 
            {
                AddPlayerCombo(PlayerAttackType.WOOD);
                GameManager.Instance.PlaySound(StringManager.playerSkillSound);
                SetPlayerCanInput(false);
            }
            else if (Input.GetKeyDown(KeyCode.Space) || gameManager.CheckButtonPressed(9)) 
            {
                AddPlayerCombo(PlayerAttackType.RUN);
                GameManager.Instance.PlaySound(StringManager.playerRunSound);
                SetPlayerCanInput(false);
            }
        }

        if (ifRun)
        {
            tempTime += Time.deltaTime;
            if (tempTime >= 3f)//想间隔的时间
            {
                StopRun();
                tempTime = 0f;
            }
        }
    }
    #endregion
    #region 跳跃动画

    public void SetPlayerCanRun(bool canrun)
    {
        playerCanRun = canrun;
    }
    private void InitJumpTween()
    {
        jumpTween = playerTrans.DOLocalMoveY(0,0.16f);
        jumpTween.SetEase(Ease.OutCubic);
        jumpTween.SetAutoKill(false);
        jumpTween.Pause();
        jumpTween.OnComplete(JumpDown);
    }

    private void JumpUp()
    {
        jumpTween.PlayForward();
    }

    private void JumpDown()
    {
        jumpTween.PlayBackwards();
    }
    private void PlayAttackAnim()
    {
        playerTrans.Find("Player").GetComponent<Animator>().SetTrigger("attack");
        playerTrans.Find("Player2").GetComponent<Animator>().SetTrigger("attack");
    }

    #endregion
    #region 其他动画
    public void PlayHurtAnim()
    {
        playerTrans.Find("Player").GetComponent<Animator>().SetTrigger("hurt");
        playerTrans.Find("Player2").GetComponent<Animator>().SetTrigger("hurt");
    }
    private void PlayRunAnim()
    {
        playerTrans.Find("Player").GetComponent<Animator>().SetBool("run", true);
        playerTrans.Find("Player2").GetComponent<Animator>().SetBool("run", true);
    }
    private void PlayRongHeAnim()
    {
        playerTrans.Find("Player2").Find("ronghe").GetComponent<Animator>().SetTrigger("attack");
    }
    private void StopRunAnim()
    {
        playerTrans.Find("Player").GetComponent<Animator>().SetBool("run", false);
        playerTrans.Find("Player2").GetComponent<Animator>().SetBool("run", false);
    }
    private void StopBreakAnim()
    {
        playerTrans.Find("Player").GetComponent<Animator>().SetTrigger("break");
        playerTrans.Find("Player2").GetComponent<Animator>().SetTrigger("break");
    }

    private void PlayerRun()
    {
        PlayRunAnim();
        gameManager.Send("PlayerRun", true);
        ifRun = true;
    }

    public void StopRun()
    {
        StopRunAnim();
        gameManager.Send("PlayerRun", false);
        ifRun = false;
    }
    #endregion
    #region 其他方法
    public void SetPlayerSkillType(PlayerSkillType playerSkillType)
    {
        if (FinishCount == 1 && playerCombo.Count > 0)
        {
            FinishPlayerCombo();
        }
        FinishCount++;
        this.playerSkillType = playerSkillType;
        //StopRun();
    }

    public void SetPlayerCanInput(bool canInput)
    {
        this.playerCanInput = canInput;
    }
    /// <summary>
    /// 显示玩家游戏物体
    /// </summary>
    public void ShowPlayer()
    {
        playerTrans.gameObject.SetActive(true);
    }
    /// <summary>
    /// 销毁玩家游戏物体
    /// </summary>
    public void RecyclePlayer()
    {
        gameManager.RecycleObj(StringManager.player,playerTrans.gameObject);
    }
    /// <summary>
    /// 重置玩家状态
    /// </summary>
    public void InitPlayer()
    {
        playerTrans.position = initPos;
        StopRun();
    }
    public Vector3 GetPlayerPos()
    {
        return initPos;
    }
    public void SetWeaponPlayer()
    {
        playerTrans.Find("Player2").gameObject.SetActive(true);
        playerTrans.Find("Player").gameObject.SetActive(false);
    }

    public Transform GetPlayerTrans()
    {
        return playerTrans;
    }
    #endregion
    #region 玩家技能组合
    private void AddPlayerCombo(PlayerAttackType type)
    {
        //重置越界的音符
        FinishCount = 0;

        playerCombo.Add(type);
        gameManager.Send("PlayerControl", type);
        playerSkillType = PlayerSkillType.OVER;
        //达到技能组合键
        if (playerCombo.Count > 0 && playerCombo.Count < INPUT)
        {
            bool check1 = false;
            bool check2 = false;
            for ( int i = 0; i< playerCombo.Count; i++)
            {

                if (playerCombo[i] == PlayerAttackType.RUN)
                {
                    check1 = true;
                }else
                {
                    check2 = true;
                }
            }
            if (check1 && check2)
                FinishPlayerCombo();
        }
        else if (playerCombo.Count == INPUT)
        {
            FinishPlayerCombo();
        }
    }
    private void FinishPlayerCombo()
    {
        if ((GetPlayerComboString() == "0009" || GetPlayerComboString() == "0019" || GetPlayerComboString() == "0119") && playerTrans.Find("Player2").gameObject.activeSelf)
        //if ((GetPlayerComboString() == "0009" || GetPlayerComboString() == "0019" || GetPlayerComboString() == "0119"))
        {
            comboTimes++;
            gameManager.GetObj(StringManager.skillEffect + GetPlayerComboString());
            gameManager.Send("ComboFinish", true);
            PlayRongHeAnim();
            PlayAttackAnim();
        }
        else if (GetPlayerComboString() == "9999")
        {
            comboTimes++;
            if (playerCanRun)
            {
                PlayerRun();
            }
            else
            {
                StopBreakAnim();
                gameManager.Send("ShowGuide", "玩家 它挡住我了,试着消灭它");
            }
            gameManager.Send("ComboFinish", true);
        }
        else
        {
            comboTimes = 0;
            gameManager.Send("ComboFinish", false);
            StopBreakAnim();
        }
        playerCombo.Clear();
    }
    public List<PlayerAttackType> GetPlayerCombo()
    {
        return playerCombo;
    }
    private string GetPlayerComboString()
    {
        if (playerCombo.Count != INPUT)
            return "";
        List<int> temp = new List<int>();
        foreach (PlayerAttackType type in playerCombo)
        {
            temp.Add(Convert.ToInt32(type));
        }
        temp.Sort((x, y) => x.CompareTo(y));
        string ret = "";
        foreach(int order in temp)
        {
            ret += order.ToString();
        }
        //return ret.Substring(0, ret.Length - 1);
        return ret;
    }
    #endregion
    #region 血量
    // + 加血  - 减血
    public void DoPlayerDamage(float hurt)
    {
        playerHealth += hurt;
        if (playerHealth <= 0)
            playerHealth = 0;
        else if (playerHealth >= 100)
            playerHealth = 100;
        gameManager.Send("PlayerHealth", playerHealth);
    }
    #endregion
}

public enum PlayerSkillType
{
    SKILL,
    OVER,
}
public enum PlayerAttackType
{
    FIRE,
    ICE,
    WOOD,

    RUN = 9,
}

