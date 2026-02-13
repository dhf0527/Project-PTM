using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulBuyCheckText : MonoBehaviour
{
    [SerializeField] UnitUpgrade unitUpgrade;
    TMP_Text tmp_text;

    private void Awake()
    {
        tmp_text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        SetText();
    }

    void SetText()
    {
        tmp_text.text = "소울을 소모하여 스타를 구매하시겠습니까?" +
            $"\n(구매할 때마다 비용이 {MainManager.instance.starBuyCost_Per_Count} 증가합니다.)" +
            $"\n\n 보유 소울: {MainManager.instance.Soul}, 소모 소울: {unitUpgrade.StarBuyCost}";
    }
}
