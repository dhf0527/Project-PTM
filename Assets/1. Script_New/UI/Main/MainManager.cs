using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public DungeonData[] dungeonDatas;
    public AreaPanel[] areaPanels;
    public DungeonPanel dungeonPanel;

    int cur_Stage_Index;

    public void OnSetDungeonDatas(int stage)
    {
        //스테이지는 1부터 시작하므로 인덱스로 변환
        stage -= 1;
        cur_Stage_Index = stage;

        for (int i = 0; i < 3; i++)
        {
            areaPanels[i].SetData(dungeonDatas[stage * 3 + i]);
        }
    }

    public void OnSetDungeonPanel(int number)
    {
        dungeonPanel.SetData(dungeonDatas[cur_Stage_Index * 3 + number]);
    }
}
