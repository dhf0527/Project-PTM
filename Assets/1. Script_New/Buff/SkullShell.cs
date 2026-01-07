using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullShell : Buff
{
    [HideInInspector] public int hpCut1;
    [HideInInspector] public int hpCut2;
    [HideInInspector] public List<int> buffValues;
    Unit skullGiantUnit;
    int pre_buffValue = 0;

    protected override void Init()
    {
        buffIcon_Index = -1;
        skullGiantUnit = GetComponent<Unit>();
    }

    private void Update()
    {
        base.Update();

        int buffValue;
        if (skullGiantUnit.Cur_Hp < skullGiantUnit.Max_Hp * hpCut1 * 0.01f)
            buffValue = buffValues[0];
        else if (skullGiantUnit.Cur_Hp < skullGiantUnit.Max_Hp * hpCut2 * 0.01f)
            buffValue = buffValues[1];
        else
            buffValue = buffValues[2];

        PlusArmor(buffValue);
    }

    protected override bool PreventStack()
    {
        if (GetComponents<SkullShell>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<SkullShell>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    void PlusArmor(int value)
    {
        if (pre_buffValue == value)
            return;

        //기존 버프량만큼 감소(버프 초기화)
        skullGiantUnit.unitStatData_st.armor_Plus -= pre_buffValue;

        //새로운 버프량만큼 증가
        skullGiantUnit.unitStatData_st.armor_Plus += value;
        //버프량 기록
        pre_buffValue = value;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
    }

    public override void BuffEnd()
    {
        base.BuffEnd();
    }
}
