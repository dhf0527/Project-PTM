using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    public GameObject mapPanel;
    public GameObject upgradePanel;
    public GameObject areaPanel;
    public GameObject dungeonPanel;

    //dungeon data
    public Dungeon[] dungeonDatas;
    private int selectAreaId;
    //area
    [Header("Area")]
    public Transform areaBase;
    public GameObject[] areaTopUIPrefabs;
    public Image[] areaBossImage;
    public GameObject[] areaClearStar;
    public Text[] areaDungeonNum;

    [Header("Dungeon")]
    public Transform dungeonBase;
    public GameObject dungeonTopUI;
    public Text dungeonNumText;
    public Image[] dungeonUnits;
    public Image dungeonBossImage;
    public Sprite[] dungeonUnitBaseImage;
    public Image[] dungeonUnitBase;
    public Text dungeonHighScoreText;
    public GameObject[] areaClearStars;
    public GameObject[] clearStarImages;

    public ChangeSceneManager changeSceneManager;

    private void Start()
    {

        for (int i = 0; i < areaClearStars.Length; i++)
        {
            int clearNum = PlayerPrefs.GetInt("dungeonClear" +(i+1), 0);
            if (clearNum == 1) //클리어 했을때
            {
                areaClearStars[i].SetActive(true);
            }
            else
            {
                areaClearStars[i].SetActive(false);
            }

            //
            if (PlayerPrefs.GetInt("dungeonClear" + (i + 1), 0) == 1)
            {
                dungeonDatas[i].isClear = true;
            }
        }
    }
    public void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        upgradePanel?.SetActive(false);
    }

    //
    public void OpenAreaPanel(int type)
    {
        DataManager.currentArea = type;
        //top
        bool isClearTable = false;
        GameObject go = Instantiate(areaTopUIPrefabs[type]);
        go.transform.SetParent(areaBase,false);
        areaTop areaTopScript = go.GetComponent<areaTop>();
        areaTopScript.isDisable = true;
        //items
        for (int i = 0; i < areaDungeonNum.Length; i++) //스크롤 하나씩 돌음
        {
            
            areaDungeonNum[i].text = (type + 1) + "-" + (i+1);
            switch (type)
            {
                case 0:
                    areaBossImage[i].sprite = dungeonDatas[i].dungeonBoss.itemIcon;
                    isClearTable = dungeonDatas[i].isClear ? true : false;
                    selectAreaId = 0;
                    break;

                case 1: //i가 3,4,5여야함
                    areaBossImage[i].sprite = dungeonDatas[i+3].dungeonBoss.itemIcon;
                    isClearTable = dungeonDatas[i+3].isClear ? true : false;
                    selectAreaId = 1;
                    break;

                case 2://i가 6,7,8여야함
                    areaBossImage[i].sprite = dungeonDatas[i+6].dungeonBoss.itemIcon;
                    isClearTable = dungeonDatas[i+6].isClear ? true : false;
                    selectAreaId = 2;
                    break;

                case 3://i가 9,10,11여야함
                    areaBossImage[i].sprite = dungeonDatas[i+9].dungeonBoss.itemIcon;
                    isClearTable = dungeonDatas[i+9].isClear ? true : false;
                    selectAreaId = 3;
                    break;
            }

            areaClearStar[i].SetActive(isClearTable);
        }

        areaPanel.SetActive(true);
    }

    public void CloseAreaPanel()
    {
        areaPanel.SetActive(false);
    }

    public void OpenDungeonPanel(int dunNum)
    {
        DataManager.currentDungeon = (DataManager.currentArea * 3) + dunNum;
        Debug.Log(DataManager.currentDungeon);
        //CloseAreaPanel();
        if (PlayerPrefs.GetInt("dungeonClear" + DataManager.currentDungeon) == 1)
        {
            if (PlayerPrefs.GetInt("dungeonTime" + DataManager.currentDungeon) >= 450f)
            {
                clearStarImages[0].SetActive(true);
                clearStarImages[1].SetActive(false);
                clearStarImages[2].SetActive(false);
            }
            else if (PlayerPrefs.GetInt("dungeonTime" + DataManager.currentDungeon) < 450f && PlayerPrefs.GetInt("dungeonTime" + DataManager.currentDungeon) > 200f)
            {
                clearStarImages[0].SetActive(false);
                clearStarImages[1].SetActive(true);
                clearStarImages[2].SetActive(false);
            }
            else if (PlayerPrefs.GetInt("dungeonTime" + DataManager.currentDungeon) <= 200f)
            {
                clearStarImages[0].SetActive(false);
                clearStarImages[1].SetActive(false);
                clearStarImages[2].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < clearStarImages.Length; i++)
            {
                clearStarImages[i].SetActive(false);
            }
        }
        dungeonHighScoreText.text = PlayerPrefs.GetInt("dungeonTime"+ DataManager.currentDungeon,0) + " sec ";
        dungeonNumText.text = selectAreaId+1 + " - " + dunNum;
        dungeonBossImage.sprite = dungeonDatas[selectAreaId*3 + dunNum-1].dungeonBoss.itemIcon;
        for (int i = 0; i < dungeonUnits.Length; i++)
        {
            if (dungeonDatas[selectAreaId * 3 + dunNum - 1].dungeonUnits[i] == null)
            {
                dungeonUnits[i].sprite = null;
                dungeonUnitBase[i].sprite = dungeonUnitBaseImage[4];
                dungeonUnits[i].color = new Color(0, 0, 0, 0);
            }
            else
            {
                dungeonUnitBase[i].sprite = dungeonUnitBaseImage[dungeonDatas[selectAreaId * 3 + dunNum - 1].dungeonUnits[i].member];
                dungeonUnits[i].color = new Color(1, 1, 1, 1);
                dungeonUnits[i].sprite = dungeonDatas[selectAreaId * 3 + dunNum - 1].dungeonUnits[i].itemIcon;
            }
        }
        GameObject go = Instantiate(areaTopUIPrefabs[DataManager.currentArea]);
        go.transform.SetParent(dungeonBase, false);
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 310, 0);
        areaTop areaTopScript = go.GetComponent<areaTop>();
        areaTopScript.isDisable = true;
        //
        dungeonPanel.SetActive(true);
    }

    public void CloseDungeonPanel()
    {
        dungeonPanel.SetActive(false);
    }

    public void GameStart()
    {
        switch (DataManager.currentArea)
        {
            case 0:
                changeSceneManager.GoToDungeon1();
                break;
                
            case 1:
                changeSceneManager.GoToDungeon2();
                break;

            case 2:
                changeSceneManager.GoToDungeon3();
                break;

            case 3:
                changeSceneManager.GoToDungeon4();
                break;
        }
    }
}
