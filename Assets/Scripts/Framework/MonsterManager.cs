using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 负责存贮怪物引用，播放相应动画，监听怪物的输入monsterTrans
/// </summary>
class MonsterManager
{
    private GameManager gameManager;
    private Transform monsterTrans;
    private Animator animator;
    private Vector3 initPos;
    private bool ifRun;

    private float monsterHealth = 0;

    public MonsterManager()
    {
        gameManager = GameManager.Instance;
    }

    public void CreateMonster(string monsterName, int heal)
    {
        if (monsterHealth == 0)
        {
            monsterTrans = gameManager.GetObj(StringManager.monsters + monsterName).transform;
            animator = monsterTrans.GetComponent<Animator>();
            initPos = monsterTrans.position;
            ShowMonster();
            monsterHealth = heal;
        }
        gameManager.Send("monsterHealth", monsterHealth);
    }
    public Vector3 GetMonsterPos()
    {
        if (monsterTrans)
            return monsterTrans.position;
        else
            return new Vector3(0, 0, 0);

    }

    #region 其他动画
    public void PlayHurtAnim()
    {
        if (animator)
            animator.SetTrigger("hurt");
    }
    private void PlayRunAnim()
    {
        if (animator)
            animator.SetBool("run", true);
    }
    private void StopRunAnim()
    {
        if (animator)
            animator.SetBool("run", false);
    }
    private void PlayDieAnim()
    {
        if(animator)
            animator.SetBool("die", true);
    }
    public void PlayAttackAnim()
    {
        if (animator)
            animator.SetTrigger("attack");
    }

    public void MonsterRun()
    {
        PlayRunAnim();
        ifRun = true;
    }

    private void StopRun()
    {
        StopRunAnim();
        ifRun = false;
    }
    #endregion
    #region 其他方法
    /// <summary>
    /// 显示怪物游戏物体
    /// </summary>
    public void ShowMonster()
    {
        monsterTrans.gameObject.SetActive(true);
    }
    /// <summary>
    /// 销毁怪物游戏物体
    /// </summary>
    public void RecycleMonster()
    {
        gameManager.RecycleObj(StringManager.monsters, monsterTrans.gameObject);
    }
    /// <summary>
    /// 重置怪物状态
    /// </summary>
    public void InitMonster()
    {
        monsterTrans.position = initPos;
        StopRun();
    }

    public Transform GetMonsterTrans()
    {
        return monsterTrans;
    }
    #endregion
    #region 血量
    // + 加血  - 减血
    public void DoMonsterDamage(float hurt)
    {
        if (hurt < 0 && animator)
            PlayHurtAnim();
        monsterHealth += hurt;
        if (monsterHealth <= 0)
        {
            monsterHealth = 0;
            PlayDieAnim();
        }
        else if (monsterHealth >= 100)
            monsterHealth = 100;
        gameManager.Send("monsterHealth", monsterHealth);
    }
    #endregion
}

