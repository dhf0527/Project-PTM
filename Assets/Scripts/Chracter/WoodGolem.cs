using UnityEngine;

namespace Chracter
{
    public class WoodGolem : BaseCharacter
    {

        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(240, 80, 22, 3.3f, AttackRangeMeleeDefault, true, true, MoveLow, 60, 20);
            healthBar.SetHealth(MaxHealth, MaxHealth);

        }
    }
}
