using System.Collections;
using UnityEngine;

namespace Chracter
{
    public class Wizard : BaseCharacter
    {


        public override void Spawn()
        {
            base.Spawn();
            SetCharacterSettings(80, 30, 0, 2.5f, AttackRangeRangedLong, false, false, MoveDefault, 60, 60);
            healthBar.SetHealth(MaxHealth, MaxHealth);

        }
    }
}
