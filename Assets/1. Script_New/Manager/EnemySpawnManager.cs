using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour
{
    //싱글톤
    public static EnemySpawnManager instance;

    //유닛이 생성될 위치
    [SerializeField] Transform spawn_Trans;
    //유닛 오브젝트가 들어갈 부모
    [SerializeField] Transform enemy_Unit_Parent;
    //웨이브 텍스트
    public WavePanel wavePanel;
    [SerializeField] ShockWave shockWave_prf;
    public Image waveProgress_Image;

    [Header("(테스트용)자동 스폰 중지")]
    [SerializeField] bool isStopSpawn;
    [SerializeField] Animator backGround_Anim;

    //경고 텍스트
    public Image warnText_Image;
    [SerializeField] TMP_Text warnText_Text;
    [Header("0:경고, 1:카운트다운")]
    [SerializeField] Color[] textColors = new Color[2]; 
    public float[] waveTimes = new float[3];
    [Header("0:초반, 1:중반")]
    public float[] phaseTimes = new float[2];
    public float[] phase0_SpawnTimes = new float[3];
    public float[] phase1_SpawnTimes = new float[3];
    public float[] phase2_SpawnTimes = new float[3];

    [Header("웨이브 변화 조건 - 요새 체력(%, 내림차순)")]
    public float[] wave_BaseHps = new float[2];

    [HideInInspector] public Unit boss_Unit;

    [Header("보스 소환 조건 - 요새 체력(%)")]
    public float boss_BaseHp;

    [Header("보스 능력치 배수(Hp1+(번호*Hp2))")]
    public float boss_Hp1;
    public float boss_Hp2;
    public float boss_Attack1;
    public float boss_Attack2;

    [Header("유닛 레벨별 처치 골드")]
    public List<int> killGolds;
    public int killGold_Boss;

    [HideInInspector] public bool isBossDead;

    //보스 오라 이펙트
    public GameObject bossAura;

    //현재 웨이브-1
    [HideInInspector] public int cur_Wave = 0;
    int cur_Phase = 0;
    int target_Wave = 0;

    //C_SetWarnText를 담는 코루틴
    Coroutine cor_warn;
    Coroutine cor_countDown;
    Color warnImg_Color;
    float cur_WaveTime = 0;
    bool isWarned;
    bool isCountDowned;

    // spawn_Units[m,n] -> 유닛번호 m-n(m웨이브 n번째) 
    Unit[,] spawn_Units = new Unit[3,3];
    //스폰 시간
    float[] spawn_Time = { 10, 20, 30 };
    //스폰 시간 카운터
    float[] spawn_Time_Count = new float[3];
    //보스 소환 판별 변수
    bool isBossSpawned = false;
    

    private void Awake()
    {
        instance = this;
        warnImg_Color = warnText_Image.color;
    }

    private void Start()
    {
        for (int i = 0; i < spawn_Units.GetLength(0); i++)
        {
            spawn_Units[0, i] = GameManager.Instance.current_Dungeon.units_Wave1[i];
            spawn_Units[1, i] = GameManager.Instance.current_Dungeon.units_Wave2[i];
            spawn_Units[2, i] = GameManager.Instance.current_Dungeon.units_Wave3[i];

            if(spawn_Units[0, i])
                spawn_Units[0, i].killGold = killGolds[spawn_Units[0,i].ud.level - 1];
            if (spawn_Units[1, i])
                spawn_Units[1, i].killGold = killGolds[spawn_Units[1,i].ud.level - 1];
            if (spawn_Units[2, i])
                spawn_Units[2, i].killGold = killGolds[spawn_Units[2,i].ud.level - 1];
        }
        boss_Unit = GameManager.Instance.current_Dungeon.bossUnit;

        if(!isStopSpawn)
            Spawn_Unit(spawn_Units[0, 0]);
    }

    private void Update()
    {
        //테스트
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Spawn_Unit(spawn_Units[0,0]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ToNextWave();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            isBossSpawned = false;
            OnBossSpawn();
        }

        //웨이브 시간 확인
        CheckWaveTime();

        if (DunGeonManager_New.instance.pauseStack == 0 && target_Wave > cur_Wave)
            ToNextWave();

        //주기적으로 유닛 생산
        if(!isStopSpawn)
            Spawn_Timer();

        if(isBossSpawned)
        {
            if (!isBossDead)
                DunGeonManager_New.instance.enemyBase.isImmune = true;
            else
                DunGeonManager_New.instance.enemyBase.isImmune = false;
        }
        
    }

    #region 유닛 생산 함수
    //주기적으로 유닛을 생산하는 함수
    void Spawn_Timer()
    {
        for (int i = 0; i < spawn_Time_Count.Length; i++)
        {
            spawn_Time_Count[i] += Time.deltaTime;

            //스폰 시간마다 유닛 생산
            if (spawn_Time_Count[i] >= spawn_Time[i])
            {
                spawn_Time_Count[i] = 0;

                //해당 유닛 번호가 비어있으면 유닛을 생산하지 않음
                if (spawn_Units[cur_Wave, i])
                {
                    //크라운 스테이크
                    if (GameManager.Instance.current_Meal?.code == 103)
                        StartCoroutine(C_Spawn_Unit(spawn_Units[cur_Wave, i]));
                    else
                        Spawn_Unit(spawn_Units[cur_Wave, i]);

                }
            }
        }
    }

    //유닛 생산 함수
    public Unit Spawn_Unit(Unit unit)
    {
        if (!unit)
            return null;

        //유닛 생산
        Unit spawned_Unit = Instantiate(unit, spawn_Trans);
        spawned_Unit.transform.position += DunGeonManager_New.instance.SpawnY(spawned_Unit);
        //부모 설정(적 유닛들만 모아놓은 Gameobject)
        spawned_Unit.transform.parent = enemy_Unit_Parent;
        //팀 설정
        spawned_Unit.IsTeam = false;

        //식사 효과(꽃빙수)
        if(GameManager.Instance.current_Meal?.code == 4)
        {
            spawned_Unit.unitStatData_st.moveSpeed_PlusPercent -= GameManager.Instance.current_Meal.mealValue;
            spawned_Unit.unitStatData_st.attackSpeed_PlusPercent -= GameManager.Instance.current_Meal.mealValue;
        }

        DunGeonManager_New.instance.onStageUnits_Test.Add(spawned_Unit);
        return spawned_Unit;
    }

    IEnumerator C_Spawn_Unit(Unit unit)
    {
        Spawn_Unit(unit);
        yield return new WaitForSeconds(0.5f);
        Spawn_Unit(unit);
    }
    #endregion

    #region 웨이브 함수
    //웨이브 시간을 확인하는 함수
    void CheckWaveTime()
    {
        cur_WaveTime += Time.deltaTime;

        //구간 설정
        //후반
        if (cur_WaveTime >= phaseTimes[0] + phaseTimes[1])
        {
            if (cur_Phase != 2)
            {
                cur_Phase = 2;
                spawn_Time[0] = phase0_SpawnTimes[0];
                spawn_Time[1] = phase0_SpawnTimes[1];
                spawn_Time[2] = phase0_SpawnTimes[2];
                Debug.Log($"{cur_Phase + 1}페이즈");
            }
        }
        //중반
        else if (cur_WaveTime >= phaseTimes[0])
        {
            if (cur_Phase != 1)
            {
                cur_Phase = 1;
                spawn_Time[0] = phase1_SpawnTimes[0];
                spawn_Time[1] = phase1_SpawnTimes[1];
                spawn_Time[2] = phase1_SpawnTimes[2];
                Debug.Log($"{cur_Phase + 1}페이즈");
            }
        }
        //초반
        else
        {
            if (cur_Phase != 0)
            {
                cur_Phase = 0;
                spawn_Time[0] = phase2_SpawnTimes[0];
                spawn_Time[1] = phase2_SpawnTimes[1];
                spawn_Time[2] = phase2_SpawnTimes[2];
                Debug.Log($"{cur_Phase + 1}페이즈");
            }
        }

        //마지막 웨이브일 때
        if (cur_Wave >= waveTimes.Length - 1)
        {
            //120초 남았을 때 경고
            if (cur_WaveTime >= waveTimes[waveTimes.Length - 1] - 120 && !isWarned)
            {
                if(cor_warn != null)
                isWarned = true;
                InitTexts(0);
                warnText_Text.text = "남은 시간 120초!";

                //코루틴 중첩 방지
                if (cor_warn != null)
                    StopCoroutine(cor_warn);
                cor_warn = StartCoroutine(C_SetWarnText(5));
            }
            //10초 남았을 때 카운트 다운
            if (cur_WaveTime >= waveTimes[waveTimes.Length - 1] - 10.5f && !isCountDowned)
            {
                isCountDowned = true;
                InitTexts(1);

                //코루틴 중첩 방지
                if (cor_warn != null)
                    StopCoroutine(cor_warn);
                cor_warn = StartCoroutine(C_SetWarnText(11f));
                cor_countDown = StartCoroutine(C_CountDown(10.5f));
            }

            //15초 경과 시 보스 등장
            if (!isBossSpawned && cur_WaveTime >= 15f)
                OnBossSpawn();
        }
        //웨이브 시간 종료 시 다음 웨이브로 넘어감
        else if (cur_WaveTime >= waveTimes[cur_Wave])
            ToNextWave();

        waveProgress_Image.fillAmount = cur_WaveTime / waveTimes[cur_Wave];
    }

    //다음 웨이브로 넘어가는 함수
    public void ToNextWave()
    {
        if (cur_Wave >= waveTimes.Length - 1)
            return;

        cur_WaveTime = 0;
        wavePanel.ToNextWave(cur_Wave);
        cur_Wave++;
        cur_Phase = 0;
        DunGeonManager_New.instance.OpenUnitUnlock(cur_Wave + 1);
    }

    //(wave)웨이브 이전이라면 다음 웨이브로 넘어가는 함수
    public void ToNextWave(int wave)
    {
        target_Wave = wave;
    }

    void InitTexts(int index)
    {
        warnText_Image.color = warnImg_Color;
        warnText_Text.color = textColors[index];
    }

    //warnTime동안 메세지를 노출하는 함수
    IEnumerator C_SetWarnText(float warnTime)
    {
        warnText_Image.gameObject.SetActive(true);
        float image_origin_alp = warnText_Image.color.a;
        float text_origin_alp = warnText_Text.color.a;
        float cur_Warn_Time = 0;
        float img_alp = 0;
        float text_alp = 0;

        while (cur_Warn_Time <= warnTime)
        {
            cur_Warn_Time += Time.deltaTime;
            
            //0.5초에 걸쳐 디졸브 인
            if (cur_Warn_Time < 0.5f)
            {
                img_alp = Mathf.Lerp(0, image_origin_alp, cur_Warn_Time / 0.5f);
                text_alp = Mathf.Lerp(0, text_origin_alp, cur_Warn_Time / 0.5f);
            }
            //wanrTime이 끝나기 0.5초 전부터 디졸브 아웃
            else if (cur_Warn_Time > warnTime - 0.5f)
            {
                img_alp = Mathf.Lerp(image_origin_alp, 0, (cur_Warn_Time - (warnTime - 0.5f)) / 0.5f);
                text_alp = Mathf.Lerp(text_origin_alp, 0, (cur_Warn_Time - (warnTime - 0.5f)) / 0.5f);
            }

            Color img_color = warnText_Image.color;
            Color text_color = warnText_Text.color;
            img_color.a = img_alp;
            text_color.a = text_alp;
            warnText_Image.color = img_color;
            warnText_Text.color = text_color;

            yield return new WaitForEndOfFrame();
        }
        warnText_Image.gameObject.SetActive(false);
    }
    
    //카운트다운 함수
    IEnumerator C_CountDown(float countTime)
    {
        float cur_Timer = countTime;
        while (cur_Timer > 0)
        {
            cur_Timer -= Time.deltaTime;
            warnText_Text.text = $"남은 시간 {(int)cur_Timer + 1}초!";
            yield return new WaitForEndOfFrame() ;
        }
        DunGeonManager_New.instance.OpenGameOverPanel();
    }

    public void StopWarn()
    {
        StopAllCoroutines();
        warnText_Image.gameObject.SetActive(false);
    }
    #endregion

    #region 보스 소환 함수
    public void OnBossSpawn()
    {
        if (!isBossSpawned)
        {
            ToNextWave(3);
            backGround_Anim.SetTrigger("SpawnBoss");
            AudioManager.Instance.PlayerBgm(BGM_Enum.Boss);
            isBossSpawned = true;
        }
    }

    public void SpawnBossUnit()
    {
        //카메라 진동
        Camera.main.GetComponent<CameraMove>().ShakeCamera(1f, 2);
        //충격파 발생
        MakeShockWave();
        //보스 소환
        Unit bossUnit = Spawn_Unit(boss_Unit);
        //넉백 방지
        bossUnit.canKnockBack = false;
        //크기 조정
        bossUnit.origin_Scale = 1.2f;
        //소,중형 -> 중,대형
        if(bossUnit.ud.size != Unit_Size.Large)
        {
            bossUnit.ud.size++;
            DunGeonManager_New.instance.SpawnY(bossUnit);
        }
        //능력치 조정
        bossUnit.unitStatData_st.max_Hp_Plus += bossUnit.ud.hp * ((boss_Hp1 - 1) + GameManager.Instance.current_Dungeon.stage * boss_Hp2);
        bossUnit.Cur_Hp = bossUnit.Max_Hp;
        bossUnit.unitStatData_st.attack_PlusPercent += ((boss_Attack1 - 1) + GameManager.Instance.current_Dungeon.number * boss_Attack2) * 100;
        bossUnit.unitStatData_st.targetCount_Plus += bossUnit.ud.target_Count;
        bossUnit.killGold = killGold_Boss;
        //보스의 수호 버프 부여
        bossUnit.AddComponent<BossGuard>();
        //아우라 생성
        Instantiate(bossAura, bossUnit.transform);
    }

    public void MakeShockWave()
    {
        Instantiate(shockWave_prf, spawn_Trans);
    }
    #endregion


}
