using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief_New : Unit
{
    [SerializeField] int attack_Gold = 2;
    [SerializeField] int kill_Gold = 10;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        base.ApplyAttack(target_Unit, damage, ud.attack_Type);

        if (IsTeam)
        {
            //¸íÁß ½Ã 2°ñµå È¹µæ
            DunGeonManager_New.instance.GetGold(attack_Gold);
            //Ã³Ä¡ ½Ã 10°ñµå È¹µæ
            if (target_Unit.Cur_Hp <= 0)
            {
                DunGeonManager_New.instance.GetGold(kill_Gold);
            }
        }
    }
}
