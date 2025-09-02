using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class GameClearPanel : MonoBehaviour
{
    [SerializeField] Image backGround_Image;
    [SerializeField] TMP_Text dungeonNumber_Text;
    [SerializeField] TMP_Text resultTime_Text;
    [SerializeField] Image star_Image;
    [SerializeField] TMP_Text newRecord_Text;
    [SerializeField] TMP_Text reward_Text;
    [SerializeField] TMP_Text info_Text;
    [SerializeField] Image meal_Image;

    [SerializeField] Sprite[] rank_Sprites;
    [SerializeField] bool isWin;

    int resultTime;
    int reward;
    float origin_Volume;

    public void SetClearPanel()
    {
        Time.timeScale = 1;

        //임시
        int stage = GameManager.Instance.current_Dungeon.stage;
        int number = GameManager.Instance.current_Dungeon.number;
        resultTime = (int)DunGeonManager_New.instance.inGamePlayTime;

        string dungeonNumber = $"{stage}-{number}";
        //소울 보상 계산
        reward = (int)((200 + 0.5f * resultTime) * (stage + number));

        if (isWin)
        {
            //모험가의 주먹밥
            if (GameManager.Instance.current_Meal?.code == 5)
            {
                reward *= 5;
                meal_Image.sprite = GameManager.Instance.current_Meal.mealIcon;
                meal_Image.gameObject.SetActive(true);
                info_Text.text = "(승리 보너스X5)";
            }
            else
            {
                reward *= 2;
                meal_Image.gameObject.SetActive(false);
                info_Text.text = "(승리 보너스X2)";
            }

            //랭크 스타 이미지 변경
            int clear_Rank = resultTime == 0 ? 0 : resultTime < 300 ? 3 : resultTime < 480 ? 2 : 1;
            if (clear_Rank == 0)
                star_Image.gameObject.SetActive(false);
            else
            {
                star_Image.gameObject.SetActive(true);
                star_Image.sprite = rank_Sprites[clear_Rank - 1];
            }    

            //클리어 정보 전달
            string clearData_Key = ConstData.dungeonClearTime + $"{stage},{number}";
            int pre_Record = PlayerPrefs.GetInt(clearData_Key);
            //신기록 처리
            if (resultTime < pre_Record || pre_Record == 0 )
            {
                #region 해금 처리
                if(pre_Record == 0)
                {
                    if (stage == 1 && number == 1)
                    {
                        PlayerPrefs.SetInt(ConstData.unitItem_Unlock, 1);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.unitItem_Unlock);
                    }
                    else if (stage == 1 && number == 2)
                    { 
                        PlayerPrefs.SetInt(ConstData.heroUpgrade_Unlock, 1);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.heroUpgrade_Unlock);
                    }
                    else if (stage == 1 && number == 3)
                    {
                        PlayerPrefs.SetInt(ConstData.meal_Unlock, 1);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.meal_Unlock);
                    }
                    else if (stage == 2 && number == 1 && PlayerPrefs.GetInt(ConstData.unitUpgrade_Unlock) == 0)
                    {
                        PlayerPrefs.SetInt(ConstData.unitUpgrade_Unlock, 1);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.unitUpgrade_Unlock + "1");
                    }
                    else if (stage == 2 && number == 2)
                    {
                        PlayerPrefs.SetInt(ConstData.pedia_Unlock, 1);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.pedia_Unlock);
                    }
                    else if (stage == 2 && number == 3 && PlayerPrefs.GetInt(ConstData.unitUpgrade_Unlock) == 1)
                    {
                        PlayerPrefs.SetInt(ConstData.unitUpgrade_Unlock, 2);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.unitUpgrade_Unlock + "2");
                    }
                    else if (stage == 3 && number == 1 && PlayerPrefs.GetInt(ConstData.unitUpgrade_Unlock) == 2)
                    {
                        PlayerPrefs.SetInt(ConstData.unitUpgrade_Unlock, 3);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.unitUpgrade_Unlock + "3");
                    }
                    else if (stage == 3 && number == 3 && PlayerPrefs.GetInt(ConstData.unitUpgrade_Unlock) == 3)
                    {
                        PlayerPrefs.SetInt(ConstData.unitUpgrade_Unlock, 4);
                        PlayerPrefs.SetString(ConstData.new_Unlock, ConstData.unitUpgrade_Unlock + "4");
                    }
                }
                #endregion

                //스타 획득
                int pre_Rank = pre_Record == 0 ? 0 : pre_Record < 300 ? 3 : pre_Record < 480 ? 2 : 1;
                PlayerPrefs.SetInt("Star", PlayerPrefs.GetInt("Star") + clear_Rank - pre_Rank); 

                PlayerPrefs.SetInt(clearData_Key, resultTime);
                newRecord_Text.gameObject.SetActive(true);
            }
            else
                newRecord_Text.gameObject.SetActive(false);
        }

        //보상 지급
        PlayerPrefs.SetInt("Soul", PlayerPrefs.GetInt("Soul") + reward);
        //식사 효과 제거
        GameManager.Instance.current_Meal = null;

        dungeonNumber_Text.text = dungeonNumber;
        resultTime_Text.text = $"0s";
        reward_Text.text = $"0";

        //전투 결과 효과음
        StartCoroutine(C_SetSound());
    }

    IEnumerator C_SetSound()
    {
        AudioManager.Instance.mixer.GetFloat(EMixer.BGM.ToString(), out origin_Volume);
        float t = 0;

        //배경음악 페이드 아웃
        while (t < 2)
        {
            t += Time.unscaledDeltaTime;
            AudioManager.Instance.mixer.SetFloat(EMixer.BGM.ToString(), Mathf.Lerp(origin_Volume, -80 , t / 2));

            yield return new WaitForEndOfFrame();
        }

        //전투 결과 효과음
        AudioManager.Instance.PlayerSfx(isWin ? SFX_Enum.Victory : SFX_Enum.Defeated);
    }

    public void ResetSound()
    {
        AudioManager.Instance.mixer.SetFloat(EMixer.BGM.ToString(), origin_Volume);
    }

    public void OnSetPlayTime()
    {
        StartCoroutine(C_SetPlayTime());
    }

    public void OnSetReward()
    {
        StartCoroutine(C_SetReward());
    }

    IEnumerator C_SetPlayTime()
    {
        float showTime = 0.5f;
        float curTime = 0;

        while (curTime < showTime)
        {
            curTime += Time.unscaledDeltaTime;
            resultTime_Text.text = $"{(int)(resultTime * (curTime / showTime))}sec";

            yield return new WaitForEndOfFrame();
        }
        resultTime_Text.text = $"{resultTime}sec";
        yield return new WaitForSecondsRealtime(0.2f);
        StartCoroutine(C_SetReward());
    }

    IEnumerator C_SetReward()
    {
        float showTime = 0.5f;
        float curTime = 0;

        while (curTime < showTime)
        {
            curTime += Time.unscaledDeltaTime;
            reward_Text.text = $"{(int)(reward * (curTime / showTime))}";

            yield return new WaitForEndOfFrame();
        }
        reward_Text.text = $"{reward}";
    }

    public void OnStopTime()
    {
        Time.timeScale = 0;
    }
}
