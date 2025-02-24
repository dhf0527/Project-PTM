using UnityEngine;

namespace Chracter
{
    public class Paladin : BaseCharacter
    {

        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(240, 60, 14, 2.5f, AttackRangeMeleeDefault, false, true, MoveDefault, 120, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
