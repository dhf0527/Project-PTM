using UnityEngine;

namespace Chracter
{
    public class Slime : BaseCharacter
    {


        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(20, 8, 0, 2.0f, AttackRangeMeleeDefault, false, true, 1f, 60, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
