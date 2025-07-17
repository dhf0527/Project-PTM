using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireSkull_New : Unit
{
    public float buff_Time;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        base.ApplyAttack(target_Unit, damage, ud.attack_Type);
        target_Unit.AddComponent<CursedFlame>().buff_Time = buff_Time;
    }
}
