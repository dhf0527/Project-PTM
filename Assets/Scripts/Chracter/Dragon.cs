using UnityEngine;

namespace Chracter
{
    public class Dragon : BaseCharacter
    {


        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(240, 80, 22, 3.3f, AttackRangeRangedLong, false, false, MoveDefault, 120, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
