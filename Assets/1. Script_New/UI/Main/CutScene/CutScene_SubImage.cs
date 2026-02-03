using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene_SubImage : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnStartAnimation()
    {
        CutSceneManager.instance.IsCartoonCutSceneProgressing = true;
    }

    public void OnEndAnimation()
    {
        CutSceneManager.instance.IsCartoonCutSceneProgressing = false;
    }
}
