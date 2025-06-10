using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meal : MonoBehaviour
{
    [Header("panel, select, eating, complete¼ø")]
    public List<GameObject> gameObjects;

    public void OpenGo(GameObject go)
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(false);
        }
        go.SetActive(true);
    }


}
