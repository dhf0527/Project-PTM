using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paladin_New : Unit
{
    private void Update()
    {
        base.Update();

        float skillRange = 1f;

        //스캔할 레이어 설정
        string target_Layer = IsTeam ? TeamLayer : EnemyLayer;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.left * skillRange, Vector2.right, skillRange * 2, LayerMask.GetMask(target_Layer));

        foreach (var item in hits)
        {
            if (item.collider.GetComponent<TeamBase_Unit>() || item.collider.GetComponent<EnemyBase_Unit>())
                continue;

            Unit target_Unit = item.collider.GetComponent<Unit>();
            target_Unit.AddComponent<Guard>();
            //버프 처리
        }
    }
}
