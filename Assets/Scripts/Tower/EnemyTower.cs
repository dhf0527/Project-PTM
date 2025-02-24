using Chracter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class EnemyTower : BaseCharacter
{
    [System.Serializable]
    public struct WaveEnemy
    {
        public GameObject enemyObj;
        public float delay;
        public float timer;
    }

    [System.Serializable]
    public struct WaveData
    {
        public WaveEnemy enemy;

        public float totalTime;
        public float firstTime; //초반
        public float midTime; //중반
        public float lateTime; //후반
    }
    
    public List<WaveData> stageData;

    //Spawn

    [Header("UI")]
    public GameObject[] enemyObjs;

    public List<EnemySpawn> spawnList;

    public float spwanDelay1;
    public float spwanDelay2;
    public float spwanDelay3;
    string spawnType;

    public Transform enemySpawnPoint;

    private bool isStageEnd = false;
    private bool isCanSpawnEnemy = true;
    //UI
    [Header("UI")]
    public Slider towerHPSlider;
    public Text towerHPText;

    public GameObject Boss;


    //Game Clear
    public GameObject stageClearPanel;

    private void Awake()
    {
        //spawn
        spawnList = new List<EnemySpawn>();

        //delay 초기화
        spwanDelay1 = 10;
        spwanDelay2 = 20;
        spwanDelay3 = 30;
    }

    private void Start()
    {
        SetCharacterSettings(500);
        towerHPSlider.maxValue = MaxHealth;
        ReadSpawnFile();

        //
        EnemyCreate();
    }

    private void Update()
    {
        towerHPSlider.value = CurrentHealth;
        towerHPText.text = CurrentHealth + " / " + MaxHealth;
        
    }

    public void ReadSpawnFile()
    {
        //변수 초기화
        spawnList.Clear();


        for (int i = 0; i < StageManager.Instance.wave; i++)
        {
            //파일 읽기
            Debug.Log("Wave" + StageManager.Instance.wave.ToString());
            TextAsset textFile = Resources.Load("Dungeon" + DataManager.currentDungeon + "/Wave" + StageManager.Instance.wave.ToString()) as TextAsset;
            StringReader reader = new StringReader(textFile.text);

            string line = reader.ReadLine();
            Debug.Log(line);

            EnemySpawn spawnData = new EnemySpawn();
            spawnData.enemyA = line.Split(',')[0]; //
            spawnData.enemyB = line.Split(',')[1];
            spawnData.enemyC = line.Split(',')[2]; //3개값을 추가한 스폰데이터를 넣어준다
            spawnList.Add(spawnData);

            Debug.Log(spawnList);
            //텍스트 파일 닫기
            reader.Close();
        }
    }

    void SpawnEnemy(string type)
    {
        int enemyIndex = 0;
        if (type == "없음")
        {
            isCanSpawnEnemy = false;
        }
        else
        {
            isCanSpawnEnemy = true;
        }
        switch (type)
        {
            case "검사":
                enemyIndex = 0;
                break;

            case "도적":
                enemyIndex = 1;
                break;

            case "엘프궁수":
                enemyIndex = 2;
                break;

            case "고블린":
                enemyIndex = 3;
                break;

            case "슬라임":
                enemyIndex = 4;
                break;

            case "해골전사":
                enemyIndex = 5;
                break;

            case "시체박쥐":
                enemyIndex = 6;
                break;

            case "마법사":
                enemyIndex = 7;
                break;

            case "마검사":
                enemyIndex = 8;
                break;

            case "미노타우로스":
                enemyIndex = 9;
                break;

            case "켄타우로스":
                enemyIndex = 10;
                break;

            case "골렘":
                enemyIndex = 11;
                break;

            case "불타는해골":
                enemyIndex = 12;
                break;

            case "성기사":
                enemyIndex = 13;
                break;

            case "사슴기사":
                enemyIndex = 14;
                break;

            case "나무거인":
                enemyIndex = 15;
                break;

            case "정령술사":
                enemyIndex = 16;
                break;

            case "드래곤":
                enemyIndex = 17;
                break;

            case "수확자":
                enemyIndex = 18;
                break;
        }

        if (!isCanSpawnEnemy)
            return;

        Debug.Log("#@##");
        GameObject enemy = GameObject.Instantiate(enemyObjs[enemyIndex], enemySpawnPoint);
        enemy.tag = "Enemy";
        enemy.layer = 7;
        enemy.GetComponent<BaseCharacter>().Spawn();
    }

    bool[] isEndWave = { false,false,false };
    public override void TakeDamage(float damage, float enemyAccuracy = 200,bool pierce=false ,string weak = "없음")
    {
        if (isStageEnd)
            return;

        if (Boss != null)
        {
            return;
        }

        float finalDamage = damage - Armor;
        if (finalDamage <= 0)
        {
            finalDamage = 1;
        }

        CurrentHealth -= finalDamage;

        switch (StageManager.Instance.wave)
        {
            case 1:
                if (CurrentHealth <= MaxHealth / 10 * 8) //80%이하로 내려가면
                {
                    StageManager.Instance.wave++;
                    StageManager.Instance.StageStart();
                }
                break;

            case 2:
                if (CurrentHealth <= MaxHealth / 10 * 4) //40%이하로 내려가면
                {
                    StageManager.Instance.wave++;
                    StageManager.Instance.StageStart();
                }
                break;
        }
        
        if (CurrentHealth <= 0)
        {
            StageManager.Instance.GameClear();
            //Die();
            CurrentHealth = 0;
        }


        if (healthBar != null)
        {
            healthBar.TowerHealth(CurrentHealth, MaxHealth);
        }

        if (!StageManager.Instance.isAppearBoss && CurrentHealth <= MaxHealth / 2) //보스가 생성된 적 없고 체력이 50퍼 이하로 내려갔다면
        {
            StageManager.Instance.isAppearBoss = true;
            Boss =StageManager.Instance.AppearBoss();
        }
    }


    public void EnemyCreate()
    {
        Invoke("EnemyCreate1Ready", spwanDelay1);
        Invoke("EnemyCreate2Ready", spwanDelay2);
        Invoke("EnemyCreate3Ready", spwanDelay3);
    }

    IEnumerator EnemyCreate1()
    {
        switch (StageManager.Instance.wave)
        {
            case 1:
                SpawnEnemy(spawnList[0].enemyA); //웨이브 1의 첫번째 적
                break;

            case 2:
                SpawnEnemy(spawnList[1].enemyA); //웨이브 2의 첫번째 적
                break;

            case 3:
                SpawnEnemy(spawnList[2].enemyA); //웨이브 3의 첫번째 적
                break;
        }


        Debug.Log("x-1적: " + spawnList[StageManager.Instance.wave - 1].enemyA);
        yield return new WaitForSeconds(spwanDelay1);

        StartCoroutine(EnemyCreate1());
    }

    public void EnemyCreate1Ready()
    {
        StartCoroutine(EnemyCreate1());
    }
    public void EnemyCreate2Ready()
    {
        StartCoroutine(EnemyCreate2());
    }
    public void EnemyCreate3Ready()
    {
        StartCoroutine(EnemyCreate3());
    }
    IEnumerator EnemyCreate2()
    {
        switch (StageManager.Instance.wave)
        {
            case 1:
                SpawnEnemy(spawnList[0].enemyB); //웨이브 1의 첫번째 적
                break;

            case 2:
                SpawnEnemy(spawnList[1].enemyB); //웨이브 2의 두번째 적
                break;

            case 3:
                SpawnEnemy(spawnList[2].enemyB); //웨이브 3의 두번째 적
                break;
        }

        Debug.Log("x-2적: "+spawnList[StageManager.Instance.wave - 1].enemyB);


        yield return new WaitForSeconds(spwanDelay2);

        StartCoroutine(EnemyCreate2());
    }

    IEnumerator EnemyCreate3()
    {
        switch (StageManager.Instance.wave)
        {
            case 1:
                SpawnEnemy(spawnList[0].enemyC); //웨이브 1의 첫번째 적
                break;

            case 2:
                SpawnEnemy(spawnList[1].enemyC); //웨이브 2의 세번째 적
                break;

            case 3:
                SpawnEnemy(spawnList[2].enemyC); //웨이브 3의 세번째 적
                break;
        }


        Debug.Log("x-3적: " + spawnList[StageManager.Instance.wave - 1].enemyC);
        yield return new WaitForSeconds(spwanDelay3);

        StartCoroutine(EnemyCreate3());
    }
}
