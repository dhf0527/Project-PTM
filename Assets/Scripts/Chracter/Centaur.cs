using UnityEngine;

namespace Chracter
{
    public class Centaur : BaseCharacter
    {
        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(80, 20, 0, 2.0f, AttackRangeMeleeLong, true, true, MoveDefault, 60, 40);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
