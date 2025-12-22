using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalMeal : MonoBehaviour
{
    [SerializeField] TMP_Text additional_Text;
    [SerializeField] Button additionalMeal_Button;
    [SerializeField] Button resetMeal_Button;
    int additionalMeal_cost;
    int resetMeal_cost;

    private void Awake()
    {
        additionalMeal_cost = MainManager.instance.additionalMeal_cost;
        resetMeal_cost = MainManager.instance.resetMeal_cost;
    }

    private void Update()
    {
        SetText();
    }

    public void SetText()
    {
        int soul = MainManager.instance.Soul;
        additional_Text.text = $"{additionalMeal_cost}소울을 소모해 식사를 한 번 더 진행하시겠습니까?\n\r\n또는 {resetMeal_cost}소울을 소모해 식사를 초기화할 수 있습니다.\n 현재 소울: {soul}";
        additionalMeal_Button.interactable = soul >= additionalMeal_cost;
        resetMeal_Button.interactable = soul >= resetMeal_cost;
    }

    public void PayAdditionalMealCost()
    {
        MainManager.instance.Soul -= additionalMeal_cost;
    }

    public void PayResetMealCost()
    {
        MainManager.instance.Soul -= resetMeal_cost;
    }
}
