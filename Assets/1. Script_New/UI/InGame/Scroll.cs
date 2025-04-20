using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public List<Transform> list_Content_Trans;

    public void OnResetContent(Transform content_Trans)
    {
        Vector3 tmpPos = content_Trans.position;
        tmpPos.y = 0;
        content_Trans.position = tmpPos;
    }

    public void OnResetAllContents()
    {
        foreach (var item in list_Content_Trans)
        {
            OnResetContent(item);
        }
    }
}
