using UnityEngine;

namespace Chracter
{
    public class MinoTaur : BaseCharacter
    {
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(100, 30, 6, 2.5f, AttackRangeMeleeLong, true, true, MoveDefault, 60, 20);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
