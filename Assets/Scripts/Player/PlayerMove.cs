using System.Collections;
using System.Collections.Generic;
using Chracter;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : BaseCharacter
{

    private bool Attacking = false;
    private bool ismoving = false;
    private bool isSkillMotion = false;
    [SerializeField] protected GameObject rangedAttackPrefableft = null;
    private bool inBattle = true;
    private bool canHeal = false;
    //left right limit
    public bool isTouchLeft = false;
    public bool isTouchRight = false;



    float MaxSkill2Delay = 30;
    float Skill2Delay = 30;
    bool bSkill2 = true;

    float MaxSkill1Delay = 60;
    float Skill1Delay = 60;
    bool bSkill1 = true;

    [SerializeField]
    Image delayImage1;
    [SerializeField]
    Image delayImage2;

    
    [SerializeField]
    GameObject skill1Effect;
    [SerializeField]
    GameObject skill2Effect;

    public bool bisDead = false;
    void Update()
    {

        if (isSkillMotion)
        {

        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
                Skill1();
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
                Skill2();
            }
        }
        if (ismoving)
        {
            transform.position += MoveSpeed * Time.deltaTime * RightLeft;
        }


        if(inBattle)
        {
            StopCoroutine("CHealPlayer");
            StopCoroutine("CGainHealth");
            StartCoroutine("CHealPlayer");
        }

        if (!bSkill2)
        {

            Skill2Delay -= Time.deltaTime;
            float time = Skill2Delay / MaxSkill2Delay;
            delayImage2.fillAmount = time;
            if (Skill2Delay <= 0f)
            {
                bSkill2 = true;
                Skill2Delay = MaxSkill2Delay;
                skill1Effect.GetComponent<Animation>().Play();
            }
        }
        if (!bSkill1)
        {

            Skill1Delay -= Time.deltaTime;
            float time = Skill1Delay / MaxSkill1Delay;
            delayImage1.fillAmount = time;
            if (Skill1Delay <= 0f)
            {
                bSkill1 = true;
                Skill1Delay = MaxSkill1Delay;
                skill2Effect.GetComponent<Animation>().Play();
            }
        }


    }

    private void FixedUpdate()
    {
        if (Enemy != null && !isSkillMotion)
            CheckEnemy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Limit")
        {
        
            switch (collision.gameObject.name)
            {
                case "Left":
                    MoveSpeed = 0;
                    isTouchLeft = true;
                    break;


                case "Right":
                    MoveSpeed = 0;
                    isTouchRight = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Limit")
        {

            switch (collision.gameObject.name)
            {
                case "Left":
                    isTouchLeft = false;
                    break;


                case "Right":
                    isTouchRight = false;
                    break;
            }
        }
    }

    public override void Spawn()
    {

        SetPlayer();
        CheckTeam();
        healthBar.SetHealthBarColor("Player");
        //get stats from statsUI
        // Armor, Health,Attack, AttackSpeed, MoveSpeed, Accuracy, Avoid
        float armor = PlayerPrefs.GetInt("Armor", 0);
        float health = PlayerPrefs.GetInt("Health", 0);
        float attack = PlayerPrefs.GetInt("AttackDamage", 0);
        float attackSpeed = PlayerPrefs.GetInt("AttackSpeed", 0);
        float moveSpeed = PlayerPrefs.GetInt("MoveSpeed", 0);
        float accuracy = PlayerPrefs.GetInt("Accuracy", 0);
        float avoid = PlayerPrefs.GetInt("Avoid", 0);

        SetCharacterSettings(500 + 500 * health/10, 20 + 20 * attack, 0, 1.4f - (1.4f * attackSpeed/10), 1f, true, true, 1.5f + (1.5f * moveSpeed/10), 200 + 200 * accuracy, 120 + 120 * avoid); //���� 10�ε� �ӽ÷� 200���� �ٲ�


        healthBar.SetHealth(MaxHealth, MaxHealth);
        healthBar.slider.value = float.MaxValue;
    }

    public override void TakeDamage(float damage, float enemyAccuracy = 60, bool pierce = false, string weak = "없음")    
    {
        inBattle = true;
        base.TakeDamage(damage, enemyAccuracy, pierce);
    }


    private IEnumerator Move()
    {
        transform.position += MoveSpeed * Time.deltaTime * RightLeft;
        yield return null;
    }
    public void MoveLeft()
    {
        if (isTouchLeft)
            return;

        if (!ismoving)
        {
            MoveSpeed = 1.0f;
            ismoving = true;
            animator.SetTrigger(DoMove);
        }
        this.GetComponent<SpriteRenderer>().flipX = true;
        RightLeft = Vector3.left;
    }
    public void MoveRight()
    {
        if (isTouchRight)
            return;

        if (!ismoving)
        {
            MoveSpeed = 1.0f;
            ismoving = true;
            animator.SetTrigger(DoMove);
        }
        this.GetComponent<SpriteRenderer>().flipX = false;
        RightLeft = Vector3.right;
    }
    public void StopAndFlip()
    {
        ismoving = false;
        MoveSpeed = 0.0f;
        this.GetComponent<SpriteRenderer>().flipX = false;
        RightLeft = Vector3.right;
        animator.SetTrigger("doStop");
    }


    protected override void CheckEnemy()
    {
        if (ismoving) { return; }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + raycastHeight, RightLeft, AttackRange, LayerMask.GetMask(Enemy));
        //draw the ray in the scene view with distance 
       
        Debug.DrawRay(transform.position + raycastHeight, RightLeft * AttackRange, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(Enemy) && isAttacking == false &&!bBackWard)
            {
                isAttacking = true;
                if (IsMelee)
                {
                    inBattle = true;
                    Attackcoroutine=StartCoroutine(Attack(hit));
                }
            }
        }
    }

    public void Skill1()
    {
        if(!bSkill1)
        { return; }

        inBattle = true;
        this.GetComponent<SpriteRenderer>().flipX = false;
        isSkillMotion = true;
        animator.SetTrigger("doSkill1");
        StartCoroutine(Skill1Motion());
    }
    public void Skill2()
    {
        if (!bSkill2)
        {
            return;
        }


        inBattle = true;
        this.GetComponent<SpriteRenderer>().flipX = false;
        isSkillMotion = true;
        Debug.Log("Skill2");
        animator.SetTrigger("doSkill2");
        StartCoroutine(Skill2Motion());
    }

    protected IEnumerator Skill1Motion()
    {
        bSkill1 = false;
        // check my team is around me
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2f, LayerMask.GetMask("Team"));

        //1hitcollider is this
        if (hitColliders.Length == 0)
        {
            isSkillMotion = false;
            this.GetComponent<BaseCharacter>().PrincessBuff();
            yield break;
        }
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<BaseCharacter>().PrincessBuff();
        }
        yield return new WaitForSeconds(1.0f);
        isSkillMotion = false;
    }
    protected IEnumerator Skill2Motion()
    {
        bSkill2 = false;
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Skill2Fin");
        isSkillMotion = false;
    }
    


    public override IEnumerator RangedAttack(RaycastHit2D hit)
    {


        yield return new WaitForEndOfFrame();
    }

    protected override void RangedAttackShoot()
    {
        GameObject rangedAttack = Instantiate(rangedAttackPrefab, rangedAttackSpawnPoint.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
        rangedAttack.GetComponent<PlayerRangeSkill>().SkillSetting(AttackDammage);
    }

    private IEnumerator CHealPlayer()
    {
        inBattle = false;
        
        yield return new WaitForSeconds(8.0f);
        canHeal = true;
        StartCoroutine("CGainHealth");
    }
    private IEnumerator CGainHealth()
    {
        
        GainHealth(MaxHealth*0.1f);
        yield return new WaitForSeconds(1.0f);
        if(!inBattle)
        {
            StartCoroutine("CGainHealth");
        }
    }


    public override void Die()
    {
        StartCoroutine(DieAnimChar());
    }

    public IEnumerator DieAnimChar()
    {
        animator.SetTrigger(DODie);
        yield return new WaitForSeconds(1.0f);
        Hide();
    }

    public void Hide()
    {
        bisDead = true;
        this.transform.position = new Vector3(-30, 0, 0);
    }

    public void ReSpawn()
    {
        this.GetComponent<BoxCollider2D>().enabled = true;
        Spawn();
        this.transform.position = new Vector3(-9, 0, 0);
        bisDead = false;
    }
}
