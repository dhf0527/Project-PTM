using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

    public GameObject heroUpgrade_Button;
    public GameObject meal_Button;
    public List<GameObject> unitUpgrade_Buttons;
    public GameObject pedia_Button;
    public GameObject product_Button;

    [Header("영웅,식사,용병단,도감 순서")]
    public List<GameObject> lockImages_go;
    public List<GameObject> lockImages_go_HardMode;

    public GameObject notification;
    public TMP_Text notification_Text_1;
    public TMP_Text notification_Text_2;

    public GameObject notification_heroUpgrade;
    public GameObject notification_meal;
    public List<GameObject> notification_unitUpgrades;
    public GameObject notification_pedia;

    public List<Button> pages;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        CheckUnlock();
        CheckHardModeUnlock();

        SetNotification();
    }

    public void CheckUnlock()
    {
        heroUpgrade_Button.SetActive(PlayerPrefs.GetInt(ConstData.heroUpgrade_Unlock) == 1);
        lockImages_go[0].SetActive(PlayerPrefs.GetInt(ConstData.heroUpgrade_Unlock) != 1);

        meal_Button.SetActive(PlayerPrefs.GetInt(ConstData.meal_Unlock) == 1);
        lockImages_go[1].SetActive(PlayerPrefs.GetInt(ConstData.meal_Unlock) != 1);

        pedia_Button.SetActive(PlayerPrefs.GetInt(ConstData.pedia_Unlock) == 1);
        lockImages_go[3].SetActive(PlayerPrefs.GetInt(ConstData.pedia_Unlock) != 1);

        product_Button.SetActive(PlayerPrefs.GetInt(ConstData.product_Unlock) == 1);

        lockImages_go[2].SetActive(PlayerPrefs.GetInt(ConstData.unitUpgrade_Unlock) == 0);
        for (int i = 0; i < unitUpgrade_Buttons.Count; i++)
        {
            if (i < PlayerPrefs.GetInt(ConstData.unitUpgrade_Unlock))
                unitUpgrade_Buttons[i].SetActive(true);
            else
                unitUpgrade_Buttons[i].SetActive(false);
        }
    }

    void CheckHardModeUnlock()
    {
        //하드모드 컨텐츠들 언락
        bool isUnlockHardMode = PlayerPrefs.GetInt(ConstData.hardMode_Unlock) != 0
            && PlayerPrefs.GetInt(ConstData.dungeonClearTime + $"{4},{3}") > 0;
        foreach (var item in lockImages_go_HardMode)
            item.SetActive(isUnlockHardMode);

        //최초 언락시 알림
        if (PlayerPrefs.GetInt(ConstData.hardMode_Unlock) == 1 && isUnlockHardMode)
        {
            PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.hardMode_Unlock);
            SetNotification();
            PlayerPrefs.SetInt(ConstData.hardMode_Unlock, 2);
        }
    }

    void SetNotification()
    {
        string new_Unlock = PlayerPrefs.GetString(ConstData.new_Unlock);
        if(string.IsNullOrEmpty(new_Unlock))
        {
            //notification.SetActive(false);
            return;
        }
        else
        {
            notification.SetActive(true);
            switch (new_Unlock)
            {
                case ConstData.unitItem_Unlock:
                    notification_Text_1.text = "용병 아이템 기능이 해금 되었습니다!";
                    notification_Text_2.text = "이제 용병을 모집할 때 용병이 아이템을 지닌 채 등장하기도 합니다.";
                    break;
                case ConstData.heroUpgrade_Unlock:
                    notification_Text_1.text = "영웅 업그레이드 기능이 해금 되었습니다!";
                    notification_Text_2.text = "전투를 하며 모은 소울을 소모해 영웅을 강화할 수 있습니다.";
                    notification_heroUpgrade.SetActive(true);
                    break;
                case ConstData.meal_Unlock:
                    notification_Text_1.text = "식사 기능이 해금 되었습니다!";
                    notification_Text_2.text = "전투를 시작하기 전, 식사를 통해 요리의 효과를 받을 수 있습니다.";
                    notification_meal.SetActive(true);
                    break;
                case ConstData.unitUpgrade_Unlock + "1":
                    notification_Text_1.text = "용병단 업그레이드 기능이 해금 되었습니다!";
                    notification_Text_2.text = "기록을 달성하며 모은 스타를 소모해 용병들을 강화할 수 있습니다.";
                    notification_unitUpgrades[0].SetActive(true);
                    break;
                case ConstData.pedia_Unlock:
                    notification_Text_1.text = "도감 기능이 해금 되었습니다!";
                    notification_Text_2.text = "도감을 통해 유닛의 능력치를 포함한 다양한 정보를 확인할 수 있습니다.";
                    notification_pedia.SetActive(true);
                    break;
                case ConstData.unitUpgrade_Unlock + "2":
                    notification_Text_1.text = "용병단 업그레이드의 새로운 카테고리가 추가 되었습니다!";
                    notification_Text_2.text = "이제 공격 유형 별로 용병들을 강화할 수 있습니다.";
                    notification_unitUpgrades[0].SetActive(true);
                    notification_unitUpgrades[1].SetActive(true);
                    break;
                case ConstData.unitUpgrade_Unlock + "3":
                    notification_Text_1.text = "용병단 업그레이드의 새로운 카테고리가 추가 되었습니다!";
                    notification_Text_2.text = "이제 크기 별로 용병들을 강화할 수 있습니다.";
                    notification_unitUpgrades[0].SetActive(true);
                    notification_unitUpgrades[2].SetActive(true);
                    break;
                case ConstData.unitUpgrade_Unlock + "4":
                    notification_Text_1.text = "용병단 업그레이드의 새로운 카테고리가 추가 되었습니다!";
                    notification_Text_2.text = "이제 용병단 업그레이드의 모든 카테고리를 확인할 수 있습니다.";
                    notification_unitUpgrades[0].SetActive(true);
                    notification_unitUpgrades[3].SetActive(true);
                    break;
                case ConstData.hardMode_Unlock:
                    notification_Text_1.text = "왕실 용병단 패키지를 구매해주셔서 감사합니다!";
                    notification_Text_2.text = "이제 일용직 용병단의 모든 컨텐츠를 \r\n자유롭게 즐기실 수 있습니다.\r\n(자세한 내용은 상세 창을 확인해주세요)";
                    notification_unitUpgrades[0].SetActive(true);
                    notification_unitUpgrades[4].SetActive(true);
                    break;
                default:
                    Debug.LogError($"잘못된 new_Unlock값 :{new_Unlock}");
                    break;
            }

            PlayerPrefs.SetString(ConstData.new_Unlock, null);
        }
    }

    public void OnProductBuy(bool isUnlock = true)
    {
        //중복 구매시 예외처리
        if(PlayerPrefs.GetInt(ConstData.hardMode_Unlock) != 0 && isUnlock)
        {
            Debug.Log("중복 구매");
            return;
        }

        PlayerPrefs.SetInt(ConstData.hardMode_Unlock, isUnlock ? 1 : 0);
        CheckHardModeUnlock();
        SetNotification();
    }
}
