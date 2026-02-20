using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitSpawn : MonoBehaviour
{
    [HideInInspector] public Unit selected_Unit;
    [HideInInspector] public ItemData selected_Id;

    public void InitData()
    {
        selected_Unit = null;
        selected_Id = null;
    }

    public void OnTeamSpawnUnit()
    {
        if (selected_Unit == null)
            return;

        Unit spawned_unit = DunGeonManager_New.instance.SpawnUnit(selected_Unit);
        ApplyItemEffect(spawned_unit);
        ApplyUpgrade(spawned_unit);
        ApplyMeal(spawned_unit);
    }

    public void OnEnemySpawnUnit()
    {
        if (selected_Unit == null)
            return;

        Unit spawned_Unit = EnemySpawnManager.instance.Spawn_Unit(selected_Unit);
    }

    //아이템 효과 적용
    void ApplyItemEffect(Unit unit)
    {
        switch (selected_Id?.ItemCode)
        {
            case 0:
            case 100:
                unit.unitStatData_st.spawnCoolDown_MinusPercent += selected_Id.itemValue;
                break;
            case 1:
            case 101:
                unit.unitStatData_st.attack_Plus += selected_Id.itemValue * unit.ud.level;
                break;
            case 2:
            case 102:
                unit.unitStatData_st.attackSpeed_Plus += selected_Id.itemValue;
                break;
            case 3:
            case 103:
                unit.unitStatData_st.armor_Plus = (int)selected_Id.itemValue;
                break;
            case 4:
            case 104:
                unit.unitStatData_st.max_Hp_Plus += selected_Id.itemValue;
                break;
            case 5:
            case 105:
                unit.unitStatData_st.cost_MinusPercent += selected_Id.itemValue;
                break;
        }
    }

    #region 업그레이드 효과 적용
    void ApplyUpgrade(Unit unit)
    {
        
        //신입 모집
        if (unit.ud.level == 1)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 0);
            if (upgradeLv != 0)
                unit.unitStatData_st.cost_MinusPercent += GameManager.Instance.unitUpgradeDatas[0].upgradeValue[upgradeLv - 1];
        }
        //군사 훈련
        else if (unit.ud.level == 2)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 1);
            if (upgradeLv != 0)
                unit.unitStatData_st.attackSpeed_Plus += GameManager.Instance.unitUpgradeDatas[1].upgradeValue[upgradeLv - 1];
        }
        //성과 대우
        else if (unit.ud.level == 3)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 2);
            if (upgradeLv != 0)
                unit.unitStatData_st.spawnCoolDown_MinusPercent += GameManager.Instance.unitUpgradeDatas[2].upgradeValue[upgradeLv - 1];
        }
        //장갑 보강
        if (unit.ud.attack_RangeType == AttackRangeType.Melee)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 3);
            if (upgradeLv != 0)
                unit.unitStatData_st.max_Hp_Plus += GameManager.Instance.unitUpgradeDatas[3].upgradeValue[upgradeLv - 1] * 0.01f * unit.ud.hp;
        }
        //사격 훈련
        if (unit.ud.attack_RangeType == AttackRangeType.Ranged)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 4);
            if (upgradeLv != 0)
                unit.unitStatData_st.attack_PlusPercent += GameManager.Instance.unitUpgradeDatas[4].upgradeValue[upgradeLv - 1];
        }
        //개인 침낭
        if (unit.ud.size == Unit_Size.Small)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 5);
            if (upgradeLv != 0)
                unit.unitStatData_st.avoidance_Plus += (int)GameManager.Instance.unitUpgradeDatas[5].upgradeValue[upgradeLv - 1];
        }
        //대형 텐트
        if (unit.ud.size == Unit_Size.Medium || unit.ud.size == Unit_Size.Large)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 6);
            if (upgradeLv != 0)
                unit.unitStatData_st.moveSpeed_PlusPercent += GameManager.Instance.unitUpgradeDatas[6].upgradeValue[upgradeLv - 1];
        }
        //정의의 용병단
        if (unit.ud.faction == Faction.Guild || unit.ud.faction == Faction.Fairy)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 10);
            if (upgradeLv != 0)
            {
                unit.unitStatData_st.avoidance_Plus += (int)GameManager.Instance.unitUpgradeDatas[10].upgradeValue[upgradeLv - 1];
                unit.unitStatData_st.accuracy_Plus += (int)GameManager.Instance.unitUpgradeDatas[10].upgradeValue[upgradeLv - 1];
            }
        }
        //파괴의 용병단
        if (unit.ud.faction == Faction.Graveyard || unit.ud.faction == Faction.Demon)
        {
            int upgradeLv = PlayerPrefs.GetInt(ConstData.unitUpgrade + 11);
            if (upgradeLv != 0)
            {
                unit.unitStatData_st.cost_MinusPercent += GameManager.Instance.unitUpgradeDatas[11].upgradeValue[upgradeLv - 1];
                unit.unitStatData_st.spawnCoolDown_MinusPercent += GameManager.Instance.unitUpgradeDatas[11].upgradeValue[upgradeLv - 1];
            }
        }
    }
    #endregion

    #region 식사 효과 적용
    void ApplyMeal(Unit unit)
    {

        MealData md;
        //숙성 참치회
        if (GameManager.Instance.CheckAppliedMeal(8, out md))
            unit.unitStatData_st.attack_Plus += (EnemySpawnManager.instance.cur_Wave + 1) * (md.mealValue);
        //유리비늘 생선구이
        if (GameManager.Instance.CheckAppliedMeal(0, out md) && unit.ud.armor == 0)
            unit.unitStatData_st.armor_Plus += (int)md.mealValue;
        //칠면조 바비큐
        if (GameManager.Instance.CheckAppliedMeal(3, out md))
            unit.unitStatData_st.max_Hp_Plus += md.mealValue;
        //파인애플 피자
        if (GameManager.Instance.CheckAppliedMeal(106, out md))
        {
            unit.unitStatData_st.isFixed_AttackSpeed = true;
            unit.unitStatData_st.fixedAttackSpeed = md.mealValue;
        }
        //로즈베리 케이크
        if (GameManager.Instance.CheckAppliedMeal(104, out md))
        {
            unit.unitStatData_st.isNoTypeDamage = true;
            unit.unitStatData_st.isPenetration = true;
        }
        //불사조 닭발
        if (GameManager.Instance.CheckAppliedMeal(100, out md))
            unit.unitStatData_st.attack_PlusPercent += md.mealValue2;
        //든든 국밥
        if (GameManager.Instance.CheckAppliedMeal(101, out md))
        {
            unit.unitStatData_st.avoidance_Plus += (int)md.mealValue;
            unit.unitStatData_st.accuracy_Plus += (int)md.mealValue2;
        }
        //정체불명 햄버거
        if (GameManager.Instance.CheckAppliedMeal(102, out md))
            unit.unitStatData_st.cost_MinusPercent += md.mealValue;
        //드워프 맥주
        if (GameManager.Instance.CheckAppliedMeal(105, out md))
        {
            unit.unitStatData_st.damageReduction_PlusPercent -= md.mealValue;
            unit.unitStatData_st.attackBoost_PlusPercent += md.mealValue2;
        }
        //드래곤알 오믈렛
        if (GameManager.Instance.CheckAppliedMeal(200, out md))
        {
            unit.unitStatData_st.cost_MinusPercent += md.mealValue;
        }
    }
    #endregion

    public void TestUnitDelete()
    {
        foreach (var onStageUnit in DunGeonManager_New.instance.onStageUnits_Test)
        {
            Destroy(onStageUnit.gameObject);
            Destroy(onStageUnit.hpBar.gameObject);
        }
        DunGeonManager_New.instance.onStageUnits_Test.Clear();
    }
    
    public void TestToWave(int setWave)
    {
        EnemySpawnManager.instance.Test_SetWave(setWave);
    }
}
