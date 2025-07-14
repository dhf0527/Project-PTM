using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
}
