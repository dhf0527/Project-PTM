using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)16 / 9);    //가로:세로 16:9
        float scalewidth = 1f / scaleHeight;
        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1 - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1 - scalewidth) / 2f;
        }
        camera.rect = rect;
    }
}
