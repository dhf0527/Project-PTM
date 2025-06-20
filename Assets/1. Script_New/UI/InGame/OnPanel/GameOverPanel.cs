using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] TMP_Text dungeonNumber_Text;
    [SerializeField] TMP_Text resultTime_Text;
    [SerializeField] Image rank_Image;
    [SerializeField] TMP_Text reward_Text;

    [SerializeField] Sprite[] rank_Sprites;

    public void SetClearPanel()
    {
        //임시
        int stage = 1;
        int order = 1;
        int resultTime = (int)DunGeonManager_New.instance.inGamePlayTime;

        string dungeonNumber = $"{stage}-{order}";
        //소울 보상 계산
        int reward = resultTime * (3 + stage);

        dungeonNumber_Text.text = dungeonNumber;
        resultTime_Text.text = $"{resultTime}s";
        reward_Text.text = $"{reward}";

    }
}
