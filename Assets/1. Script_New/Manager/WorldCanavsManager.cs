using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanavsManager : MonoBehaviour
{
    public static WorldCanavsManager instance;

    //World Space Canvas
    public Transform worldCanvas_Trans;

    private void Awake()
    {
        instance = this;
    }

    //���� ü�¹� ������
    public HpBar_new hpBar_Prf;
}
