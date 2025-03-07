using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    //경고 텍스트
    [SerializeField] Image warnText_Image;
    [SerializeField] TMP_Text warnText_Text;
    [Header("0:경고, 1:카운트다운")]
    [SerializeField] Color[] textColors = new Color[2]; 
    public float[] waveTimes = new float[3];
    [Header("0:초반, 1:중반")]
    public float[] phaseTimes = new float[2];
    public float[] phase0_SpawnTimes = new float[3];
    public float[] phase1_SpawnTimes = new float[3];
    public float[] phase2_SpawnTimes = new float[3];

    [Header("적 유닛(임시)")]
    public Unit[] wave1_enemy = new Unit[3];
    public Unit[] wave2_enemy = new Unit[3];
    public Unit[] wave3_enemy = new Unit[3];

    //현재 웨이브-1
    int cur_Wave = 0;
    int cur_Phase = 0;


    //C_SetWarnText를 담는 코루틴
    Coroutine cor_warn;
    Color warnImg_Color;
    float cur_WaveTime = 0;
    bool isWarned;
    bool isCountDowned;

    //유닛간 최소 Y축 차이
    Vector3 spawn_Y;
    // spawn_Units[m,n] -> 유닛번호 m-n(m웨이브 n번째) 
    Unit[,] spawn_Units = new Unit[3,3];
    //스폰 시간
    float[] spawn_Time = { 10, 20, 30 };
    //스폰 시간 카운터
    float[] spawn_Time_Count = new float[3];

    private void Awake()
    {
        instance = this;
        warnImg_Color = warnText_Image.color;
    }

    private void Start()
    {
        spawn_Y = DunGeonManager_New.instance.SpawnY();

        //스폰할 유닛 데이터 삽입(임시)
        for (int i = 0; i < spawn_Units.GetLength(0); i++)
        {
            spawn_Units[0, i] = wave1_enemy[i];
            spawn_Units[1, i] = wave2_enemy[i];
            spawn_Units[2, i] = wave3_enemy[i];
        }

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

        //웨이브 시간 확인
        CheckWaveTime();

        //주기적으로 유닛 생산
        Spawn_Timer();
        
    }

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
                    Spawn_Unit(spawn_Units[cur_Wave, i]);
            }
        }
    }

    //유닛 생산 함수
    void Spawn_Unit(Unit unit)
    {
        //유닛 생산
        Unit baseUnit = Instantiate(unit, spawn_Trans);
        baseUnit.transform.position += spawn_Y;
        //부모 설정(적 유닛들만 모아놓은 Gameobject)
        baseUnit.transform.parent = enemy_Unit_Parent;
        //팀 설정
        baseUnit.IsTeam = false;
    }

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
                StartCoroutine(C_CountDown(10.5f));
            }
        }
        //웨이브 시간 종료 시 다음 웨이브로 넘어감
        else if (cur_WaveTime >= waveTimes[cur_Wave])
            ToNextWave();
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
    }

    //(wave)웨이브 이전이라면 다음 웨이브로 넘어가는 함수
    public void ToNextWave(int wave)
    {
        if (cur_Wave < wave - 1)
            ToNextWave();
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
    }
}
