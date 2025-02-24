using Chracter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    public Tower tower;
    public EnemyTower enemyTower;
    public Item[] unitItems;
    public GameObject[] unitObjs;
    public Transform teamSpawnPoint;
    public Transform enemySpawnPoint;
    public Text stageTimeText;

    private void Update()
    {
        StageTime();
    }

    public void GoldMax()
    {
        tower.currentGold = tower.maxGold;
    }

    public void UnityTimeRest()
    {
        for (int i = 0; i < unitItems.Length; i++)
        {
            unitItems[i].spawnDelay = 0;
        }
    }

    public void TowerHpHill()
    {
        tower.CurrentHealth = tower.MaxHealth;
    }

    public void UnitHpHill()
    {
        GameObject[] Units = GameObject.FindGameObjectsWithTag("Team");
        for (int i = 0; i < Units.Length; i++)
        {
            BaseCharacter bc = Units[i].GetComponent<BaseCharacter>();
            bc.CurrentHealth = bc.MaxHealth;
        }
    }

    private int unitIndex;
    public void CreateTeamUnitCheat(int index)
    {
        unitIndex = index;

        GameObject team = GameObject.Instantiate(unitObjs[unitIndex], teamSpawnPoint);
        team.tag = "Team";
        team.layer = 7;
        team.GetComponent<BaseCharacter>().Spawn();
    }

    public void CreateEnemyUnitCheat(int index)
    {
        unitIndex = index;

        GameObject enemy = GameObject.Instantiate(unitObjs[unitIndex], enemySpawnPoint);
        enemy.tag = "Enemy";
        enemy.layer = 7;
        enemy.GetComponent<BaseCharacter>().Spawn();
    }

    public void StageTime()
    {
        stageTimeText.text = "남은 시간 : " + StageManager.Instance.stageTime.ToString();
    }
}
