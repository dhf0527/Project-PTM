using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LongClick : MonoBehaviour
{
    bool isClick;
    float clickTime;
    float longClickTime = 1.5f;

    private void Update()
    {
        if (isClick)
        {
            clickTime += Time.deltaTime;
            if (clickTime > longClickTime)
            {
                LongClickFunc();
                OnButtonUp();
            }
        }
        else
            clickTime = 0;
    }

    public void OnButtonDown()
    {
        isClick = true;
    }

    public void OnButtonUp()
    {
        isClick = false;
        clickTime = 0;
    }

    protected abstract void LongClickFunc();
}
