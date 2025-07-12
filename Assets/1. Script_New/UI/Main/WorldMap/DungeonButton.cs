using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DungeonButton : MonoBehaviour
{
    public int stage;
    public DungeonButton nextStage_DungeonButton;

    public GameObject subButton_Go;

    public List<Image> star_Images;
    public List<Sprite> star_Sprites;

    private void Start()
    {
        CheckClear();
    }

    public void CheckClear()
    {
        int clearCount = 0;

        for (int i = 0; i < star_Images.Count; i++)
        {
            int number = i + 1;

            //클리어 정보(0 = 미클리어, 1 = 노란 별, 2 파란 별, 3 빛나는 별)
            int clear_Time = PlayerPrefs.GetInt(ReadOnlyData.dungeonClearTime + $"{stage},{number}");
            int clear_Rank = clear_Time == 0 ? 0 : clear_Time < 200 ? 3 : clear_Time < 300 ? 2 : 1;

            if (clear_Rank != 0)
            {
                star_Images[i].gameObject.SetActive(true);
                star_Images[i].sprite = star_Sprites[clear_Rank - 1];
                clearCount++;
            }
            else
                star_Images[i].gameObject.SetActive(false);
        }

        if (!nextStage_DungeonButton)
            return;

        //다음 스테이지 오픈
        if(clearCount >= 3)
        {
            nextStage_DungeonButton.ActiveButton(true);
            nextStage_DungeonButton.CheckClear();
        }
        else
            nextStage_DungeonButton.ActiveButton(false);
    }

    void ActiveButton(bool isActive)
    {
        GetComponent<Button>().interactable = isActive;
        subButton_Go.SetActive(isActive);
    }

    public void Test_ResetClearData()
    {
        for (int i = 0; i < star_Images.Count; i++)
        {
            int number = i + 1;
            PlayerPrefs.SetInt(ReadOnlyData.dungeonClearTime + $"{stage},{number}" , 0);
        }

        CheckClear();
    }
}
