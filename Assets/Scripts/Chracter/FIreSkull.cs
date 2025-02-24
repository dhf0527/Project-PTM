using UnityEngine;

namespace Chracter
{
    public class FireSkull : BaseCharacter
    {
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(30, 10, 0, 1.4f, AttackRangeRangedDefault, false, false, 1.5f, 60, 80);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
