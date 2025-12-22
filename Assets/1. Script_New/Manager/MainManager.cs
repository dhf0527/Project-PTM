using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public DungeonData[] dungeonDatas;
    public AreaPanel areaPanel;
    public Area_Page[] areaPages;
    public DungeonPanel dungeonPanel;

    public TMP_Text soul_Text;

    public int additionalMeal_cost;
    public int resetMeal_cost;
    public Image[] meal_Icons = new Image[2];
    public Image[] meal_Backgrounds = new Image[2];
    [Header("0고급, 1희귀, 2전설")]
    [SerializeField] List<Sprite> meal_BackgroundSprites;

    public TMP_Text floatingMessage;
    Coroutine c_Floating;
    bool isFloating;

    [HideInInspector] public bool isOpenStage = false;

    [HideInInspector] public MealData mealData;
    int cur_Stage_Index;
    int soul;
    public int Soul
    {
        get 
        { 
            return soul; 
        }
        set
        {
            soul = value;
            SetSoul();
            PlayerPrefs.SetInt("Soul", value);
        }

    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Soul = PlayerPrefs.GetInt("Soul");

        OnMeal();
    }

    //areaPanel에 던전 데이터 삽입
    public void OnSetDungeonDatas(int stage)
    {
        //스테이지는 1부터 시작하므로 인덱스로 변환
        int stageIndex = stage - 1;
        cur_Stage_Index = stageIndex;

        for (int i = 0; i < 3; i++)
        {
            int number = i + 1;
            areaPages[i].SetData(dungeonDatas[stageIndex * 3 + i]);
            if (!isOpenStage && i != 0 && PlayerPrefs.GetInt(ConstData.dungeonClearTime + $"{stage},{number - 1}") == 0)
                PageLock(i, true);
            else
                PageLock(i, false);
        }
        areaPanel.SetAreaPanelData(dungeonDatas[stageIndex * 3].stage_Faction);
    }

    //dungeonPanel에 던전 데이터 삽입
    public void OnSetDungeonPanel(int number)
    {
        dungeonPanel.SetData(dungeonDatas[cur_Stage_Index * 3 + number]);
    }

    void SetSoul()
    {
        soul_Text.text = Soul.ToString();
    }

    public void FloatMessage(string message)
    {
        if (!isFloating)
        {
            floatingMessage.text = message;
            StartCoroutine(C_FloatMessage());
        }
    }

    IEnumerator C_FloatMessage()
    {
        floatingMessage.transform.parent.gameObject.SetActive(true);
        isFloating = true;
        yield return new WaitForSeconds(2f);
        floatingMessage.transform.parent.gameObject.SetActive(false);
        isFloating = false;
    }

    public void OnMeal()
    {
        for (int i = 0; i < meal_Icons.Length; i++)
        {
            if (GameManager.Instance.applied_Meals[i])
            {
                meal_Icons[i].transform.parent.gameObject.SetActive(true);
                MealData md = GameManager.Instance.applied_Meals[i];
                meal_Icons[i].sprite = md.mealIcon;
                meal_Backgrounds[i].sprite = meal_BackgroundSprites[(int)md.mealRarity];
            }
            else
                meal_Icons[i].transform.parent.gameObject.SetActive(false);
        }
    }

    void PageLock(int number, bool isLock)
    {
        areaPages[number].GetComponent<Button>().interactable = !isLock;
        Color targetColor = isLock ? new Color(150 / 255f, 150 / 255f, 150 / 255f) : Color.white;

        foreach (var item in areaPages[number].GetComponentsInChildren<Image>())
            item.color = targetColor;
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
