using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Princess : Hero
{
    [Header("방패 강타 데미지 비율(%)")]
    [SerializeField] float skill1_Damage;
    [SerializeField] float skill1_buff_Time;
    [SerializeField] float skill1_move_Decrease;
    [SerializeField] float skill1_attackSpeed_Decrease;

    [SerializeField] int skill2_accuracy_increase;
    [SerializeField] float skill2_buff_Time;

    //공주 스킬1: 방패 강타
    public override void OnSkill1()
    {
        nonCombatTime = 0;
        float skillRange = 1.5f;

        //스캔할 레이어 설정
        string target_Layer = EnemyLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, skillRange, LayerMask.GetMask(target_Layer));

        foreach (var item in hits)
        {
            Unit target_Unit = item.collider.GetComponent<Unit>();
            //넉백
            target_Unit.OnStartKnockBack();
            //데미지 부여
            float dmg = (skill1_Damage * 0.01f) * AttackDamage * (1+ unitStatData_st.attackBoost_PlusPercent * 0.01f);
            ApplyAttack(target_Unit, dmg, AttackType.Physical);
            //디버프 처리
            ShieldSmiteDebuff shieldSmiteDebuff = target_Unit.AddComponent<ShieldSmiteDebuff>();
            shieldSmiteDebuff.buff_Time = skill1_buff_Time;
            shieldSmiteDebuff.attackSpeed_Decrease = skill1_attackSpeed_Decrease;
            shieldSmiteDebuff.move_Decrease = skill1_move_Decrease;
        }
        StartCoroutine(C_Skill1_CoolDown());

        AudioManager.Instance.PlayerSfx(SFX_Enum.ShieldSmite);
    }

    //공주 스킬2: 부러진 영웅검
    public override void OnSkill2()
    {
        nonCombatTime = 0;
        float skillRange = 1f;

        //스캔할 레이어 설정
        string target_Layer = TeamLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.left * skillRange, Vector2.right, skillRange * 2, LayerMask.GetMask(target_Layer));

        foreach (var item in hits)
        {
            if (item.collider.GetComponent<TeamBase_Unit>())
                continue;

            Unit target_Unit = item.collider.GetComponent<Unit>();
            //버프 처리
            BrokenHeroSword brokenHeroSword = target_Unit.AddComponent<BrokenHeroSword>();
            brokenHeroSword.buff_Time = skill2_buff_Time;
            brokenHeroSword.accuracy_increase = skill2_accuracy_increase;
        }
        StartCoroutine(C_Skill2_CoolDown());

        AudioManager.Instance.PlayerSfx(SFX_Enum.BrokenHeroSword);
    }
}
