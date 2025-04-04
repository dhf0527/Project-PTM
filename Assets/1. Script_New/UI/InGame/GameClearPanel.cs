using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearPanel : MonoBehaviour
{
    [SerializeField] Image backGround_Image;
    [SerializeField] TMP_Text dungeonNumber_Text;
    [SerializeField] TMP_Text resultTime_Text;
    [SerializeField] Image rank_Image;
    [SerializeField] TMP_Text newRecord_Text;
    [SerializeField] TMP_Text reward_Text;

    [SerializeField] Sprite[] rank_Sprites;

    int resultTime;
    int reward;
    public void SetClearPanel()
    {
        Time.timeScale = 1;

        //임시
        int stage = 1;
        int order = 1;
        resultTime = (int)DunGeonManager_New.instance.inGamePlayTime;

        string dungeonNumber = $"{stage}-{order}";
        //소울 보상 계산
        reward = resultTime * (3 + stage) * 2;
        //보상 지급
        PlayerPrefs.SetInt("Soul", PlayerPrefs.GetInt("Soul") + reward);

        dungeonNumber_Text.text = dungeonNumber;
        resultTime_Text.text = $"0s";
        reward_Text.text = $"0";
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
