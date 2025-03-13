using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementalProjectile : Projectile
{
    Unit elementalist_Unit;

    //unit에게 데이터를 받아오는 함수
    public override void SetData(Unit unit)
    {
        base.SetData(unit);
        elementalist_Unit = unit;
    }

    protected override void ApplyAttack(Unit target_Unit)
    {
        base.ApplyAttack(target_Unit);
        elementalist_Unit.GetComponent<Elementalist_New>().Heal();
    }

}
