using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_000 : SkillMonoBase
{

    protected override void InitSkill()
    {
        base.InitSkill();

        Effect_Skill.transform.position = new Vector3( gameManager.GetMonsterPos().x, Effect_Skill.transform.position.y,0);
    }
    protected override void SkillView()
    {
        
    }
}
