using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    [SerializeField] Button buy_Button;
    [SerializeField] TMP_Text buy_Text;

    private void OnEnable()
    {
        CheckBuyable();
    }

    public void CheckBuyable()
    {
        bool isBuyable = PlayerPrefs.GetInt(ConstData.hardMode_Unlock) == 0;

        buy_Button.interactable = isBuyable;
        if(isBuyable)
        {
            buy_Text.text = "구매";
            buy_Text.color = buy_Button.colors.normalColor;
        }
        else
        {
            buy_Text.text = "구매 완료";
            buy_Text.color = buy_Button.colors.disabledColor;
        }
    }
}
