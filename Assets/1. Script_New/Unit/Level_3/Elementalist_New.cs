using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elementalist_New : Unit
{
    float healAmount = 10;

    public void Heal()
    {
        float range = 1f;

        //스캔할 레이어 설정
        string target_Layer = IsTeam ? TeamLayer : EnemyLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.left * range, Vector2.right, range * 2, LayerMask.GetMask(target_Layer));

        //주위에 아무도 없으면 자신을 치유
        Unit healTarget_Unit = this;

        //생명력 비율이 가장 낮은 아군 찾기
        foreach (var item in hits)
        {
            if (item.collider.GetComponent<TeamBase_Unit>() || item.collider.GetComponent<EnemyBase_Unit>())
                continue;

            Unit target_Unit = item.collider.GetComponent<Unit>();

            if (target_Unit.Cur_Hp / target_Unit.Max_Hp < healTarget_Unit.Cur_Hp / healTarget_Unit.Max_Hp)
                healTarget_Unit = target_Unit;
        }

        healTarget_Unit.Cur_Hp += healAmount;
        healTarget_Unit.GetHp(healAmount);
    }
}
