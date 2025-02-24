using UnityEngine;

namespace Chracter
{
    public class DeerWarrior : BaseCharacter
    {
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(200, 60, 0, 1.6f, AttackRangeMeleeLong, true, true, MoveDefault, 120, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
