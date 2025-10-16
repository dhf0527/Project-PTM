using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLongClick : LongClick
{
    protected override void LongClickFunc()
    {
        DunGeonManager_New.instance.OnPause(true);
        DunGeonManager_New.instance.skillDetail_go.SetActive(true);
        DunGeonManager_New.instance.skillDataDetail_1.SetSkillDataDetail(DunGeonManager_New.instance.princess.skillDatas[0], 1);
        DunGeonManager_New.instance.skillDataDetail_2.SetSkillDataDetail(DunGeonManager_New.instance.princess.skillDatas[1], 2);
    }
}
