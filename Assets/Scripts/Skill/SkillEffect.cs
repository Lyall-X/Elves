using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillSkillEffect
{
    void InitSkill();
    void DoSkill();
    void DoEffect();
}

public abstract class SkillEffect : ISkillSkillEffect
{
    // 技能影响持续时间
    private int durationTime;
    // 造成的伤害
    private int damage;

    public void DoSkill()
    {
        throw new System.NotImplementedException();
    }

    public void InitSkill()
    {
        throw new System.NotImplementedException();
    }

    public void DoEffect()
    {
        throw new System.NotImplementedException();
    }
}
