using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamBase_Unit : Unit
{
    int base_level = 1;
    public int Base_level
    {
        get { return base_level; }
        set 
        {
            base_level = value;
            DunGeonManager_New.instance.baseLevelUpPanel.Set_LevelText(base_level);
            DunGeonManager_New.instance.baseLevelUpPanel.Set_CostText(DunGeonManager_New.instance.base_abillitiesByLevels[base_level - 1].base_UpgradeCost_By_Level);
        }
    }

    private void Start()
    {
        SetHpBar();
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

        unitData_st.max_Hp = ud.hp;
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
        Cur_Hp = unitData_st.max_Hp;
    }

    public override void Dead()
    {
        Time.timeScale = 0;
        Debug.Log("패배");
    }

    //요새 레벨업을 했을 때 호출
    public void Base_LevelUp()
    {
        Base_level++;
        float tmp_max_Hp = unitData_st.max_Hp;
        Set_BaseAbillityByLevel(DunGeonManager_New.instance.base_abillitiesByLevels[Base_level - 1]);
        //최대 체력이 오른만큼 현재 체력 상승
        Cur_Hp += unitData_st.max_Hp - tmp_max_Hp;
    }

    //레벨에 따라 능력치를 설정
    public void Set_BaseAbillityByLevel(DunGeonManager_New.AbillitiesByLevel base_abillitiesByLevel)
    {
        
        unitData_st.max_Hp = base_abillitiesByLevel.base_Hp_By_Level;
        unitData_st.armor = base_abillitiesByLevel.base_Armor_By_Level;
        /*
        //업그레이드 비용
        DunGeonManager_New.instance.Gold_Per_Sec = base_abillitiesByLevels[base_level - 1].base_GoldPerSec_By_Level;
        DunGeonManager_New.instance.Max_Gold = base_abillitiesByLevels[base_level - 1].base_MaxGold_By_Level;
        */
    }

}



