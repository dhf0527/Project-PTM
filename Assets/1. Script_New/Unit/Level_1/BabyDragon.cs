using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragon : Unit
{
    [Header("1,2,3웨이브 순")]
    public List<int> attackByWave;

    public override void Init()
    {
        base.Init();
        ExplosiveGrowth explosiveGrowth = gameObject.AddComponent<ExplosiveGrowth>();
        explosiveGrowth.buffValues = attackByWave;
        explosiveGrowth.buff_Time = int.MaxValue;
    }
}
