using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireSkull_New : Unit
{
    public float buff_Time;
    public float buff_Damage;
    public float buff_Heal_Decrease;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        base.ApplyAttack(target_Unit, damage, ud.attack_Type);
        CursedFlame cursedFlame = target_Unit.AddComponent<CursedFlame>();
        cursedFlame.buff_Time = buff_Time;
        cursedFlame.damage = buff_Damage;
        cursedFlame.heal_Decrease = buff_Heal_Decrease;
    }
}
