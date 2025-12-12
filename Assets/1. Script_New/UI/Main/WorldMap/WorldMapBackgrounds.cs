using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldMapBackgrounds : MonoBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] Transform dayMap;
    [SerializeField] Transform nightMap;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;
    [SerializeField] float changeTime;

    float screen_Width;
    bool isNight;
    bool IsNight
    {
        get { return isNight; }
        set
        {
            isNight = value;
            nextButton.SetActive(!value);
            backButton.SetActive(value);
        }
    }
    bool isChanging;
    bool IsChanging 
    { 
        get { return isChanging; }
        set
        {
            isChanging = value;
            ui.SetActive(!value);
        }
    }

    private void Awake()
    {
        screen_Width = Screen.width;
        SetTransformMaps(0);
    }

    private void Update()
    {
        
    }

    void SetTransformMaps(float value)
    {
        dayMap.localPosition = new Vector3(-value * screen_Width, 0, 0);
        nightMap.localPosition = dayMap.localPosition + new Vector3(screen_Width, 0, 0);
    }

    public void DayToNight()
    {
        if(!IsChanging && !IsNight)
            StartCoroutine(C_DayToNight());
    }

    IEnumerator C_DayToNight()
    {
        IsChanging = true;
        float curTime = 0;
        while(curTime < changeTime)
        {
            SetTransformMaps(curTime / changeTime);
            //curTime += Time.unscaledDeltaTime;
            curTime += 0.017f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        SetTransformMaps(1);
        IsChanging = false;
        IsNight = true;
    }

    public void NightToDay()
    {
        if (!IsChanging && IsNight)
            StartCoroutine(C_NightToDay());
    }

    IEnumerator C_NightToDay()
    {
        IsChanging = true;
        float curTime = 0;
        while (curTime < changeTime)
        {
            SetTransformMaps(1 - curTime / changeTime);
            //curTime += Time.unscaledDeltaTime;
            curTime += 0.017f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        SetTransformMaps(0);
        IsChanging = false;
        IsNight = false;
    }
}
