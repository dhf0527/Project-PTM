using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLongClick : LongClick
{
    [SerializeField] int unitIndex;

    protected override void LongClickFunc()
    {
        if (!DunGeonManager_New.instance.spawnUnits[unitIndex])
            return;

        DunGeonManager_New.instance.OnPause(true);
        DunGeonManager_New.instance.unitDetailPanel.gameObject.SetActive(true);
        DunGeonManager_New.instance.unitDetailPanel.SetDetail(DunGeonManager_New.instance.spawnUnits[unitIndex]);
    }
}
