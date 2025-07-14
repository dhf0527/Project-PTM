using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    //결과창 패널
    public GameClearPanel GameOverPanel;
    public GameClearPanel GameClearPanel;

    public GameObject pauseMask;
    #endregion
    #region 유닛 생산 변수
    [Header("유닛 생산 변수")]
    //유닛을 생산할 위치
    [SerializeField] Transform spawn_Trans;
    //유닛(팀)의 부모
    [SerializeField] Transform unit_Parent;
    public List<Unit> units_Level_1;
    public List<Unit> units_Level_2;
    public List<Unit> units_Level_3;
    [Header("유닛 아이템")]
    public List<ItemData> item_Advanced;
    public List<ItemData> item_Rare;
    [Header("유닛 업그레이드")]
    public List<UnitUpgradeData> unitUpgradeDatas;

    List<int> costs = new List<int>();

    //현재 가진 아이템들
    [HideInInspector] public ItemData[] itemDatas = new ItemData[3];
    float spawn_Z = 0;
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
    #region 세력 변수
    [Header("세력 (왕국,요정,마왕,묘지기순)")]
    public List<Sprite> bridge_Sprites;
    public List<Sprite> base_Sprites;
    public List<Sprite> backGround_Sprites;
    public SpriteRenderer bridge_Sr;
    public SpriteRenderer base_Sr;
    public SpriteRenderer backGround_Sr;
    #endregion
    #region 디버깅
    [Header("(테스트용)고용할 유닛들")]
    [SerializeField] Unit[] test_Units = new Unit[3];
    #endregion

    //총 전투 시간
    [HideInInspector] public float inGamePlayTime;

    //투사체 부모
    public Transform projectile_Parent;

    [HideInInspector] public Princess princess;
    [HideInInspector] public TeamBase_Unit teamBase;
    [HideInInspector] public EnemyBase_Unit enemyBase;

    [HideInInspector] public bool isFasty;

    int pauseStack = 0;

    //싱글톤
    public static DunGeonManager_New instance;

    private void Awake()
    {
        instance = this;

        //공주 찾아서 저장
        princess = FindAnyObjectByType<Princess>();
        teamBase = FindAnyObjectByType<TeamBase_Unit>();
        enemyBase = FindAnyObjectByType<EnemyBase_Unit>();

        Set_GoldByBaseLevel();
        spawnUnits = new Unit[3];
    }

    private void Start()
    {
        //경계선 양끝 좌표 가져오기
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;

        //유닛 해금 창 설정 후 열기
        OpenUnitUnlock(1);

        //베이스 1레벨 표시
        teamBase.Base_level = 1;

        //배경, 요새 스프라이트 변경
        if (GameManager.Instance.current_Dungeon)
        {
            bridge_Sr.sprite = bridge_Sprites[(int)GameManager.Instance.current_Dungeon.stage_Faction];
            base_Sr.sprite = base_Sprites[(int)GameManager.Instance.current_Dungeon.stage_Faction];
            backGround_Sr.sprite = backGround_Sprites[(int)GameManager.Instance.current_Dungeon.stage_Faction];
        }

        //식사 효과(잿빛 후추 라면)
        if (GameManager.Instance.current_Meal?.code == 7)
            for (int i = 0; i < GameManager.Instance.current_Meal.mealValue - 1; i++)
                BaseLevelUp();
    }

    private void Update()
    {
        inGamePlayTime += Time.deltaTime;

        //0.1초마다 골드 획득
        GetGoldPerSec();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pauseMask.SetActive(Time.timeScale != 0);
            OnPause(Time.timeScale != 0);
            princess.OnMove(0);
            CutSceneManager.instance.StartCutScene("1-1" , ()=> {
                pauseMask.SetActive(Time.timeScale != 0);
                OnPause(Time.timeScale != 0);
            });

        }
    }

    #region 유닛 생산 함수(골드 포함)
    //생산 버튼을 눌렀을 때 호출될 함수
    public void OnSpawnUnit(int index)
    {
        int cost = spawnUnits[index].Cost;

        if (!unitSpawnButton[index].isCoolDown && Cur_Gold >= cost)
        {
            StartCoroutine(C_SpawnUnit(index));
            Cur_Gold -= cost;
            unitSpawnButton[index].SetCoolDown();
            AudioManager.Instance.PlayerSfx(SFX_Enum.UnitEmploy);
        }
        else
            AudioManager.Instance.PlayerSfx(SFX_Enum.Deny);
    }

    //실제로 생산을 하는 함수
    IEnumerator C_SpawnUnit(int index)
    {
        int spawnCount = spawnUnits[index].SpawnCount;

        //쌍둥이 꼬치
        if (GameManager.Instance.current_Meal?.code == 2)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand < GameManager.Instance.current_Meal.mealValue)
                spawnCount++;
        }    

        //생산 수만큼 반복
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnUnit(spawnUnits[index]);

            //생산 딜레이
            yield return new WaitForSeconds(0.5f);
        }
    }

    public Unit SpawnUnit(Unit spawnUnit)
    {
        //유닛 하나 생성 및 설정
        Unit unit = Instantiate(spawnUnit, spawn_Trans);
        unit.transform.position += SpawnY(unit) + Vector3.forward * spawn_Z;
        spawn_Z += 0.001f;
        unit.transform.parent = unit_Parent;
        unit.IsTeam = true;

        //용병 업그레이드 효과(용병단 깃발)
        int upgradeLv = PlayerPrefs.GetInt(ReadOnlyData.unitUpgrade + 8);
        if (upgradeLv != 0)
            unit.unitStatData_st.accuracy_Plus += (int)(unitUpgradeDatas[8].upgradeValue[upgradeLv - 1] * teamBase.Base_level);

        //식사 효과(숙성 참치회)
        if (GameManager.Instance.current_Meal?.code == 9)
            unit.unitStatData_st.attack_Plus += EnemySpawnManager.instance.cur_Wave * (GameManager.Instance.current_Meal.mealValue);

        return unit;
    }

    public void SetUnitSpawnButton(int index)
    {
        unitSpawnButton[index].unit = spawnUnits[index];
        unitSpawnButton[index].item = itemDatas[index];
        unitSpawnButton[index].SetUI();
    }

    //유닛이 생산될 Y축 벡터를 반환하는 함수
    public Vector3 SpawnY(Unit unit)
    {
        float y_BySize;
        switch (unit.size)
        {
            case Unit_Size.Medium:
                y_BySize = 0.12f;
                break;
            case Unit_Size.Large:
                y_BySize = 0.24f;
                break;
            default:
                y_BySize = 0;
                break;
        }

        Vector3 return_Vec = Vector3.up * y_BySize;
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
            BaseLevelUp();
        }
    }

    void BaseLevelUp()
    {
        //요새 레벨업 처리
        teamBase.Base_LevelUp();
        //골드 관련 레벨업 처리
        Set_GoldByBaseLevel();

        AudioManager.Instance.PlayerSfx(SFX_Enum.TowerUpGrade);
    }

    //아군 요새 레벨에 따라 골드 관련 변수 설정
    public void Set_GoldByBaseLevel()
    {
        Gold_Per_Sec = base_abillitiesByLevels[teamBase.Base_level - 1].base_GoldPerSec_By_Level;
        int upgradeLv = PlayerPrefs.GetInt(ReadOnlyData.unitUpgrade + 7);
        if (upgradeLv != 0)
            Gold_Per_Sec += unitUpgradeDatas[7].upgradeValue[upgradeLv - 1] * teamBase.Base_level;

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

    public void OpenUnitUnlock(int unitLevel)
    {
        SetUnlockData(unitLevel);
        unitUnlock.OpenUnitUnlock(true);
    }

    //해금 유닛 데이터 설정
    public void SetUnlockData(int unitLevel)
    {
        List<Unit> targetLevel_Units;
        switch (unitLevel)
        {
            case 1:
                targetLevel_Units = units_Level_1;
                break;
            case 2:
                targetLevel_Units = units_Level_2;
                break;
            case 3:
                targetLevel_Units = units_Level_3;
                break;
            default:
                targetLevel_Units = new List<Unit>();
                Debug.LogError("유닛 레벨 설정 오류");
                break;
        }

        #region 해당 레벨 유닛 중 중복 없이 카드 개수만큼 뽑기
        List<int> numbers = new List<int>();
        for (int i = 0; i < targetLevel_Units.Count; i++)
            numbers.Add(i);

        for (int k = 0; k < unitUnlock.cards.Count; k++)
        {
            //아이템 설정
            unitUnlock.cards[k].item = GetRandomItem();

            int index = UnityEngine.Random.Range(0, numbers.Count);
            unitUnlock.cards[k].SetData(targetLevel_Units[numbers[index]]);
            numbers.RemoveAt(index);
        }

        #endregion

        //정체불명 햄버거
        if(GameManager.Instance.current_Meal?.code == 102)
        {
            unitUnlock.cards[1].gameObject.SetActive(false);
            unitUnlock.cards[2].gameObject.SetActive(false);
        }
        else
        {
            foreach (var item in unitUnlock.cards)
                item.gameObject.SetActive(true);
        }

        //해금에 나올 유닛들 수동으로 설정(테스트용)
        for (int i = 0; i < unitUnlock.cards.Count; i++)
        {
            if (test_Units.Length > i && test_Units[i])
                unitUnlock.cards[i].SetData(test_Units[i]);
        }
    }

    //무작위로 Item을 가져오는 함수
    ItemData GetRandomItem()
    {
        //아이템 등장 확률
        int item_pro = 40;
        if (UnityEngine.Random.Range(0, 100) >= item_pro)
            return null;

        //희귀 확률
        int rare_pro = 30;
        //고급 확률
        int advenced_pro = 70;

        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < rare_pro)
            return item_Rare[UnityEngine.Random.Range(0, item_Rare.Count)];
        else 
            return item_Advanced[UnityEngine.Random.Range(0, item_Advanced.Count)];
    }

    #endregion

    #region 공주
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
        princess.transform.position = spawn_Trans.position + SpawnY(princess);
        princess.Rivive();
    }
    #endregion

    #region 배속
    //2배속 함수
    public void OnFasty()
    {
        isFasty = !isFasty;
        Time.timeScale = isFasty ? 2 : 1;
        fasty_Mask.SetActive(isFasty);
    }

    public void OnPause(bool isPause)
    {
        pauseStack += isPause ? 1 : -1;
        Time.timeScale = pauseStack > 0 ? 0 : isFasty ? 2 : 1;
    }

    public void ResetPause()
    {
        pauseStack = 0;
        Time.timeScale = 1;
    }
    #endregion

    #region 결과창
    public void OpenGameClearPanel()
    {
        EnemySpawnManager.instance.StopWarn();
        GameClearPanel.gameObject.SetActive(true);
        GameOverPanel.gameObject.SetActive(false);
        GameClearPanel.SetClearPanel();
    }

    public void OpenGameOverPanel()
    {
        EnemySpawnManager.instance.StopWarn();
        GameOverPanel.gameObject.SetActive(true);
        GameClearPanel.gameObject.SetActive(false);
        GameOverPanel.SetClearPanel();
    }
    #endregion


}
