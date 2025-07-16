using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;

    public GameObject canvas_2;
    public Transform mask_trans;

    public List<GameObject> list_Point;
    int index;
    //강조된 UI
    GameObject point_go;

    public class DialogueData
    {
        public bool isRight;
        public bool isPause;
        public bool isSpeak;
        public string character_Speak;
        public CharacterData character_Show;
        [TextArea(0,3)]
        public string dialogue;
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
            Time.timeScale = 1;

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
            PointUI(list_Point[index]);
            index++;
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
        point_go = Instantiate(target_go, target_go.transform.parent);
        point_go.transform.SetParent(mask_trans);
        point_go.GetComponent<Button>().onClick.AddListener(PrintNextDialogue);
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
            if(characterDatas[i].characterName == inputName)
                return characterDatas[i];
        }
        return null;
    }
    #endregion
}
