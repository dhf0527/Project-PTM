using System;
using System.Collections;
using System.Collections.Generic;
using Chracter;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    private bool _Setting = false;
    private RaycastHit2D Enemy;
    private string EnemyTag;
    private float AttackDammage = 10;
    private float AttackRange = 1;
    private float Accuracy = 60;
    private int MaxHit = 1;

    private Vector3 firstSpawn;

    private Vector3 move = new Vector3(1, 0, 0);
    private Vector3 move2 = new Vector3(-1, 0, 0);
    
    public bool skull = false;


    // Start is called before the first frame update
    void Start()
    {
        firstSpawn = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_Setting)
            FlytoEnemy();
    }

    public void EnemySetting(RaycastHit2D hit, string enemyTag, float attackDammage, float attackRange=2, float accuracy=60,int maxHit=1)
    {
      
        EnemyTag = enemyTag;
        if (EnemyTag == "Team")
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        Enemy = hit;
        AttackDammage = attackDammage;
        AttackRange = attackRange;
        Accuracy = accuracy;
        _Setting = true;

        if (maxHit<1)
        {
            MaxHit = 1;
        }
        else
        {
            MaxHit = maxHit;
        }
    }
    
    
    private void FlytoEnemy()
    {

            if(EnemyTag == "Team")
            {
                if(this.transform.position.x - firstSpawn.x < -AttackRange * 1.2)
                {
                    Destroy(gameObject);
                }
                transform.position = Vector2.MoveTowards(transform.position,transform.position+move2, 0.1f);
            }
            else
            {
                if(this.transform.position.x - firstSpawn.x > AttackRange * 1.2)
                {
                    Destroy(gameObject);
                }
                transform.position = Vector2.MoveTowards(transform.position,transform.position+move, 0.1f);
            }
            
        
    }
    //if hit enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (skull)
        {
            if (other.CompareTag(EnemyTag))
            {
                MaxHit--;
                other.GetComponent<BaseCharacter>().TakeDamage(AttackDammage,Accuracy);
                other.GetComponent<BaseCharacter>().TakeDamageSkull(4);

                if (MaxHit <= 0)
                {
                    Destroy(gameObject);
                }
            }   
        }
        else
        {
            if (other.CompareTag(EnemyTag))
            {
                MaxHit--;
                other.GetComponent<BaseCharacter>().TakeDamage(AttackDammage,Accuracy);

                if (MaxHit <= 0)
                {
                    Destroy(gameObject);
                }
            }    
        }
        
    }
}
