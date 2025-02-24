using UnityEngine;

namespace Chracter
{
    public class Golem : BaseCharacter
    {

        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(100, 30, 18, 3.3f, AttackRangeMeleeLong, true, true, MoveDefault, 40, 20);
            healthBar.SetHealth(MaxHealth, MaxHealth);
        }
    }
}
