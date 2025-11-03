using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;

    public GameObject canvas_2;
    public Image circleMask_Image;
    public Transform mask_trans;
    public GameObject touchBlocker;

    int action_index;
    List<Action> actions = new();
    bool isTutorial;
    TutorialKey cur_TutorialKey;

    Button target_Button;
    Toggle target_Toggle;

    public class DialogueData
    {
        public bool isRight;
        public bool isPause;
        public bool isSpeak;
        public string character_Speak;
        public Sprite character_Sprite;
        [TextArea(0,3)]
        public string dialogue;
    }

    public GameObject cutScene_Go;
    public Dialogue dialogue;
    public TMP_Text characterName_Text;
    public Image character_Image_Left;
    public Image character_Image_Right;

    List<DialogueData> list_Dialogue;
    int dialogue_index = 0;

    private void Awake()
    {
        instance = this;
        
         //튜토리얼
        if(SceneManager.GetActiveScene().name == "Dungeon")
            CheckTutorial_DungeonScene();
        else if(SceneManager.GetActiveScene().name == "MainScene" && !UnlockManager.instance.notification.activeInHierarchy)
            CheckTutorial_MainScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCutScene("1-1");
        }
    }

    public void CheckTutorial_DungeonScene()
    {
        if (GameManager.Instance.current_Dungeon.stage == 1 && GameManager.Instance.current_Dungeon.number == 1 && PlayerPrefs.GetInt(ConstData.tutorialComplete + TutorialKey.Dungeon_1) == 0)
            Tutorial_Dungeon_1();
        else if (GameManager.Instance.current_Dungeon.stage == 1 && GameManager.Instance.current_Dungeon.number == 2 && PlayerPrefs.GetInt(ConstData.tutorialComplete + TutorialKey.Dungeon_2) == 0)
            Tutorial_Dungeon_2();
    }

    public void CheckTutorial_MainScene()
    {
        if (PlayerPrefs.GetInt(ConstData.tutorialComplete + TutorialKey.WorldMap_1) == 0)
            Tutorial_WorldMap_1();
        else if (PlayerPrefs.GetInt(ConstData.dungeonClearTime + $"{1},{2}") != 0 && PlayerPrefs.GetInt(ConstData.tutorialComplete + TutorialKey.WorldMap_2) == 0)
            Tutorial_WorldMap_2();
        else if (PlayerPrefs.GetInt(ConstData.dungeonClearTime + $"{1},{3}") != 0 && PlayerPrefs.GetInt(ConstData.tutorialComplete + TutorialKey.WorldMap_3) == 0)
            Tutorial_WorldMap_3();
    }

    //다음 대사 출력
    public void PrintNextDialogue()
    {
        //마지막 문장일때
        if (dialogue_index == list_Dialogue.Count)
        {
            //컷씬 종료
            cutScene_Go.SetActive(false);
            EndCutScene();
            return;
        }

        if (list_Dialogue[dialogue_index].isPause)
            Time.timeScale = 0;
        else
        {
            if (DunGeonManager_New.instance)
            {
                if(DunGeonManager_New.instance.pauseStack == 0)
                    Time.timeScale = DunGeonManager_New.instance.isFasty ? DunGeonManager_New.instance.fastValue : 1;
            }
            else
                Time.timeScale = 1;
        }

        if (list_Dialogue[dialogue_index].isSpeak)
        {
            cutScene_Go.SetActive(true); 
            canvas_2.SetActive(false);

            //대사 출력
            dialogue.StartTypeText(list_Dialogue[dialogue_index].dialogue);
            //인물 이름 변경
            characterName_Text.text = list_Dialogue[dialogue_index].character_Speak;

            Image target_Image;
            //방향 설정
            if (list_Dialogue[dialogue_index].isRight)
            {
                //오른쪽
                target_Image = character_Image_Right;
                character_Image_Right.gameObject.SetActive(true);
                character_Image_Left.gameObject.SetActive(false);
            }
            else
            {
                //왼쪽
                target_Image = character_Image_Left;
                character_Image_Right.gameObject.SetActive(false);
                character_Image_Left.gameObject.SetActive(true);
            }
                target_Image.sprite = list_Dialogue[dialogue_index].character_Sprite;
        }
        else
        {
            cutScene_Go.SetActive(false);
            actions[action_index++].Invoke();
        }

        dialogue_index++;
    }

    //스킵 버튼
    public void OnSkipButton()
    {
        dialogue.EndTyping();
        EndCutScene();
        cutScene_Go.SetActive(false);

        AudioManager.Instance.PlayerSfx(SFX_Enum.Touch);
    }

    public void EndCutScene()
    {
        dialogue_index = 0;
        action_index = 0;
        //컷씬 열람 기록
        PlayerPrefs.SetInt(ConstData.tutorialComplete + cur_TutorialKey, 1);
        canvas_2.SetActive(false);

        if (DunGeonManager_New.instance)
        {
            if (DunGeonManager_New.instance.pauseStack == 0)
                Time.timeScale = DunGeonManager_New.instance.isFasty ? DunGeonManager_New.instance.fastValue : 1;
        }
        else
            Time.timeScale = 1;
    }

    public void StartCutScene(string eventName)
    {
        isTutorial = false;
        canvas_2.SetActive(true);

        list_Dialogue = GetDialogueDatasByCsv(eventName);
        cutScene_Go.SetActive(true);
        PrintNextDialogue();

    }

    //특정 UI를 강조하는 함수
    public void PointUI(GameObject target_go)
    {
        canvas_2.SetActive(true);
        Canvas pointUi_Canvas = target_go.AddComponent<Canvas>();
        pointUi_Canvas.overrideSorting = true;
        pointUi_Canvas.sortingOrder = 2;
        pointUi_Canvas.sortingLayerID = SortingLayer.NameToID("UI");
        pointUi_Canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
        target_go.AddComponent<GraphicRaycaster>();

        target_Button = null;
        if (target_go.TryGetComponent(out target_Button))
        {
            target_Button.onClick.AddListener(ListenerPrintNextDialogue);
            target_Button.onClick.AddListener(() => Destroy(target_go.GetComponent<GraphicRaycaster>()));
            target_Button.onClick.AddListener(() => Destroy(pointUi_Canvas));
        }
        else if (target_go.TryGetComponent(out target_Toggle))
        {
            target_Toggle.onValueChanged.AddListener(ToggleListenerPrintNextDialogue);
            target_Toggle.onValueChanged.AddListener((_) => Destroy(target_go.GetComponent<GraphicRaycaster>()));
            target_Toggle.onValueChanged.AddListener((_) => Destroy(pointUi_Canvas));
        }
        else
        {
            target_Button = target_go.AddComponent<Button>();
            target_Button.onClick.AddListener(ListenerPrintNextDialogue);
            target_Button.onClick.AddListener(() => Destroy(target_go.GetComponent<GraphicRaycaster>()));
            target_Button.onClick.AddListener(() => Destroy(pointUi_Canvas));
        }

        if (circleMask_Image)
        {
            circleMask_Image.gameObject.SetActive(true);
            circleMask_Image.transform.position = target_go.transform.position;

            RectTransform target_RectTransform = target_go.GetComponent<RectTransform>();
            Vector2 worldSize = target_RectTransform.rect.size * 1.5f;
            circleMask_Image.rectTransform.sizeDelta = new Vector2(worldSize.x, worldSize.y);

            circleMask_Image.rectTransform.localPosition += new Vector3(target_RectTransform.rect.size.x * (0.5f - target_RectTransform.pivot.x), target_RectTransform.rect.size.y * (0.5f - target_RectTransform.pivot.y), 0);
        }
    }

    void ListenerPrintNextDialogue()
    {
        target_Button.onClick.RemoveListener(ListenerPrintNextDialogue);
        circleMask_Image?.gameObject.SetActive(false);
        PrintNextDialogue();
    }
    void ToggleListenerPrintNextDialogue(bool isOn)
    {
        PrintNextDialogue();
        target_Toggle.onValueChanged.RemoveListener(ToggleListenerPrintNextDialogue);
    }

    #region CSV 함수

    //csv파일에서 대사 데이터를 읽어오는 함수
    public List<DialogueData> GetDialogueDatasByCsv(string fileName)
    {
        List<DialogueData> dds = new List<DialogueData>();

        //Assets\Resources\CSV 폴더에 있는 fileName.csv 읽어오기
        TextAsset csvFile = Resources.Load<TextAsset>("CSV/" + fileName);

        //csv파일의 데이터를 한 줄씩 저장
        string[]lines = csvFile.text.Split('\n');

        //첫 줄은 헤더이므로 i=1
        for (int i = 1; i < lines.Length; i++)
        {
            //공백 건너뛰기
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            //따옴표(")사이에 있는 ,를 제외하고 ,를 기준으로 나누기
            string[] values = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            //따옴표 제거
            for (int j = 0; j < values.Length; j++)
                values[j] = values[j].Trim().Trim('"');

            DialogueData dd = new DialogueData
            {
                isRight = values[2].Trim() == "오른쪽",
                character_Speak = values[3],
                character_Sprite = Resources.Load<Sprite>("Sprites/Stand/" + "Stand_" + values[4]),
                dialogue = values[5] + "\n" + values[6] + "\n" + values[7],
                isSpeak = true,
                isPause = true
            };
            dds.Add(dd);
        }

        return dds;
    }
    #endregion

    #region 튜토리얼
    public void StartTutorial(string eventName, Action onComplete = null)
    {
        isTutorial = true;
        list_Dialogue = GetTutorialDatasByCsv(eventName);
        cutScene_Go.SetActive(true);
        PrintNextDialogue();
    }

    //csv파일에서 튜토리얼 데이터를 읽어오는 함수
    public List<DialogueData> GetTutorialDatasByCsv(string fileName)
    {
        List<DialogueData> dds = new List<DialogueData>();

        //Assets\Resources\CSV 폴더에 있는 fileName.csv 읽어오기
        TextAsset csvFile = Resources.Load<TextAsset>("CSV/" + fileName);

        //csv파일의 데이터를 한 줄씩 저장
        string[] lines = csvFile.text.Split('\n');

        //1,2줄은 헤더이므로 i=2
        for (int i = 2; i < lines.Length; i++)
        {
            //공백 건너뛰기
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            //따옴표(")사이에 있는 ,를 제외하고 ,를 기준으로 나누기
            string[] values = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            //따옴표 제거
            for (int j = 0; j < values.Length; j++)
                values[j] = values[j].Trim().Trim('"');

            DialogueData dd = new DialogueData
            {
                isPause = !(values[3].Trim() == "X"),
                isSpeak = !(values[4].Trim() == "X"),
                isRight = values[9].Trim() == "오른쪽",
                character_Speak = values[10],
                character_Sprite = Resources.Load<Sprite>("Sprites/Stand/" + "Stand_" + values[11]),
                dialogue = ReplaceDialogue(values[12]) + "\n" + ReplaceDialogue(values[13]) + "\n" + ReplaceDialogue(values[14])
            };
            dds.Add(dd);
        }

        return dds;
    }

    string ReplaceDialogue(string input)
    {
        foreach (var words_Point in FindWords_Point(input))
            input = Regex.Replace(input, $"\\*{words_Point}\\*", $"<color=yellow>{words_Point}</color>");

        return input;
    }

    List<string> FindWords_Point(string input_Text)
    {
        List<string> words = new List<string>();

        MatchCollection matches = Regex.Matches(input_Text, "\\*(.*?)\\*");

        foreach (Match match in matches)
            words.Add(match.Groups[1].Value);

        return words;
    }

    void TutorialWait(float seconds)
    {
        StartCoroutine(C_TutorialWait(seconds));
    }

    IEnumerator C_TutorialWait(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        PrintNextDialogue();
    }

    void TutorialGetGold(int targetGold)
    {
        StartCoroutine(C_TutorialGetGold(targetGold));
    }
    
    IEnumerator C_TutorialGetGold(int targetGold)
    {
        GameObject goldPanel = SearchManager.Instance.Search(SearchKey.GoldPanel);

        DunGeonManager_New.instance.pauseMask.SetActive(true);

        Canvas pointUi_Canvas = goldPanel.AddComponent<Canvas>();
        pointUi_Canvas.overrideSorting = true;
        pointUi_Canvas.sortingOrder = 2;
        pointUi_Canvas.sortingLayerID = SortingLayer.NameToID("UI");
        pointUi_Canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;

        float origin_TimeScale = Time.timeScale;
        Time.timeScale = 3f;
        yield return new WaitUntil(() => DunGeonManager_New.instance.Cur_Gold >= targetGold);

        Time.timeScale = origin_TimeScale;
        DunGeonManager_New.instance.pauseMask.SetActive(false);
        Destroy(pointUi_Canvas);

        PrintNextDialogue();

    }

    void TutorialObjectActive(GameObject go)
    {
        StartCoroutine(C_TutorialObjectActive(go));
    }

    IEnumerator C_TutorialObjectActive(GameObject go)
    {
        yield return new WaitUntil(() => go.activeInHierarchy);
        PrintNextDialogue();
    }

    void TutorialObjectInActive(GameObject go)
    {
        StartCoroutine(C_TutorialObjectInActive(go));
    }

    IEnumerator C_TutorialObjectInActive(GameObject go)
    {
        yield return new WaitUntil(() => !go.activeInHierarchy);
        PrintNextDialogue();
    }

    #region Tutorial_WorldMap_1
    public void Tutorial_WorldMap_1()
    {
        actions.Clear();

        actions.Add(() => PointUI(SearchManager.Instance.Search(SearchKey.Dungeon1))); //3
        actions.Add(() => PointUI(MainManager.instance.areaPages[0].gameObject)); //4
        actions.Add(() => PointUI(SearchManager.Instance.Search(SearchKey.GameStartButton))); //7

        StartTutorial("Tutorial_WorldMap_1");
        cur_TutorialKey = TutorialKey.WorldMap_1;
    }

    #endregion
    #region Tutorial_WorldMap_2
    public void Tutorial_WorldMap_2()
    {
        actions.Clear();

        actions.Add(() => PointUI(SearchManager.Instance.Search(SearchKey.Dungeon1))); //3
        actions.Add(() => PointUI(MainManager.instance.areaPages[1].gameObject)); //3
        actions.Add(() => Tutorial_WorldMap_2_0()); //3
        actions.Add(() => PointUI(UnlockManager.instance.heroUpgrade_Button)); //7
        actions.Add(() => Tutorial_WorldMap_2_1()); //8 모든 능력치 버튼 강조
        actions.Add(() => PointUI(UpGrade.instance.confirmButton)); //9

        StartTutorial("Tutorial_WorldMap_2");
        cur_TutorialKey = TutorialKey.WorldMap_2;

    }

    void Tutorial_WorldMap_2_0()
    {
        StartCoroutine(C_Tutorial_WorldMap_2_0());
    }

    IEnumerator C_Tutorial_WorldMap_2_0()
    {
        touchBlocker.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        touchBlocker.gameObject.SetActive(false);
        PrintNextDialogue();
    }

    void Tutorial_WorldMap_2_1()
    {
        canvas_2.SetActive(true);
        circleMask_Image.gameObject.SetActive(true);
        circleMask_Image.rectTransform.sizeDelta = Vector2.zero;

        for (int i = 0; i < UpGrade.instance.statBars.Count; i++)
        {
            Button upgradeButton = UpGrade.instance.statBars[i].upgradeButton;
            GameObject upgradeButton_go = upgradeButton.gameObject;

            canvas_2.SetActive(true);
            Canvas pointUi_Canvas = upgradeButton_go.AddComponent<Canvas>();
            pointUi_Canvas.overrideSorting = true;
            pointUi_Canvas.sortingOrder = 2;
            pointUi_Canvas.sortingLayerID = SortingLayer.NameToID("UI");
            pointUi_Canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
            upgradeButton_go.AddComponent<GraphicRaycaster>();

            {
                upgradeButton.onClick.AddListener(ListenerTutorial_WorldMap_2_1);
                upgradeButton.onClick.AddListener(() => Destroy(upgradeButton_go.GetComponent<GraphicRaycaster>()));
                upgradeButton.onClick.AddListener(() => Destroy(pointUi_Canvas));
            }
        }
    }

    void ListenerTutorial_WorldMap_2_1()
    {
        PrintNextDialogue();
        for (int i = 0; i < UpGrade.instance.statBars.Count; i++)
        {
            Button upgradeButton = UpGrade.instance.statBars[i].upgradeButton;
            upgradeButton.onClick.RemoveListener(ListenerTutorial_WorldMap_2_1);
            Destroy(upgradeButton.GetComponent<GraphicRaycaster>());
            Destroy(upgradeButton.GetComponent<Canvas>());
        }
    }

    #endregion
    #region Tutorial_WorldMap_3
    public void Tutorial_WorldMap_3()
    {
        actions.Clear();

        StartTutorial("Tutorial_WorldMap_3");
        cur_TutorialKey = TutorialKey.WorldMap_3;

    }
    #endregion

    #region Tutorial_Dungeon_1
    public void Tutorial_Dungeon_1()
    {
        DunGeonManager_New.instance.isTutorial_1 = true;
        actions.Clear();

        //actions.Add(() => TutorialWait(1f));    //1
        actions.Add(() => Tutorial_Dungeon1_0());
        actions.Add(() => PointUI(DunGeonManager_New.instance.unitUnlock.cards[1].gameObject));    //5     검사 클릭 버튼
        actions.Add(() => PointUI(DunGeonManager_New.instance.unitUnlock.select_Button.gameObject));    //6     확정 버튼
        actions.Add(() => TutorialGetGold(100));    //11
        actions.Add(() => PointUI(SearchManager.Instance.Search(SearchKey.UnitSpawnButton1)));    //12    검사 고용 버튼
        actions.Add(() => TutorialGetGold(50));    //16
        actions.Add(() => PointUI(SearchManager.Instance.Search(SearchKey.BaseLevelUpButton)));    //17    요새 레벨업 버튼
        actions.Add(() => Tutorial_Dungeon1_1());    //21   웨이브2 대기
        actions.Add(() => Tutorial_Dungeon_1_2());    //26  보스 대기
        actions.Add(() => Tutorial_Dungeon_1_3());    //28  보스로 시점 이동
        actions.Add(() => Tutorial_Dungeon_1_4());    //35  보스 처치 대기
        actions.Add(() => TutorialWait(2f));    //36
        actions.Add(() => TutorialObjectActive(DunGeonManager_New.instance.GameClearPanel.gameObject));    //38   전투 결과 화면 대기
        actions.Add(() => TutorialWait(2f));    //40

        StartTutorial("Tutorial_Dungeon_1");
        cur_TutorialKey = TutorialKey.Dungeon_1;
    }

    void Tutorial_Dungeon1_0()
    {
        StartCoroutine(C_Tutorial_Dungeon1_0());
    }

    IEnumerator C_Tutorial_Dungeon1_0()
    {
        touchBlocker.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        touchBlocker.gameObject.SetActive(false);
        PrintNextDialogue();
    }

    //웨이브2 대기
    void Tutorial_Dungeon1_1()
    {
        StartCoroutine(C_Tutorial_Dungeon_1_1());
    }

    IEnumerator C_Tutorial_Dungeon_1_1()
    {
        yield return new WaitUntil(() => EnemySpawnManager.instance.cur_Wave > 0);
        PrintNextDialogue();
    }

    //보스 대기
    void Tutorial_Dungeon_1_2()
    {
        StartCoroutine(C_Tutorial_Dungeon_1_2());
    }

    IEnumerator C_Tutorial_Dungeon_1_2()
    {
        //보스 소환까지 대기
        yield return new WaitUntil(() => EnemySpawnManager.instance.bossUnit);
        PrintNextDialogue();
    }

    //보스로 시점 이동
    void Tutorial_Dungeon_1_3()
    {
        StartCoroutine(C_Tutorial_Dungeon_1_3());
    }

    IEnumerator C_Tutorial_Dungeon_1_3()
    {
        //보스에게로 시점 이동
        DunGeonManager_New.instance.cameraMove.isChasePrincess = false;
        DunGeonManager_New.instance.cameraMove.MoveCamera(EnemySpawnManager.instance.bossUnit.transform.position.x);
        yield return new WaitForSecondsRealtime(3f);
        DunGeonManager_New.instance.cameraMove.isChasePrincess = true;
        PrintNextDialogue();
    }

    //보스 처치 대기
    void Tutorial_Dungeon_1_4()
    {
        StartCoroutine(C_Tutorial_Dungeon_1_4());
    }

    IEnumerator C_Tutorial_Dungeon_1_4()
    {
        //보스 소환까지 대기
        yield return new WaitUntil(() => EnemySpawnManager.instance.isBossDead);
        PrintNextDialogue();
    }


    #endregion
    #region Tutorial_Dungeon_2
    public void Tutorial_Dungeon_2()
    {
        DunGeonManager_New.instance.isTutorial_2 = true;
        actions.Clear();

        actions.Add(() => TutorialObjectActive(DunGeonManager_New.instance.unitUnlock.gameObject));    //1  용병 모집 대기
        actions.Add(() => PointUI(DunGeonManager_New.instance.unitUnlock.cards[1].gameObject));    //7     슬라임 클릭 대기
        actions.Add(() => PointUI(SearchManager.Instance.Search(SearchKey.CardSelectButton)));    //8     확정 버튼 클릭 대기
        actions.Add(() => Tutorial_Dungeon2_1());    //11    웨이브2 대기
        actions.Add(() => PointUI(DunGeonManager_New.instance.unitUnlock.cards[0].gameObject));    //16     골렘 클릭 대기
        actions.Add(() => PointUI(DunGeonManager_New.instance.unitUnlock.detail_Button.gameObject));    //17    상세 확인 버튼 클릭 대기
        actions.Add(() => TutorialWait(1f));    //18
        actions.Add(() => Tutorial_Dungeon_1_2());    //24      

        StartTutorial("Tutorial_Dungeon_2");
        cur_TutorialKey = TutorialKey.Dungeon_2;
    }

    //웨이브2 대기
    void Tutorial_Dungeon2_1()
    {
        StartCoroutine(C_Tutorial_Dungeon_2_1());
    }

    IEnumerator C_Tutorial_Dungeon_2_1()
    {
        yield return new WaitUntil(() => EnemySpawnManager.instance.cur_Wave > 0);
        PrintNextDialogue();
    }
    #endregion
    #endregion

}
