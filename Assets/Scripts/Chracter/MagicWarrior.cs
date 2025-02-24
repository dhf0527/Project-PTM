using UnityEngine;

namespace Chracter
{
    public class MagicWarrior : BaseCharacter
    {

        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(80, 20, 6, 2.0f, AttackRangeMeleeDefault, false, true, MoveDefault, 60, 60);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }


        public override void AttackHIt()
        {
            if (currentEnemy)
            {

                currentEnemy.collider.GetComponent<BaseCharacter>().TakeDamage(AttackDammage, Accuracy, Pierce, Attribute);
                currentEnemy.collider.GetComponent<BaseCharacter>().MagicWarriorDebuff();
            }

            if (currentEnemys == null) return;
            foreach (var t in currentEnemys)
            {
                t.collider.GetComponent<BaseCharacter>().TakeDamage(AttackDammage, Accuracy, Pierce, Attribute);
                t.collider.GetComponent<BaseCharacter>().MagicWarriorDebuff();
            }
        }
    }
}
