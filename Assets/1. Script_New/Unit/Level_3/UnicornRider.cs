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
        Blinded blindedDebuff = target_Unit.AddComponent<Blinded>();
        blindedDebuff.buff_Time = buff_Time;
        blindedDebuff.accuracy_Decrease = accuracy_Decrease;
    }
}
