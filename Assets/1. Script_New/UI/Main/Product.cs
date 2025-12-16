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
        buy_Text.text = isBuyable ? "구매" : "구매 완료";
    }
}
