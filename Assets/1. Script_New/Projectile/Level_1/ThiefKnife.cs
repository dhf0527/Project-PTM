using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefKnife : Projectile
{
    [SerializeField] int attack_Gold;
    [SerializeField] int kill_Gold;

    protected override void ApplyAttack(Unit target_Unit)
    {
        base.ApplyAttack(target_Unit);

        if (IsTeam)
        {
            //¸íÁß ½Ã 2°ñµå È¹µæ
            DunGeonManager_New.instance.GetGold(attack_Gold);
            Debug.Log("+2°ñµå");
            //Ã³Ä¡ ½Ã 10°ñµå È¹µæ
            if (target_Unit.Cur_Hp <= 0)
            {
                DunGeonManager_New.instance.GetGold(kill_Gold);
                Debug.Log("+10°ñµå");
            }
        }
    }
}
