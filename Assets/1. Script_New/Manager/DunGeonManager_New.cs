using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DunGeonManager_New : MonoBehaviour
{
    //경계선 오브젝트
    [SerializeField] BoxCollider2D boundary;
    //경계선 양끝 좌표
    [HideInInspector] public float boundary_Min_x;
    [HideInInspector] public float boundary_Max_x;

    #region UI 변수
    [Header("UI 변수")]
    //유닛 생산 버튼
    [SerializeField] UnitSpawnButton[] unitSpawnButton = new UnitSpawnButton[3];
    //생산할 유닛(임시)
    [HideInInspector] public Unit[] spawnUnits;

    //유닛 해금 패널
    public UnitUnlock unitUnlock;

    //골드 패널
    public GoldPanel goldPanel;
    //요새 업그레이드 패널
    public BaseLevelUpPanel baseLevelUpPanel;

    public GameObject fasty_Mask;

    //카메라
    public CameraMove cameraMove;
    //공주 체력 패널
    public PrincessHpPanel princessHpPanel;
    #endregion
    #region 유닛 생산 변수
    [Header("유닛 생산 변수")]
    //유닛을 생산할 위치
    [SerializeField] Transform spawn_Trans;
    //유닛(팀)의 부모
    [SerializeField] Transform unit_Parent;
    [Header("유닛간 최소 Y축 차이")]
    public float spawn_Y = 0.03f;

    public List<Unit> units_Level_1;
    #endregion
    #region 골드 변수
    float max_Gold;
    public float Max_Gold
    {
        get { return max_Gold; }
        set
        {
            max_Gold = value;
            goldPanel.SetGoldText();
        }
    }
    float cur_Gold;
    public float Cur_Gold
    {
        get { return cur_Gold; }
        set
        {
            cur_Gold = value;
            goldPanel.SetGoldText();
        }
    }
    float gold_Per_Sec;
    public float Gold_Per_Sec
    {
        get { return gold_Per_Sec; }
        set
        {
            gold_Per_Sec = value;
            goldPanel.SetGoldText();
        }
    }
    float gold_time;
    int base_UpgradeCost;
    #endregion
    #region 요새 레벨업 변수
    [Serializable]
    public class AbillitiesByLevel
    {
        public int base_Hp_By_Level;
        public int base_Armor_By_Level;
        public int base_UpgradeCost_By_Level;
        public int base_GoldPerSec_By_Level;
        public int base_MaxGold_By_Level;
    }
    [Header("아군 요새의 (Element + 1)레벨 능력치")]
    public List<AbillitiesByLevel> base_abillitiesByLevels;
    #endregion
    #region 디버깅
    [Header("(테스트용)고용할 유닛들")]
    [SerializeField] Unit[] test_Units = new Unit[3];
    #endregion

    //투사체 부모
    public Transform projectile_Parent;

    [HideInInspector] public Princess princess;
    [HideInInspector] public TeamBase_Unit teamBase;

    bool isFasty;

    //싱글톤
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;

        //공주 찾아서 저장
        princess = FindAnyObjectByType<Princess>();
        teamBase = FindAnyObjectByType<TeamBase_Unit>();

        Gold_Per_Sec = base_abillitiesByLevels[0].base_GoldPerSec_By_Level;
        Max_Gold = base_abillitiesByLevels[0].base_MaxGold_By_Level;
        base_UpgradeCost = base_abillitiesByLevels[0].base_UpgradeCost_By_Level;
        spawnUnits = new Unit[3];

        //해금 유닛 데이터 설정
        SetUnlockData();

        //유닛 해금 창 열기
        unitUnlock.OpenUnitUnlock(true);
    }

    private void Start()
    {
        //경계선 양끝 좌표 가져오기
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }

    private void Update()
    {
        //0.1초마다 골드 획득
        GetGoldPerSec();
    }

    #region 유닛 생산 함수(골드 포함)
    //생산 버튼을 눌렀을 때 호출될 함수
    public void OnSpawnUnit(int index)
    {
        if (!unitSpawnButton[index].isCoolDown && Cur_Gold >= spawnUnits[index].ud.cost)
        {
            StartCoroutine(C_SpawnUnit(index));
            Cur_Gold -= spawnUnits[index].ud.cost;
            unitSpawnButton[index].SetCoolDown();
        }
    }

    //실제로 생산을 하는 함수
    IEnumerator C_SpawnUnit(int index)
    {
        //생산 수만큼 반복
        for (int i = 0; i < spawnUnits[index].ud.spawn_Count; i++)
        {
            //유닛 하나 생성 및 설정
            Unit unit = Instantiate(spawnUnits[index], spawn_Trans);
            unit.transform.position += SpawnY();
            unit.transform.parent = unit_Parent;
            unit.IsTeam = true;
            //생산 딜레이
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public void SetUnitSpawnButton()
    {
        //버튼과 유닛 연동
        for (int i = 0; i < spawnUnits.Length; i++)
        {
            unitSpawnButton[i].unit = spawnUnits[i];
            unitSpawnButton[i].SetUI();
        }
    }

    //유닛이 생산될 Y축 벡터를 반환하는 함수
    public Vector3 SpawnY()
    {
        int rand = UnityEngine.Random.Range(-5, 6);
        Vector3 return_Vec = Vector3.up * rand * spawn_Y + Vector3.forward * rand * spawn_Y;
        return return_Vec;
    }

    //요새 레벨업 버튼을 눌렀을 때 호출
    public void OnBaseLevelUp()
    {
        //최대 레벨
        if (teamBase.Base_level >= base_abillitiesByLevels.Count)
        {
            return;
        }

        if (Cur_Gold >= base_UpgradeCost)
        {
            Cur_Gold -= base_UpgradeCost;
            //요새 레벨업 처리
            teamBase.Base_LevelUp();
            //골드 관련 레벨업 처리
            Set_GoldByBaseLevel();
        }
    }

    //아군 요새 레벨에 따라 골드 관련 변수 설정
    public void Set_GoldByBaseLevel()
    {
        Gold_Per_Sec = base_abillitiesByLevels[teamBase.Base_level - 1].base_GoldPerSec_By_Level;
        Max_Gold = base_abillitiesByLevels[teamBase.Base_level - 1].base_MaxGold_By_Level;
        base_UpgradeCost = base_abillitiesByLevels[teamBase.Base_level - 1].base_UpgradeCost_By_Level;
    }

    //0.1초마다 골드를 획득하는 함수
    void GetGoldPerSec()
    {
        gold_time += Time.deltaTime;
        if (gold_time >= 0.1f)
        {
            GetGold(Gold_Per_Sec / 10f);
            gold_time -= 0.1f;
        }
    }

    //골드를 획득하는 함수
    public void GetGold(float getGold)
    {
        Cur_Gold += getGold;
        if (Cur_Gold > Max_Gold)
            Cur_Gold = Max_Gold;
    }

    //해금 유닛 데이터 설정
    public void SetUnlockData()
    {
        //1레벨 유닛 중 중복 없이 카드 개수만큼 뽑기
        List<int> numbers = new List<int>();
        for (int i = 0; i < units_Level_1.Count; i++)
            numbers.Add(i);

        for (int k = 0; k < unitUnlock.cards.Count; k++)
        {
            int index = UnityEngine.Random.Range(0, numbers.Count);
            unitUnlock.cards[k].SetData(units_Level_1[numbers[index]]);
            numbers.RemoveAt(index);
        }

        for (int i = 0; i < unitUnlock.cards.Count; i++)
        {
            if (test_Units[i])
                unitUnlock.cards[i].SetData(test_Units[i]);
        }
    }
    #endregion

    #region 공주 관련 함수
    //공주 부활을 쿨타임 설정하는 함수
    public void PrincessCoolDown()
    {
        princessHpPanel.rest_Time = 3f;
    }

    //공주 부활
    public void PrincessRivive()
    {
        //카메라 설정
        cameraMove.isPrincessDead = false;
        cameraMove.isChasePrincess = true;
        //스폰 위치로 이동
        princess.transform.position = spawn_Trans.position;
        princess.Rivive();
    }
    #endregion

    //2배속 함수
    public void OnFasty()
    {
        isFasty = !isFasty;
        Time.timeScale = isFasty ? 2 : 1;
        fasty_Mask.SetActive(isFasty);
    }

    public void OnPause(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
    }
}
