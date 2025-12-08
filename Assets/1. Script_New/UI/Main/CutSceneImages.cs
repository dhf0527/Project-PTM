using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneImages : MonoBehaviour
{
    public void OnNextCutScene()
    {
        CutSceneManager.instance.OnNextCutScene();
    }
}
