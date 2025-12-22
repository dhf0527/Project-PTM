using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Full_HardMode : MonoBehaviour
{
    [SerializeField] TMP_Text full_HardMode_Text;
    [SerializeField] Button resetMeal_Button;
    int resetMeal_cost;

    private void Awake()
    {
        resetMeal_cost = MainManager.instance.resetMeal_cost;
    }

    private void Update()
    {
        SetText();
    }

    public void SetText()
    {
        int soul = MainManager.instance.Soul;
        full_HardMode_Text.text = $"아무리 맛있어도 한 번에 식사 세 번은 무리입니다! \n\n{resetMeal_cost}소울을 소모해 식사를 초기화하시겠습니까?\n 현재 소울: {soul}";
        resetMeal_Button.interactable = soul >= resetMeal_cost;
    }
    public void PayResetMealCost()
    {
        MainManager.instance.Soul -= resetMeal_cost;
    }
}
