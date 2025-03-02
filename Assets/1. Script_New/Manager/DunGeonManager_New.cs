using System;
using System.Collections;
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
    [SerializeField] Unit[] spawnUnits = new Unit[3];

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
    #endregion
    //����ü �θ�
    public Transform projectile_Parent;

    [HideInInspector] public Princess princess;

    //�̱���
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;

        //���� ã�Ƽ� ����
        princess = FindAnyObjectByType<Princess>();

        //��ư�� ���� ����(�ӽ�)
        for (int i = 0; i < unitSpawnButton.Length; i++)
        {
            unitSpawnButton[i].unit = spawnUnits[i];
        }
    }

    private void Start()
    {
        //��輱 �糡 ��ǥ ��������
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }

    #region ���� ���� �Լ�
    //���� ��ư�� ������ �� ȣ��� �Լ�
    public void OnSpawnUnit(int index)
    {
        StartCoroutine(C_SpawnUnit(index));
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

    //������ ����� Y�� ���͸� ��ȯ�ϴ� �Լ�
    public Vector3 SpawnY()
    {
        int rand = UnityEngine.Random.Range(-5, 6);
        Vector3 return_Vec = Vector3.up * rand * spawn_Y + Vector3.forward * rand * spawn_Y;
        return return_Vec;
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
