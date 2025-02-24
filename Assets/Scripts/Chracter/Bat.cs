using UnityEngine;

namespace Chracter
{
    public class Bat : BaseCharacter
    {
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(200, 60, 0, 1.6f, AttackRangeMeleeLong, true, true, MoveDefault, 120, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }


        public override void TakeDamage(float damage, float enemyAccuracy = 60, bool pierce = false, string weak="없음")
        {
            float HitPercent = enemyAccuracy - Avoid*2 + 50;
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
                firstHit = true;
                Hit();
            }
            else if (CurrentHealth <= MaxHealth * 0.3f && secondHit == false)
            {
                secondHit = true;
                Hit();
            }
        }


        public override void AttackHIt()
        {
            if (currentEnemy)
            {
            
                currentEnemy.collider.GetComponent<BaseCharacter>().TakeDamage(AttackDammage, Accuracy, Pierce, Attribute);
                currentEnemy.collider.GetComponent<BaseCharacter>().BatDebuff();
            }

            if (currentEnemys == null) return;
            foreach (var t in currentEnemys)
            {
                t.collider.GetComponent<BaseCharacter>().TakeDamage(AttackDammage, Accuracy, Pierce, Attribute);
                t.collider.GetComponent<BaseCharacter>().BatDebuff();
            }
        }
    }
}
