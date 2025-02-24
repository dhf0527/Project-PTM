using System;
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;

namespace Chracter
{
    public class BaseCharacter : MonoBehaviour
    {
        public GameObject HitAnimGameObject;
        
        
        public ParticleSystem moveParticle;
        protected ClassType UnitType;
        public ParticleSystem buffEffect;
        public float MaxHealth = 100;
        public float CurrentHealth;
        protected float AttackDammage = 10;
        protected float Armor = 0;
        [SerializeField] protected float AttackSpeed = 1;
        protected float AttackRange = 0.5f;
        protected bool IsPhysical = true;
        protected bool IsMelee = true;
        protected int UnitCost;
        public Animator animator;
        [SerializeField] protected float MoveSpeed = 1f;
        protected float Accuracy = 60f;
        protected float Avoid = 60;
        protected bool Pierce = false;
        protected string weakAttribute = "없음";
        protected string Attribute = "없음";

        private bool hitAnimPlaying = false;

        public HealthBar healthBar;
        [SerializeField] protected GameObject rangedAttackPrefab = null;
        [SerializeField] protected Transform rangedAttackSpawnPoint = null;


        protected static readonly int DoMove = Animator.StringToHash("doMove");
        protected static readonly int DoAttack = Animator.StringToHash("doAttack");
        protected static readonly int DoHit = Animator.StringToHash("doHit");
        protected static readonly int DODie = Animator.StringToHash("doDie");

        protected bool isAttacking = false;
        protected bool IsMoving = false;

        protected Vector3 raycastHeight = new Vector3(0, 0.2f, 0);


        protected Vector3 RightLeft;
        protected string Enemy;
        protected string Team;
        protected bool isPlayableCharacter = false;

        protected bool firstHit = false;
        protected bool secondHit = false;

        protected bool isDead = false;

        protected RaycastHit2D currentEnemy;
        protected List<RaycastHit2D> currentEnemys;
        public  List<RaycastHit2D> Enemys = new List<RaycastHit2D>();

        private float debuffDelay = 0.0f;

        //CharacterHealth
        protected static readonly float HealthVeryLow;
        protected static readonly float HealthLow;
        protected static readonly float HealthDefault;
        protected static readonly float HealthHigh;
        protected static readonly float HealthVeryHigh;

        //CharacterHealth
        protected static readonly float AttackVeryLow;
        protected static readonly float AttackLow;
        protected static readonly float AttackDefault;
        protected static readonly float AttackHigh;
        protected static readonly float AttackVeryHigh;


        //CharacterSpeed
        protected static readonly float MoveLow = 0.7f;
        protected static readonly float MoveDefault = 1.0f;
        protected static readonly float MoveFast = 1.2f;

        //CharacterAttackRange Melee
        protected static readonly float AttackRangeMeleeSmall = 0.3f;
        protected static readonly float AttackRangeMeleeDefault = 0.5f;
        protected static readonly float AttackRangeMeleeLong = 1.0f;

        //CharacterAttackRange Ranged
        protected static readonly float AttackRangeRangeSmall = 1.5f;
        protected static readonly float AttackRangeRangedDefault = 2.0f;
        protected static readonly float AttackRangeRangedLong = 3.0f;

        //ChcaracterAttackSpeed
        protected static readonly float AttackSpeedVeryLow = 2.0f;
        protected static readonly float AttackSpeedLow = 0.7f;
        protected static readonly float AttackSpeedDefault = 1.0f;
        protected static readonly float AttackSpeedFast = 1.2f;

        //CharacterAccuracy
        protected static readonly float AccuracyVeryLow = 20;
        protected static readonly float AccuracyLow = 40;
        protected static readonly float AccuracyDefault = 60;
        protected static readonly float AccuracyHigh = 100;
        protected static readonly float AccuracyVeryHigh = 200;


        //CharacterAvoid
        protected static readonly float AvoidVeryLow = 20;
        protected static readonly float AvoidLow = 40;
        protected static readonly float AvoidDefault = 60;

        protected static readonly float AvoidHigh = 80;
        protected static readonly float AvoidVeryHigh = 120;


