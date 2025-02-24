using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chracter
{
    public class SkeletonWarrior : BaseCharacter
    {


        [SerializeField] AnimatorOverrideController overrider;
        [SerializeField] Sprite RebornIdle;
        private bool isReborn = false;
    
        // Start is called before the first frame update
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(20, 60, 0, 1.6f, AttackRangeMeleeLong, true, true, MoveDefault, 120, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }



        public override void Die()
        {
            if(!isReborn)
            {
                this.GetComponent<BoxCollider2D>().enabled = true;
                isDead = false;
                animator.runtimeAnimatorController = overrider;
                isReborn = true;
                SetCharacterSettings(200, 60, 0, 1.6f, AttackRangeMeleeLong, true, true, MoveDefault, 120, 40);
                healthBar.SetHealth(MaxHealth, MaxHealth);
                ActiveIcon(3);
                StartCoroutine(TakeDamageSW());
            }
            else
            {
                if (DieSound != null)
                {
                    DieSound.Play();
                }
                StartCoroutine(DieAnim());
            }

        }

        public override void SetCharacterSettings(float HP = 100, float Attack = 10, float armor = 0, float attackSpeed = 1,
            float attackRange = 0.5f, bool isPhysical = true, bool isMelle = true, float moveSpeed = 1.0f, float accuracy = 60, float avoid = 60)
        {
            base.SetCharacterSettings();
            if(isReborn)
            {

                AttackSpeed = (float)10 / (itemData.attackSpeedValue / 2);
                MoveSpeed = itemData.realSpeed * 1.5f;
            }
        }

        private IEnumerator TakeDamageSW()
        {
            Debug.Log("asdfaswdefasdf");

            TakeDamage(MaxHealth/10,500,true);
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(TakeDamageSW());
        }




    }
}
