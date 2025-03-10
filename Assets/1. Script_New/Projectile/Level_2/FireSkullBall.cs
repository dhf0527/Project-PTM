using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireSkullBall : Projectile
{
    protected override void ApplyAttack(Unit target_Unit)
    {
        base.ApplyAttack(target_Unit);
        target_Unit.AddComponent<CursedFlame>();
    }

}