        //PlayerCharacter Debuff,Buff
        private bool isDeBuff = false;
        private bool batDebuff = false;

        //HealthDebuff
        private bool skullDebuff = false;
        private bool bMagicWarriorDebuff = false;

        protected bool bBackWard = false;

        
        //get Data from ItemData
        public ItemData itemData;
        protected Coroutine Attackcoroutine;


        [SerializeField]
        protected AudioSource AttackSound;
        [SerializeField]
        protected AudioSource HitSound;
        
        [SerializeField]
        protected AudioSource DieSound;
        
        [SerializeField]
        bool Standing = false;


        void Start()
        {
          
            if (moveParticle != null)
            {
                moveParticle.Play();
            }
            if (this.GetComponent<PlayerMove>() == true)
            {
                Spawn();
            }


           
        }

        // Update is called once per frame
        void Update()
        {
            if (Enemy != null && !isDead)
                CheckAnimatorState();

            if(bBackWard)
            {
                StartCoroutine(BackWard());
            }
        }

        private void FixedUpdate()
        {
            if (Enemy != null && !isDead)
                CheckEnemy();
        }


        public virtual void SetCharacterSettings(float HP = 100, float Attack = 10, float armor = 0, float attackSpeed = 1,
            float attackRange = 0.5f, bool isPhysical = true, bool isMelle = true, float moveSpeed = 1.0f, float accuracy = 60, float avoid = 60)
        {
            AttackRange = attackRange;
            IsPhysical = isPhysical;
            IsMelee = isMelle;
            MaxHealth = itemData.healthValue;
            CurrentHealth = itemData.healthValue;
            AttackDammage = itemData.strengthValue;
            Armor = itemData.defenseValue;
            
            
            AttackRange = itemData.attackRange;
            AttackSpeed = (float)10/itemData.attackSpeedValue;
            MoveSpeed = itemData.realSpeed;
            Accuracy = itemData.accuracyValue;
            Avoid = itemData.avoidanceValue;
            if (itemData.Attribute == "물리")
            {
                Attribute = "Physical";
            }
            else
            {
                Attribute = "Magic";
            }
            if (itemData.weakAttribute == "물리")
            {
                weakAttribute = "Physical";
            }
            else if (itemData.weakAttribute == "마법")
            {
                weakAttribute = "Magic";
            }


        }


