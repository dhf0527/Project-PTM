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

    [SerializeField] List<GameObject> dayButtons;
    [SerializeField] List<GameObject> nightButtons;

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

            foreach (var item in dayButtons)
                item.SetActive(!isNight);
            foreach (var item in nightButtons)
                item.SetActive(isNight);
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
        screen_Width = GetComponent<RectTransform>().rect.width;
        SetTransformMaps(0);

        FastChange(GameManager.Instance.current_Dungeon?.stage >= 5);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            FastChange(true);
        if (Input.GetKeyDown(KeyCode.M))
            FastChange(false);
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

    public void FastChange(bool isToNight)
    {
        SetTransformMaps(isToNight ? 1 : 0);
        IsNight = isToNight;
    }
}
