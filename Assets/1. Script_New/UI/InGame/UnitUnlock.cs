using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUnlock : MonoBehaviour
{
    public List<Card_new> cards;
    [SerializeField] Button select_Button;

    Unit selected_Unit;
    int level = 0;

    private void Awake()
    {
        Init();
    }

    //선택한 카드를 제외한 나머지를 어둡게 만드는 함수
    public void SetDark()
    {
        foreach (var item in cards)
        {
            item.SetDarkMask();
        }
    }

    //card toggle에서 호출
    public void OnCardSelect()
    {
        SetDark();

        foreach(var item in cards)
        {
            if(item.GetComponent<Toggle>().isOn)
            {
                selected_Unit = item.unit;
                break;
            }
        }
    }

    //선택 버튼 눌렀을 때 호출
    public void OnSelectButton()
    {
        //유닛 데이터 전달
        DunGeonManager_New.instance.spawnUnits[level] = selected_Unit;
        //유닛 생산 버튼 동기화
        DunGeonManager_New.instance.SetUnitSpawnButton();
        //다음 해금을 위해 레벨 증가
        level++;
        //선택창 닫기
        OpenUnitUnlock(false);
    }

    public void OpenUnitUnlock(bool isOpen)
    {
        Init();
        if (isOpen)
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        //카드 선택 초기화
        foreach (var item in cards)
            item.GetComponent<Toggle>().isOn = false;

        select_Button.interactable = false;
    }
}
