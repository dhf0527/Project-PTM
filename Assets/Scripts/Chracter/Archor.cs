using System.Collections;
using UnityEngine;

namespace Chracter
{
    public class Archor : BaseCharacter
    {
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(20, 12, 0, 2.0f, AttackRangeRangedDefault, true, false, 1.5f, 200, 120);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
