using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat_New : Unit
{
    public float buff_Time;
    public int armor_Decrease_percent;
    public int avoidance_Decrease;


    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        base.ApplyAttack(target_Unit, damage, attackType);
        EarringDebuff earringDebuff = target_Unit.AddComponent<EarringDebuff>();
        earringDebuff.buff_Time = buff_Time;
        earringDebuff.accuracy_Decrease = armor_Decrease_percent;
        earringDebuff.avoidance_Decrease = avoidance_Decrease;
    }
}