        protected virtual void CheckEnemy()
        {
            
            int MaxAttackCount = itemData.attackCountLimitValue;
           
            if(MaxAttackCount<1) { MaxAttackCount = 1; }

            if (MaxAttackCount > 1)
            {
                Enemys.Clear();
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + raycastHeight, RightLeft, AttackRange, LayerMask.GetMask(Enemy));
                Debug.DrawRay(transform.position + raycastHeight, RightLeft * AttackRange, Color.blue);

                if (hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        //max attack count
                        if(i==MaxAttackCount)
                        {
                            break;
                        }
                        if (hits[i].collider.CompareTag(Enemy))
                        {
                            Enemys.Add(hits[i]);
                        }
                    }
                    if (Enemys.Count > 0 && isAttacking == false&& !bBackWard)
                    {
                        isAttacking = true;
                        IsMoving = false;
                        if (IsMelee)
                        {
                            Attackcoroutine = StartCoroutine(Attacks(Enemys));

                        }
                        else
                        {
                            Attackcoroutine= StartCoroutine(RangedAttack(Enemys[0]));
                        }
                    }
                }
                else if (isAttacking == false && IsMoving == false)
                {
                    IsMoving = true;
                    animator.SetTrigger(DoMove);
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + raycastHeight, RightLeft, AttackRange, LayerMask.GetMask(Enemy));
                Debug.DrawRay(transform.position + raycastHeight, RightLeft * AttackRange, Color.red);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag(Enemy) && isAttacking == false &&!bBackWard)
                    {
                        isAttacking = true;
                        IsMoving = false;
                        if (IsMelee)
                        {
                            Attackcoroutine=StartCoroutine(Attack(hit));
                        }
                        else
                        {
                            Attackcoroutine=StartCoroutine(RangedAttack(hit));
                        }
                    }
                }

                else if (isAttacking == false && IsMoving == false)
                {

                    IsMoving = true;
                    animator.SetTrigger(DoMove);
                }
            }
        }

        private void CheckAnimatorState()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            {
                StartCoroutine(MoveCharacter());
            }
        }

        private IEnumerator MoveCharacter()
        {
            transform.position += MoveSpeed * Time.deltaTime * RightLeft;
            yield return null;
        }

        private IEnumerator BackWard()
        {
            transform.position -= 1.5f * Time.deltaTime * RightLeft;
            yield return null;
        }




        // virtual method for attack
       
        protected virtual IEnumerator Attack(RaycastHit2D hit)
        {
           
            currentEnemy = hit;
            Debug.Log("StartAttack");
            animator.SetTrigger(DoAttack);
            yield return new WaitForSeconds(AttackSpeed);
            yield return new WaitForSeconds(debuffDelay);
            isAttacking = false;
            Debug.Log("AttackEnd");
        }

        protected virtual IEnumerator Attacks(List<RaycastHit2D> Enemys)
        {
          
            currentEnemys = Enemys;

            animator.SetTrigger(DoAttack);
            yield return new WaitForSeconds(AttackSpeed);
            yield return new WaitForSeconds(debuffDelay);
            isAttacking = false;
        }

        public virtual void AttackHIt()
        {
            if (currentEnemy)
            {
                currentEnemy.collider.GetComponent<BaseCharacter>().TakeDamage(AttackDammage, Accuracy, Pierce, Attribute);
                if(AttackSound)
                AttackSound.Play();
            }

            if (currentEnemys == null) return;
            if (AttackSound)
                AttackSound.Play();
            foreach (var t in currentEnemys)
            {
                t.collider.GetComponent<BaseCharacter>().TakeDamage(AttackDammage, Accuracy, Pierce, Attribute);
            }
        }

        public virtual IEnumerator RangedAttack(RaycastHit2D hit)
        {
            animator.SetTrigger(DoAttack);
            currentEnemy = hit;
            yield return new WaitForSeconds(AttackSpeed);
            yield return new WaitForSeconds(debuffDelay);
            isAttacking = false;
        }
        public virtual IEnumerator RangedAttacks(List<RaycastHit2D> hit)
        {
            animator.SetTrigger(DoAttack);
            currentEnemys = hit;
            yield return new WaitForSeconds(AttackSpeed);
            yield return new WaitForSeconds(debuffDelay);
            isAttacking = false;
        }

        protected virtual void RangedAttackShoot()
        {
            if (currentEnemy)
            {
                if(AttackSound)
                AttackSound.Play();
                GameObject rangedAttack = Instantiate(rangedAttackPrefab, rangedAttackSpawnPoint.position, Quaternion.identity);
                rangedAttack.GetComponent<RangedAttack>().EnemySetting(currentEnemy, Enemy, AttackDammage, AttackRange, Accuracy,itemData.attackCountLimitValue);
            }
        }



        public virtual void TakeDamage(float damage, float enemyAccuracy = 60, bool pierce = false, string Attribute="none" )
        {
            if(weakAttribute == Attribute)
            {
                damage = damage * 1.5f;
            }
            
            float HitPercent = enemyAccuracy - Avoid + 50;
            if (HitPercent >= 100)
            {
                HitPercent = 100;
            }
            else if (HitPercent <= 5)
            {
                HitPercent = 5;
            }
            int HitCalculate = UnityEngine.Random.Range(0, 100);
            if (HitCalculate > HitPercent)
            {
                return;
            }

            Animator hitanim =HitAnimGameObject.GetComponent<Animator>();
            if (HitSound != null)
            {
                HitSound.Play();                
            }
            hitanim.SetTrigger("Hit");
            
            if (pierce)
            {
                float finalDamage = damage;
                if (finalDamage <= 0)
                {
                    finalDamage = 1;
                }
                CurrentHealth -= finalDamage;
                if (healthBar != null)
                {
                    healthBar.SetHealth(CurrentHealth, MaxHealth);
                }

            }
            else
            {
                float finalDamage = damage - Armor;
                if (finalDamage <= 0)
                {
                    finalDamage = 1;
                }
                CurrentHealth -= finalDamage;
                if (healthBar != null)
                {
                    healthBar.SetHealth(CurrentHealth, MaxHealth);
                }
            }

            if (CurrentHealth <= 0)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
                isDead = true;
                Die();
            }
            else if (CurrentHealth <= MaxHealth * 0.6f && firstHit == false && CurrentHealth>MaxHealth*0.3f)
            {
                if (Standing)
                {
                    return;
                }
                
                animator.ResetTrigger(1);
               
                isAttacking = true;
                if(Attackcoroutine!=null)
                {
                    Debug.Log("StopCoroutine");
                    StopCoroutine(Attackcoroutine);
                }
                
                firstHit = true;
                Hit();
            }
            else if (CurrentHealth <= MaxHealth * 0.3f && secondHit == false)
            {
                if (Standing)
                {
                    return;
                }
                animator.ResetTrigger(1);
                isAttacking = true;
                if (Attackcoroutine != null)
                {
                    StopCoroutine(Attackcoroutine);
                }
                firstHit = true;
                secondHit = true;
                Hit();
            }
        }

        public virtual void TakeRangedDamage(float damage, float enemyAccuracy = 60, bool pierce = false)
        {
            float HitPercent = enemyAccuracy - Avoid + 50;
            if (HitPercent >= 100)
            {
                HitPercent = 100;
            }
            else if (HitPercent <= 5)
            {
                HitPercent = 5;
            }
            int HitCalculate = UnityEngine.Random.Range(0, 100);
            if (HitCalculate > HitPercent)
            {
                return;
            }
            if (pierce)
            {
                float finalDamage = damage;
                if (finalDamage <= 0)
                {
                    finalDamage = 1;
                }
                CurrentHealth -= finalDamage;
                if (healthBar != null)
                {
                    healthBar.SetHealth(CurrentHealth, MaxHealth);
                }

            }
            else
            {
                float finalDamage = damage - Armor;
                if (finalDamage <= 0)
                {
                    finalDamage = 1;
                }
                CurrentHealth -= finalDamage;
                if (healthBar != null)
                {
                    healthBar.SetHealth(CurrentHealth, MaxHealth);
                }
            }

            if (CurrentHealth <= 0)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
                isDead = true;
                Die();
            }
            else if (CurrentHealth <= MaxHealth * 0.6f && firstHit == false)
            {
                if (Standing)
                {
                    return;
                }
                isAttacking = true;
                firstHit = true;
                Hit();
            }
            else if (CurrentHealth <= MaxHealth * 0.3f && secondHit == false)
            {
                if (Standing)
                {
                    return;
                }
                isAttacking = true;
                secondHit = true;
                Hit();
            }
        }

        public virtual void GainHealth(float heal)
        {
            if(skullDebuff)
            {
                heal = heal / 2;
            }

            if(CurrentHealth+heal>=MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            else
            {
                CurrentHealth = CurrentHealth + heal;
            }
            healthBar.SetHealth(CurrentHealth, MaxHealth);
        }


        public virtual void Die()
        {
            if (DieSound != null)
            {
                DieSound.Play();
            }
            StartCoroutine(DieAnim());
        }

        public virtual void Hit()
        {
            StartCoroutine(HitAnim());
            bBackWard = true;
        }

        public IEnumerator DieAnim()
        {
            animator.SetTrigger(DODie);
            yield return new WaitForSeconds(1.0f);
            Destroy(gameObject);
        }
        public IEnumerator HitAnim()
        {
           
            animator.SetTrigger(DoHit);
            yield return new WaitForSeconds(0.5f);
            IsMoving = false;
            isAttacking = false;
            bBackWard = false;

        }



        public virtual void Spawn()
        {
            CheckTeam();
            healthBar.SetHealthBarColor(this.tag);
        }


        public void CheckTeam()
        {
            if (CompareTag("Enemy"))
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
                RightLeft = Vector3.left;
                Enemy = "Team";
                Team = "Enemy";
            }
            else
            {
                RightLeft = Vector3.right;
                Enemy = "Enemy";
                Team = "Team";
            }

            animator.SetTrigger(DoMove);
        }

        protected void SetPlayer()
        {
            isPlayableCharacter = true;
        }

        public void PrincessBuff()
        {
            StartCoroutine(BuffParticle());
            PierceAttack(6);
        }

        private IEnumerator BuffParticle()
        {
            buffEffect.Play();
            yield return new WaitForSeconds(6.0f);
            buffEffect.Stop();

        }

        internal void PrincessDebuff()
        {
            if (isDeBuff)
            {
                return;
            }
            isDeBuff = true;
            StartCoroutine(DebuffCor());
        }

        private IEnumerator DebuffCor()
        {
            float moveSpeedOrigin = MoveSpeed;
            float attackSpeedOrigin = AttackSpeed;

            MoveSpeed *= 0.2f;
            debuffDelay = AttackSpeed * 0.8f;
            ActiveIcon(1);
            yield return new WaitForSeconds(4.0f);
            MoveSpeed = moveSpeedOrigin;
            AttackSpeed = attackSpeedOrigin;
            debuffDelay = 0.0f;
            isDeBuff = false;
            DeactiveIcon(1);
        }

        public void PierceAttack(float time)
        {

            StartCoroutine(PierceCor(time));
        }


        private IEnumerator PierceCor(float time)
        {
            Pierce = true;
            ActiveIcon(0);
            yield return new WaitForSeconds(time);
            Pierce = false;
            DeactiveIcon(0);
        }

        public void ActiveIcon(int i)
        {
            healthBar.ActiveBuff(i);
        }

        public void DeactiveIcon(int i)
        {
            healthBar.DeActiveBuff(i);
        }

        public void ChangeBossStats(float Health, float Attack, float Movespeed)
        {
        }

        public void ReaperBuff()
        {
            StartCoroutine("CReaperBuff");
        }
        private IEnumerator CReaperBuff()
        {
            ActiveIcon(5);
            AttackDammage = AttackDammage + 20;
            Accuracy = Accuracy + 20;
            yield return new WaitForSeconds(6.0f);
            AttackDammage = AttackDammage - 20;
            Accuracy = Accuracy - 20;
            DeactiveIcon(5);
        }

        public void BatDebuff()
        {
            if (batDebuff)
            {
                StopCoroutine("CBatDebuff");
                Avoid = Avoid + 20;
                Accuracy = Accuracy + 20;
                StartCoroutine("CBatDebuff");

            }
            else
            {
                StartCoroutine("CBatDebuff");
            }
            //corourtine
        }
        private IEnumerator CBatDebuff()
        {
            healthBar.ActiveBuff(6);
            batDebuff = true;
            Avoid = Avoid - 20;
            Accuracy = Accuracy - 20;
            yield return new WaitForSeconds(4.0f);
            Avoid = Avoid + 20;
            Accuracy = Accuracy + 20;
            batDebuff = false;
            healthBar.DeActiveBuff(6);

        }

        Coroutine dotCoroutine;
        public void SkullDebuff()
        {
            if (skullDebuff)
            {
                StopCoroutine("CSkullDebuff");
                StartCoroutine("CSkullDebuff");

            }
            else
            {
                StartCoroutine("CSkullDebuff");
            }
            //corourtine
        }
        private IEnumerator CSkullDebuff()
        {
            ActiveIcon(4);
            skullDebuff = true;
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            yield return new WaitForSeconds(0.5f);
            TakeDamageDot();
            skullDebuff = false;
            DeactiveIcon(4);

        }

        public virtual void TakeDamageSkull(float damage)
        {
            SkullDebuff();
        }

        public void TakeDamageDot()
        {
            float finalDamage = 2;
            CurrentHealth -= finalDamage;
            if (healthBar != null)
            {
                healthBar.SetHealth(CurrentHealth, MaxHealth);
            }
            if (CurrentHealth <= 0)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
                isDead = true;
                Die();
            }
        }
        
        

        public void MagicWarriorDebuff()
        {
            ActiveIcon(2);
            if (bMagicWarriorDebuff)
            {
                return;
            }
            else
            {
                Armor = Armor / 2;
                bMagicWarriorDebuff = true;
            }

        }


    }
}
