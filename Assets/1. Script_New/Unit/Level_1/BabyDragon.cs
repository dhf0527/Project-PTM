using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragon : Unit
{
    [SerializeField] int attackPerWave;

    public override void Init()
    {
        base.Init();
        unitStatData_st.attack_Plus += EnemySpawnManager.instance.cur_Wave * attackPerWave;
        Debug.Log("공격력 증가" + EnemySpawnManager.instance.cur_Wave * attackPerWave);
    }
}
