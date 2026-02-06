using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnicornRider : Unit
{
    [SerializeField] float buff_Time = 6f;
    [SerializeField] int accuracy_Decrease = 30;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        base.ApplyAttack(target_Unit, damage, attackType);
        Blinded earringDebuff = target_Unit.AddComponent<Blinded>();
        earringDebuff.buff_Time = buff_Time;
        earringDebuff.accuracy_Decrease = accuracy_Decrease;
    }
}
