using UnityEngine;

namespace Chracter
{
    public class Reaper : BaseCharacter
    {
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(200, 60, 0, 1.6f, AttackRangeMeleeLong, true, true, MoveDefault, 120, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }





        protected override void CheckEnemy()
        {
            int MaxAttackCount = itemData.attackCountLimitValue;

            if (MaxAttackCount < 1) { MaxAttackCount = 1; }

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
                        if (i == MaxAttackCount)
                        {
                            break;
                        }
                        if (hits[i].collider.CompareTag(Enemy))
                        {
                            Enemys.Add(hits[i]);
                        }
                    }
                    if (Enemys.Count > 0 && isAttacking == false && !bBackWard)
                    {
                        isAttacking = true;
                        IsMoving = false;
                        if (IsMelee)
                        {
                            foreach (RaycastHit2D Enemy in Enemys)
                            {
                                float current = Enemy.collider.GetComponent<BaseCharacter>().CurrentHealth;
                                float Max= Enemy.collider.GetComponent<BaseCharacter>().MaxHealth;
                                if(current<=Max/3)
                                {
                                    ReaperBuff();
                                    Enemy.collider.GetComponent<BaseCharacter>().Die();
                                }
                            }


                            Attackcoroutine = StartCoroutine(Attacks(Enemys));

                        }
                        else
                        {
                            Attackcoroutine = StartCoroutine(RangedAttack(Enemys[0]));
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
                    if (hit.collider.CompareTag(Enemy) && isAttacking == false && !bBackWard)
                    {
                        isAttacking = true;
                        IsMoving = false;
                        if (IsMelee)
                        {
                            Attackcoroutine = StartCoroutine(Attack(hit));
                        }
                        else
                        {
                            Attackcoroutine = StartCoroutine(RangedAttack(hit));
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
    }
}
