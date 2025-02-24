using System;
using System.Collections;
using System.Collections.Generic;
using Chracter;
using UnityEngine;

public class RangedAttackSkull : RangedAttack
{
    private bool _Setting = false;
    private RaycastHit2D Enemy;
    private string EnemyTag = "Team";
    private float AttackDammage = 10;
    private float AttackRange = 1;
    private float Accuracy = 60;
    private int MaxHit = 1;

    private Vector3 firstSpawn;

    private Vector3 move = new Vector3(1, 0, 0);
    private Vector3 move2 = new Vector3(-1, 0, 0);


    //if hit enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(EnemyTag))
        {
            MaxHit--;
            
            if (MaxHit <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
