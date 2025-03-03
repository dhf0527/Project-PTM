using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunGeonManager_New : MonoBehaviour
{
    //��輱 ������Ʈ
    [SerializeField] BoxCollider2D boundary;
    //��輱 �糡 ��ǥ
    [HideInInspector] public float boundary_Min_x;
    [HideInInspector] public float boundary_Max_x;

    #region UI ����
    [Header("UI ����")]
    //���� ���� ��ư
    [SerializeField] UnitSpawnButton[] unitSpawnButton = new UnitSpawnButton[3];
    //������ ����(�ӽ�)
    [HideInInspector] public Unit[] spawnUnits;

    //���� �ر� �г�
    public UnitUnlock unitUnlock;

    //��� �г�
    public GoldPanel goldPanel;
    //��� ���׷��̵� �г�
    public BaseLevelUpPanel baseLevelUpPanel;

    //ī�޶�
    public CameraMove cameraMove;
    //���� ü�� �г�
    public PrincessHpPanel princessHpPanel;
    #endregion
    #region ���� ���� ����
    [Header("���� ���� ����")]
    //������ ������ ��ġ
    [SerializeField] Transform spawn_Trans;
    //����(��)�� �θ�
    [SerializeField] Transform unit_Parent;
    [Header("���ְ� �ּ� Y�� ����")]
    public float spawn_Y = 0.03f;

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
    int base_UpgradeCost;

    float gold_time;
    #endregion
    public List<Unit> units_Level_1;

    [Serializable]
    public class AbillitiesByLevel
    {
        public int base_Hp_By_Level;
        public int base_Armor_By_Level;
        public int base_UpgradeCost_By_Level;
        public int base_GoldPerSec_By_Level;
        public int base_MaxGold_By_Level;
    }
    [Header("�Ʊ� ����� (Element + 1)���� �ɷ�ġ")]
    public List<AbillitiesByLevel> base_abillitiesByLevels;

    //����ü �θ�
    public Transform projectile_Parent;

    [HideInInspector] public Princess princess;
    [HideInInspector] public TeamBase_Unit teamBase;

    //�̱���
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;

        //���� ã�Ƽ� ����
        princess = FindAnyObjectByType<Princess>();
        teamBase = FindAnyObjectByType<TeamBase_Unit>();

        Gold_Per_Sec = base_abillitiesByLevels[0].base_GoldPerSec_By_Level;
        Max_Gold = base_abillitiesByLevels[0].base_MaxGold_By_Level;
        base_UpgradeCost = base_abillitiesByLevels[0].base_UpgradeCost_By_Level;
        spawnUnits = new Unit[3];

        //ī�忡 ������ ����
        List<int> numbers = new List<int>();
        for (int i = 0; i < units_Level_1.Count; i++)
            numbers.Add(i);

        for (int k = 0; k < unitUnlock.cards.Count; k++)
        {
            int index = UnityEngine.Random.Range(0, numbers.Count);
            unitUnlock.cards[k].SetData(units_Level_1[numbers[index]]);
            numbers.RemoveAt(index);
        }

        //���� �ر� â ����
        unitUnlock.OpenUnitUnlock(true);
    }

    private void Start()
    {
        //��輱 �糡 ��ǥ ��������
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }

    private void Update()
    {
        //0.1�ʸ��� ��� ȹ��
        GetGoldPerSec();
    }

    #region ���� ���� �Լ�(��� ����)
    //���� ��ư�� ������ �� ȣ��� �Լ�
    public void OnSpawnUnit(int index)
    {
        if (Cur_Gold >= spawnUnits[index].ud.cost)
        {
            StartCoroutine(C_SpawnUnit(index));
            Cur_Gold -= spawnUnits[index].ud.cost;
        }
    }

    //������ ������ �ϴ� �Լ�
    IEnumerator C_SpawnUnit(int index)
    {
        //���� ����ŭ �ݺ�
        for (int i = 0; i < spawnUnits[index].ud.spawn_Count; i++)
        {
            //���� �ϳ� ���� �� ����
            Unit unit = Instantiate(spawnUnits[index], spawn_Trans);
            unit.transform.position += SpawnY();
            unit.transform.parent = unit_Parent;
            unit.IsTeam = true;
            //���� ������
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public void SetUnitSpawnButton()
    {
        //��ư�� ���� ����
        for (int i = 0; i < spawnUnits.Length; i++)
        {
            unitSpawnButton[i].unit = spawnUnits[i];
            unitSpawnButton[i].SetUI();
        }
    }

    //������ ����� Y�� ���͸� ��ȯ�ϴ� �Լ�
    public Vector3 SpawnY()
    {
        int rand = UnityEngine.Random.Range(-5, 6);
        Vector3 return_Vec = Vector3.up * rand * spawn_Y + Vector3.forward * rand * spawn_Y;
        return return_Vec;
    }

    //��� ������ ��ư�� ������ �� ȣ��
    public void OnBaseLevelUp()
    {
        //�ִ� ����
        if (teamBase.Base_level >= base_abillitiesByLevels.Count)
        {
            return;
        }

        if (Cur_Gold >= base_UpgradeCost)
        {
            Cur_Gold -= base_UpgradeCost;
            //��� ������ ó��
            teamBase.Base_LevelUp();
            //��� ���� ������ ó��
            Set_GoldByBaseLevel();
        }
        else
        {
            
        }
    }

    //�Ʊ� ��� ������ ���� ��� ���� ���� ����
    public void Set_GoldByBaseLevel()
    {
        Gold_Per_Sec = base_abillitiesByLevels[teamBase.Base_level - 1].base_GoldPerSec_By_Level;
        Max_Gold = base_abillitiesByLevels[teamBase.Base_level - 1].base_MaxGold_By_Level;
        base_UpgradeCost = base_abillitiesByLevels[teamBase.Base_level - 1].base_UpgradeCost_By_Level;
    }

    //0.1�ʸ��� ��带 ȹ���ϴ� �Լ�
    void GetGoldPerSec()
    {
        gold_time += Time.deltaTime;
        if (gold_time >= 0.1f)
        {
            GetGold(Gold_Per_Sec / 10f);
            gold_time -= 0.1f;
        }
    }

    //��带 ȹ���ϴ� �Լ�
    public void GetGold(float getGold)
    {
        Cur_Gold += getGold;
        if (Cur_Gold > Max_Gold)
            Cur_Gold = Max_Gold;
    }
    #endregion

    #region ���� ���� �Լ�
    //���� ��Ȱ�� ��Ÿ�� �����ϴ� �Լ�
    public void PrincessCoolDown()
    {
        princessHpPanel.rest_Time = 3f;
    }

    //���� ��Ȱ
    public void PrincessRivive()
    {
        //ī�޶� ����
        cameraMove.isPrincessDead = false;
        cameraMove.isChasePrincess = true;
        //���� ��ġ�� �̵�
        princess.transform.position = spawn_Trans.position;
        princess.Rivive();
    }
    #endregion
}
