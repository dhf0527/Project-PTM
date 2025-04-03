using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public DungeonData[] dungeonDatas;
    public AreaPanel[] areaPanels;
    public DungeonPanel dungeonPanel;

    public TMP_Text soul_Text;

    int cur_Stage_Index;
    int soul;
    public int Soul
    {
        get 
        { 
            return soul; 
        }
        set
        {
            soul = value;
            SetSoul();
            PlayerPrefs.SetInt("Soul", value);
        }

    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Soul = PlayerPrefs.GetInt("Soul");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            Soul += 5000;
        if (Input.GetKeyDown(KeyCode.F2))
            Soul = 0;
    }

    //areaPanel에 던전 데이터 삽입
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

    //dungeonPanel에 던전 데이터 삽입
    public void OnSetDungeonPanel(int number)
    {
        dungeonPanel.SetData(dungeonDatas[cur_Stage_Index * 3 + number]);
    }

    void SetSoul()
    {
        soul_Text.text = Soul.ToString();
    }
}
