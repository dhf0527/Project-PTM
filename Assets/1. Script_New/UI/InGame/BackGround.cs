using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{

    public void OnBossSpawn()
    {
        EnemySpawnManager.instance.SpawnBossUnit();
    }
}
