using System;
using System.Collections;
using System.Collections.Generic;
using Chracter;
using UnityEngine;

public class PlayerRangeSkill : MonoBehaviour
{
    private bool _Setting = false;
    private RaycastHit2D Enemy;
    private string EnemyTag;
    private float AttackDammage = 10;
    private float AttackRange = 1;
    private float Accuracy = 60;
    private bool Flip = false;

    private Vector3 firstSpawn;

    private Vector3 move = new Vector3(1, 0, 0);
    private Vector3 move2 = new Vector3(-1, 0, 0);


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
            SkillActivate();
    }

    public void SkillSetting(float damage)
    {
        EnemyTag = "Enemy";
        AttackDammage = damage;
        AttackRange = 1.5f;
        Accuracy = 200;
        _Setting = true;

    }
    
    
    private void SkillActivate()
    {
        if (this.transform.position.x - firstSpawn.x > AttackRange)
        {
            Destroy(gameObject);
        }
        transform.position = Vector2.MoveTowards(transform.position, transform.position + move, 0.1f);
    }
    //if hit enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(EnemyTag))
        {
            other.GetComponent<BaseCharacter>().TakeDamage(AttackDammage*2, Accuracy);
            other.GetComponent<BaseCharacter>().PrincessDebuff();
        }
    }
}
