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
    [SerializeField] BaseUnit[] spawnUnits = new BaseUnit[3];

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

    [Header("���ְ� Y�� ����")]
    public float spawn_Y = 0.03f;
    #endregion
    [HideInInspector] public Princess princess;

    //�̱���
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;

        princess = FindAnyObjectByType<Princess>();

        //��ư�� ���� ����(�ӽ�)
        for (int i = 0; i < unitSpawnButton.Length; i++)
        {
            unitSpawnButton[i].unit = spawnUnits[i];
        }
    }

    private void Start()
    {
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }

    //���� ��ư�� ������ �� ȣ��� �Լ�
    public void OnSpawnUnit(int i)
    {
        BaseUnit unit = Instantiate(spawnUnits[i], spawn_Trans);
        unit.transform.position += SpawnY();
        unit.transform.parent = unit_Parent;
        unit.IsTeam = true;
    }

    public Vector3 SpawnY()
    {
        int rand = Random.Range(-5, 6);
        Vector3 return_Vec = Vector3.up * rand * spawn_Y + Vector3.forward * rand * spawn_Y;
        return return_Vec;
    }

    //���� ��Ȱ ��Ÿ��
    public void PrincessCoolDown()
    {
        princessHpPanel.rest_Time = 3f;
    }

    //���� ��Ȱ
    public void PrincessRivive()
    {
        //ī�޶� ����
        cameraMove.isPrincessDead = false;
        //���� ��ġ�� �̵�
        princess.transform.position = spawn_Trans.position;
        princess.Rivive();
    }
}
