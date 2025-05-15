using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    float min_X;
    float max_X;

    Transform princess_Trans;

    void Start()
    {
        min_X = DunGeonManager_New.instance.boundary_Min_x;
        max_X = DunGeonManager_New.instance.boundary_Max_x;
        princess_Trans = FindAnyObjectByType<Princess>().transform;
    }

    void Update()
    {
        if (min_X == max_X)
        {
            min_X = DunGeonManager_New.instance.boundary_Min_x;
            max_X = DunGeonManager_New.instance.boundary_Max_x;
            return;
        }

        Vector3 target_Pos = GetComponent<RectTransform>().anchoredPosition;
        float max_w = transform.parent.GetComponent<RectTransform>().rect.width;
        target_Pos.x = (princess_Trans.position.x - min_X) / (max_X - min_X) * max_w;
        GetComponent<RectTransform>().anchoredPosition = target_Pos;
    }
}
