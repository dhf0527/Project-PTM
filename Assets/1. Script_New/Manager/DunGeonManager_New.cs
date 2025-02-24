using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunGeonManager_New : MonoBehaviour
{
    [SerializeField] BoxCollider2D boundary;

    [HideInInspector] public float boundary_Min_x;
    [HideInInspector] public float boundary_Max_x;

    //ΩÃ±€≈Ê
    public static DunGeonManager_New instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        boundary_Min_x = boundary.bounds.min.x;
        boundary_Max_x = boundary.bounds.max.x;
    }
}
