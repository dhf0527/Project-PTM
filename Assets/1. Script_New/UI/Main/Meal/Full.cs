using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Full : MonoBehaviour
{
    public TMP_Text full_Text;


    private void Start()
    {
        
    }

    void Update()
    {
        DateTime lastEat = DateTime.Parse(PlayerPrefs.GetString(ConstData.mealCompleteTime));
        DateTime now = DateTime.Now;
        TimeSpan difference = now - lastEat;

        int restSec = Mathf.Max(120 - (int)difference.TotalSeconds, 0);
        full_Text.text = $"너무 배가 불러서 더 먹을 수 없습니다!\r\n({restSec}초 뒤에 다시 식사를 진행할 수 있습니다.)";
    }
}
