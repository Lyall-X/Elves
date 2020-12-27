using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillMonoBase : MonoBehaviour
{
    //通用
    protected GameManager gameManager;
    const float textDurationTime = 0.5f;
    protected GameObject Effect_Skill;

    //受保护的
    private Text textSkill;

    public float durationTime = 1f;
    public int  damageHurt;

    void Awake()
    {
        Effect_Skill = transform.Find("Effect_Skill").gameObject;
        textSkill = transform.Find("Tip_Canvas").Find("Text_Skill").GetComponent<Text>();
        gameManager = GameManager.Instance;
        InitSkill();
    }
    private void Start()
    {
        gameObject.SetActive(false);
        Invoke("ShowPanel", 1f);
        SkillView();
        Invoke("DestorySelf", 1 + durationTime - textDurationTime);
    }
    private void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    // 定时器
    private float tempTime;
    private void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime >= 1f)//想间隔的时间
        {
            gameManager.DoMonsterDamage(-damageHurt);
            tempTime = 0f;
        }
    }
    protected virtual void InitSkill()
    {
    }
    protected abstract void SkillView();

    private void DestorySelf()
    {
        Tween skillTween = DOTween.To(() => textSkill.color, toColor => textSkill.color = toColor, new Color(textSkill.color.r, textSkill.color.g, textSkill.color.b, 0), textDurationTime);
        skillTween.OnComplete(() => { GameObject.Destroy(gameObject); });
        GameObject.Destroy(gameObject);
    }
}
