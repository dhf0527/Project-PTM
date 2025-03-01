using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase_Unit : Unit
{
    private void Start()
    {
        SetHpBar();
        IsTeam = false;
    }

    private void Update()
    {

    }

    public override void Init()
    {
        //유닛 공격 유형/사이즈별 공격 범위 설정
        if (ud.attack_RangeType == AttackRange.Melee)
            ud.attack_Range = ud.size == Unit_Size.Small ? 0.8f : ud.size == Unit_Size.Medium ? 1f : 1.2f;
        else
            ud.attack_Range = ud.size == Unit_Size.Small ? 2f : ud.size == Unit_Size.Medium ? 2.5f : 3f;

        unitData_st.moveSpeed = ud.move_Speed;
        unitData_st.attackDamage = ud.damage;
        unitData_st.attackSpeed = ud.attack_Speed;
        unitData_st.accuracy = ud.accuracy;
        unitData_st.avoidance = ud.avoidance;
        unitData_st.armor = ud.armor;

        canKnockBack = false;
    }

    //체력바 생성 및 설정
    public override void SetHpBar()
    {
        //체력바를 world canvas에 생성
        hpBar = Instantiate(WorldCanavsManager.instance.hpBar_Prf, WorldCanavsManager.instance.worldCanvas_Trans);
        //체력바 연동
        hpBar.unit = this;
        //체력바 위치 설정
        hpBar.SetHpPos(2);
        //체력바 크기 설정
        hpBar.transform.localScale *= 2;

        //체력 설정
        Cur_Hp = ud.hp;
    }

    public override void Dead()
    {
        Time.timeScale = 0;
        Debug.Log("승리");
    }

}
