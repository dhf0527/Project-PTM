using UnityEngine;

namespace Chracter
{
    public class Goblin : BaseCharacter
    {

        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(30, 8, 0, 2.0f, AttackRangeMeleeDefault, true, true, 1f, 40, 60);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }



    }
}
