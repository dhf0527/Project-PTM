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
        public float firstTime; //�ʹ�
        public float midTime; //�߹�
        public float lateTime; //�Ĺ�
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

        //delay �ʱ�ȭ
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
        //���� �ʱ�ȭ
        spawnList.Clear();


        for (int i = 0; i < StageManager.Instance.wave; i++)
        {
            //���� �б�
            Debug.Log("Wave" + StageManager.Instance.wave.ToString());
            TextAsset textFile = Resources.Load("Dungeon" + DataManager.currentDungeon + "/Wave" + StageManager.Instance.wave.ToString()) as TextAsset;
            StringReader reader = new StringReader(textFile.text);

            string line = reader.ReadLine();
            Debug.Log(line);

            EnemySpawn spawnData = new EnemySpawn();
            spawnData.enemyA = line.Split(',')[0]; //
            spawnData.enemyB = line.Split(',')[1];
            spawnData.enemyC = line.Split(',')[2]; //3������ �߰��� ���������͸� �־��ش�
            spawnList.Add(spawnData);

            Debug.Log(spawnList);
            //�ؽ�Ʈ ���� �ݱ�
            reader.Close();
        }
    }

    void SpawnEnemy(string type)
    {
        int enemyIndex = 0;
        if (type == "����")
        {
            isCanSpawnEnemy = false;
        }
        else
        {
            isCanSpawnEnemy = true;
        }
        switch (type)
        {
            case "�˻�":
                enemyIndex = 0;
                break;

            case "����":
                enemyIndex = 1;
                break;

            case "�����ü�":
                enemyIndex = 2;
                break;

            case "���":
                enemyIndex = 3;
                break;

            case "������":
                enemyIndex = 4;
                break;

            case "�ذ�����":
                enemyIndex = 5;
                break;

            case "��ü����":
                enemyIndex = 6;
                break;

            case "������":
                enemyIndex = 7;
                break;

            case "���˻�":
                enemyIndex = 8;
                break;

            case "�̳�Ÿ��ν�":
                enemyIndex = 9;
                break;

            case "��Ÿ��ν�":
                enemyIndex = 10;
                break;

            case "��":
                enemyIndex = 11;
                break;

            case "��Ÿ���ذ�":
                enemyIndex = 12;
                break;

            case "�����":
                enemyIndex = 13;
                break;

            case "�罿���":
                enemyIndex = 14;
                break;

            case "��������":
                enemyIndex = 15;
                break;

            case "���ɼ���":
                enemyIndex = 16;
                break;

            case "�巡��":
                enemyIndex = 17;
                break;

            case "��Ȯ��":
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
    public override void TakeDamage(float damage, float enemyAccuracy = 200,bool pierce=false ,string weak = "����")
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
                if (CurrentHealth <= MaxHealth / 10 * 8) //80%���Ϸ� ��������
                {
                    StageManager.Instance.wave++;
                    StageManager.Instance.StageStart();
                }
                break;

            case 2:
                if (CurrentHealth <= MaxHealth / 10 * 4) //40%���Ϸ� ��������
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

        if (!StageManager.Instance.isAppearBoss && CurrentHealth <= MaxHealth / 2) //������ ������ �� ���� ü���� 50�� ���Ϸ� �������ٸ�
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
                SpawnEnemy(spawnList[0].enemyA); //���̺� 1�� ù��° ��
                break;

            case 2:
                SpawnEnemy(spawnList[1].enemyA); //���̺� 2�� ù��° ��
                break;

            case 3:
                SpawnEnemy(spawnList[2].enemyA); //���̺� 3�� ù��° ��
                break;
        }


        Debug.Log("x-1��: " + spawnList[StageManager.Instance.wave - 1].enemyA);
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
                SpawnEnemy(spawnList[0].enemyB); //���̺� 1�� ù��° ��
                break;

            case 2:
                SpawnEnemy(spawnList[1].enemyB); //���̺� 2�� �ι�° ��
                break;

            case 3:
                SpawnEnemy(spawnList[2].enemyB); //���̺� 3�� �ι�° ��
                break;
        }

        Debug.Log("x-2��: "+spawnList[StageManager.Instance.wave - 1].enemyB);


        yield return new WaitForSeconds(spwanDelay2);

        StartCoroutine(EnemyCreate2());
    }

    IEnumerator EnemyCreate3()
    {
        switch (StageManager.Instance.wave)
        {
            case 1:
                SpawnEnemy(spawnList[0].enemyC); //���̺� 1�� ù��° ��
                break;

            case 2:
                SpawnEnemy(spawnList[1].enemyC); //���̺� 2�� ����° ��
                break;

            case 3:
                SpawnEnemy(spawnList[2].enemyC); //���̺� 3�� ����° ��
                break;
        }


        Debug.Log("x-3��: " + spawnList[StageManager.Instance.wave - 1].enemyC);
        yield return new WaitForSeconds(spwanDelay3);

        StartCoroutine(EnemyCreate3());
    }
}
