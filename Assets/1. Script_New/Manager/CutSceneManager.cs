using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public class DialogueData
    {
        public bool isRight;
        public CharacterData character_Speak;
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

    List<DialogueData> list_Dialogue;
    int i = 0;

    private void Start()
    {
        list_Dialogue = GetDialogueDatasByCsv("1-1");
        cutScene_Go.SetActive(true);
        PrintNextDialogue();
    }

    private void Update()
    {
        /*
        if(i <= list_Dialogue.Count && Input.GetMouseButtonDown(0))
        {
            dialogue.ToNextTypeText();
        }
        */
    }

    //다음 대사 출력
    public void PrintNextDialogue()
    {
        //마지막 문장일때
        if(i == list_Dialogue.Count)
        {
            //컷씬 종료
            cutScene_Go.SetActive(false);
            return;
        }
        
        //대사 출력
        dialogue.StartTypeText(list_Dialogue[i].dialogue);
        //인물 이름 변경
        characterName_Text.text = list_Dialogue[i].character_Speak.characterName;

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
        i++;
    }

    //스킵 버튼
    public void OnSkipButton()
    {
        dialogue.EndTyping();
        cutScene_Go.SetActive(false);
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
                character_Speak = NameToEnum(values[2].Trim()),
                character_Show = NameToEnum(values[3].Trim()),
                dialogue = values[7]
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
