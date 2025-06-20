using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUnlock : MonoBehaviour
{
    public GameObject cardParent;
    public DetailPanel detailPanel;

    public TMP_Text unlock_Text;
    public List<Card_new> cards;
    public Button select_Button;
    public Button detail_Button;

    Card_new selected_Card;
    int level = 0;

    private void Awake()
    {
        Init();
    }  

    //card toggle에서 호출
    public void OnCardSelect()
    {
        SetDark();

        foreach(var item in cards)
        {
            //선택된 card 가져오기
            if (item.GetComponent<Toggle>().isOn)
            {
                selected_Card = item;
                SetButtonsInteractable(true);
                return;
            }
        }

        //모든 카드가 선택되지 않았으면 버튼 비활성화
        SetButtonsInteractable(false);
    }

    //선택한 카드를 제외한 나머지를 어둡게 만드는 함수
    public void SetDark()
    {
        foreach (var item in cards)
        {
            item.SetDarkMask();
        }
    }

    //선택 버튼 눌렀을 때 호출
    public void OnSelectButton()
    {
        //유닛 데이터 전달
        DunGeonManager_New.instance.spawnUnits[level] = selected_Card.unit;
        //아이템 데이터 전달
        DunGeonManager_New.instance.itemDatas[level] = selected_Card.item;
        //유닛 생산 버튼 동기화
        DunGeonManager_New.instance.SetUnitSpawnButton(level);
        //다음 해금을 위해 레벨 증가
        level++;
        //선택창 닫기
        OpenUnitUnlock(false);
    }

    //유닛 해금창을 열거나 닫는 함수
    public void OpenUnitUnlock(bool isOpen)
    {
        Init();
        unlock_Text.text = $"Lv.{level + 1} 용병 해금!";
        if (isOpen)
        {
            DunGeonManager_New.instance.OnPause(true);
            gameObject.SetActive(true);

            AudioManager.instance.PlayerSfx(SFX_Enum.CardAppear);
        }
        else
        {
            DunGeonManager_New.instance.OnPause(false);
            gameObject.SetActive(false);
        }
    }

    //카드 선택 초기화 함수
    public void Init()
    {
        foreach (var item in cards)
        {
            item.GetComponent<Toggle>().isOn = false;
            item.SetDarkMask(true);
        }

        SetButtonsInteractable(false);
    }

    //버튼들을 활성/비활성화 하는 함수
    void SetButtonsInteractable(bool interactable)
    {
        select_Button.interactable = interactable;
        detail_Button.interactable = interactable;
    }

    //상세 확인 창을 여는 함수
    public void OpenDetailPanel(bool isOpen)
    {
        detailPanel.gameObject.SetActive(isOpen);
        cardParent.gameObject.SetActive(!isOpen);
        
        if (isOpen)
        {
            detailPanel.SetDetail(selected_Card);
        }
    }
}
