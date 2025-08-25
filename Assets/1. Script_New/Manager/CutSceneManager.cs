using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;

    public GameObject canvas_2;
    public Image circleMask_Image;
    public Transform mask_trans;

    public List<GameObject> list_PointUI;
    int index;
    //강조된 UI
    GameObject point_go;
    List<Action> actions = new();

    Button target_Button;
    Toggle target_Toggle;

    public class DialogueData
    {
        public bool isRight;
        public bool isPause;
        public bool isSpeak;
        public string character_Speak;
        public CharacterData character_Show;
        [TextArea(0,3)]
        public string dialogue;
        public TutorialType tutorialType;
    }
    public List<CharacterData> characterDatas;

    public GameObject cutScene_Go;
    public Dialogue dialogue;
    public TMP_Text characterName_Text;
    public Image character_Image_Left;
    public Image character_Image_Right;

    Action completeFunc;

    List<DialogueData> list_Dialogue;
    int i = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //StartCutScene("1-1");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            StartCutScene("1-1");
        if (Input.GetKeyDown(KeyCode.T))
            StartTutorial("tutorial_1");
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Tutorial_Tmp();
            StartTutorial_New("tutorial_Tmp");
        }
    }

    //다음 대사 출력
    public void PrintNextDialogue()
    {
        if (point_go)
        {
            Destroy(point_go);
            point_go = null;
        }

        //마지막 문장일때
        if (i == list_Dialogue.Count)
        {
            //컷씬 종료
            cutScene_Go.SetActive(false);
            EndCutScene();
            return;
        }

        if (list_Dialogue[i].isPause)
            Time.timeScale = 0;
        else
            Time.timeScale = DunGeonManager_New.instance.isFasty ? 2 : 1;

        if (list_Dialogue[i].isSpeak)
        {
            cutScene_Go.SetActive(true); 
            canvas_2.SetActive(false);

            //대사 출력
            dialogue.StartTypeText(list_Dialogue[i].dialogue);
            //인물 이름 변경
            characterName_Text.text = list_Dialogue[i].character_Speak;

            Image target_Image;
            //방향 설정
            if (list_Dialogue[i].isRight)
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
            //스프라이트 변경
            target_Image.sprite = list_Dialogue[i].character_Show.characterSprite;
        }
        else
        {
            cutScene_Go.SetActive(false);
            actions[index++].Invoke();
            //PointUI(list_PointUI[index]);
        }

        i++;
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
        i = 0;
        index = 0;
        completeFunc?.Invoke();

        canvas_2.SetActive(false);
    }

    public void StartCutScene(string eventName, Action onComplete = null)
    {
        canvas_2.SetActive(true);

        list_Dialogue = GetDialogueDatasByCsv(eventName);
        cutScene_Go.SetActive(true);
        PrintNextDialogue();

        completeFunc = onComplete;
    }

    public void StartTutorial(string eventName, Action onComplete = null)
    {
        list_Dialogue = GetTutorialDatasByCsv(eventName);
        cutScene_Go.SetActive(true);
        PrintNextDialogue();
    }

    //특정 UI를 강조하는 함수
    public void PointUI(GameObject target_go)
    {
        canvas_2.SetActive(true);
        /*
        point_go = Instantiate(target_go, target_go.transform.parent);
        point_go.transform.SetParent(mask_trans);
        point_go.GetComponent<Button>().onClick.AddListener(PrintNextDialogue);
        */
        Canvas pointUi_Canvas = target_go.AddComponent<Canvas>();
        pointUi_Canvas.overrideSorting = true;
        pointUi_Canvas.sortingOrder = 2;
        target_go.AddComponent<GraphicRaycaster>();

       
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
            circleMask_Image.transform.position = target_go.transform.position;

            RectTransform target_RectTransform = target_go.GetComponent<RectTransform>();
            Vector2 worldSize = target_RectTransform.rect.size * 1.5f;
            circleMask_Image.rectTransform.sizeDelta = new Vector2(worldSize.x, worldSize.y);
        }
    }

    void ListenerPrintNextDialogue()
    {
        PrintNextDialogue();
        target_Button.onClick.RemoveListener(ListenerPrintNextDialogue);
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
                isRight = values[1].Trim() == "오른쪽",
                character_Speak = values[2],
                character_Show = NameToEnum(values[3].Trim()),
                dialogue = values[7],
                isSpeak = true,
                isPause = true
            };
            dds.Add(dd);
        }

        return dds;
    }

    //csv파일에서 튜토리얼 데이터를 읽어오는 함수
    public List<DialogueData> GetTutorialDatasByCsv(string fileName)
    {
        List<DialogueData> dds = new List<DialogueData>();

        //Assets\Resources\CSV 폴더에 있는 fileName.csv 읽어오기
        TextAsset csvFile = Resources.Load<TextAsset>("CSV/" + fileName);

        //csv파일의 데이터를 한 줄씩 저장
        string[] lines = csvFile.text.Split('\n');

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
                isPause = !(values[2].Trim() == "X"),
                isSpeak = !(values[3].Trim() == "X"),
                //tutorialType = StringToTutorialType(values[7]),
                isRight = values[6].Trim() == "오른쪽",
                character_Speak = values[8],
                character_Show = NameToEnum(values[9].Trim()),
                dialogue = values[10] + "\n" + values[11] + "\n" + values[12]
            };
            dds.Add(dd);
        }

        return dds;
    }

    //캐릭터 이름을 string -> enum 바꿔주는 함수(매칭되지 않으면 0번째 반환)
    CharacterData NameToEnum(string inputName)
    {
        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i].characterName == inputName)
                return characterDatas[i];
        }
        return null;
    }

    TutorialType StringToTutorialType(string input)
    {
        switch (input)
        {
            case "대기":
                return TutorialType.Wait;
            case "골드 확보":
                return TutorialType.GetGold;
            case "오브젝트 활성화":
                return TutorialType.ObjectActive;
            case "오브젝트 비활성화":
                return TutorialType.ObjectInactive;
            default:
                return TutorialType.None;
        }
    }
    #endregion

    #region 리뉴얼
    public void StartTutorial_New(string eventName, Action onComplete = null)
    {
        list_Dialogue = GetTutorialDatasByCsv_New(eventName);
        cutScene_Go.SetActive(true);
        PrintNextDialogue();
    }

    //csv파일에서 튜토리얼 데이터를 읽어오는 함수
    public List<DialogueData> GetTutorialDatasByCsv_New(string fileName)
    {
        List<DialogueData> dds = new List<DialogueData>();

        //Assets\Resources\CSV 폴더에 있는 fileName.csv 읽어오기
        TextAsset csvFile = Resources.Load<TextAsset>("CSV/" + fileName);

        //csv파일의 데이터를 한 줄씩 저장
        string[] lines = csvFile.text.Split('\n');

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
                isPause = !(values[2].Trim() == "X"),
                isSpeak = !(values[3].Trim() == "X"),
                tutorialType = StringToTutorialType(values[7]),
                isRight = values[8].Trim() == "오른쪽",
                character_Speak = values[10],
                character_Show = NameToEnum(values[11].Trim()),
                dialogue = values[12] + "\n" + values[13] + "\n" + values[14]
            };
            dds.Add(dd);
        }

        return dds;
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
        yield return new WaitUntil(() => DunGeonManager_New.instance.Cur_Gold >= targetGold);
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

    void EventByTutorialType(TutorialType tutorialType)
    {
        switch (tutorialType)
        {
            case TutorialType.None:
                break;
            case TutorialType.PointUI:
                PointUI(list_PointUI[index]);
                break;
            case TutorialType.Wait:
                break;
            case TutorialType.GetGold:
                break;
            case TutorialType.ObjectActive:
                break;
            case TutorialType.ObjectInactive:
                break;
            default:
                break;
        }
    }

    #region tutorial_Tmp
    public void Tutorial_Tmp()
    {
        actions.Clear();

        actions.Add(() => TutorialWait(1f));    //1
        actions.Add(() => PointUI(list_PointUI[0]));    //5
        actions.Add(() => TutorialGetGold(100));    //10
        actions.Add(() => PointUI(list_PointUI[1]));    //11
        actions.Add(() => Tutorial_Tmp1());    //13
        actions.Add(() => TutorialGetGold(50));    //16
        actions.Add(() => PointUI(list_PointUI[2]));    //17
        actions.Add(() => Tutorial_Tmp2());    //21
        actions.Add(() => Tutorial_Tmp3());    //26
        actions.Add(() => Tutorial_Tmp4());    //27
        actions.Add(() => Tutorial_Tmp5());    //35
        actions.Add(() => Tutorial_Tmp4());    //36
        actions.Add(() => TutorialObjectActive(list_PointUI[3]));    //38
        actions.Add(() => TutorialWait(3f));    //39
        actions.Add(() => PointUI(list_PointUI[4]));    //43
    }

    void Tutorial_Tmp1()
    {
        //적 도적 유닛 등장
        //적 유닛에게로 시점 이동
        TutorialWait(10f);
    }
    void Tutorial_Tmp2()
    {
        StartCoroutine(C_Tutorial_Tmp2());
    }

    IEnumerator C_Tutorial_Tmp2()
    {
        yield return new WaitUntil(() => EnemySpawnManager.instance.cur_Wave > 0);
        PrintNextDialogue();
    }

    void Tutorial_Tmp3()
    {
        StartCoroutine(C_Tutorial_Tmp3());
    }

    IEnumerator C_Tutorial_Tmp3()
    {
        //보스 소환까지 대기
        yield return new WaitUntil(() => EnemySpawnManager.instance.bossUnit);
        PrintNextDialogue();
    }

    void Tutorial_Tmp4()
    {
        StartCoroutine(C_Tutorial_Tmp4());
    }

    IEnumerator C_Tutorial_Tmp4()
    {
        //보스에게로 시점 이동
        DunGeonManager_New.instance.cameraMove.isChasePrincess = false;
        DunGeonManager_New.instance.cameraMove.MoveCamera(EnemySpawnManager.instance.bossUnit.transform.position.x);
        yield return new WaitForSecondsRealtime(3f);
        DunGeonManager_New.instance.cameraMove.isChasePrincess = true;
        PrintNextDialogue();
    }

    void Tutorial_Tmp5()
    {
        //보스 처치까지 대기
        StartCoroutine(C_Tutorial_Tmp5());
    }

    IEnumerator C_Tutorial_Tmp5()
    {
        //보스 소환까지 대기
        yield return new WaitUntil(() => EnemySpawnManager.instance.isBossDead);
        PrintNextDialogue();
    }


    #endregion
    #endregion
}
