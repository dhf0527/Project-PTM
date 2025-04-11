using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueData
    {
        public bool isRight;
        public CharacterName character_Speak;
        public CharacterName character_Show;
        [TextArea(0,3)]
        public string dialogue;
    }
    public List<DialogueData> list_Dialogue;
    public List<CharacterData> characterDatas;

    public GameObject cutScene_Go;
    public Dialogue dialogue;
    public TMP_Text characterName_Text;
    public Image character_Image_Left;
    public Image character_Image_Right;

    int i = 0;

    private void Start()
    {
        cutScene_Go.SetActive(true);
        PrintNextDialogue();
    }

    private void Update()
    {
        if(i <= list_Dialogue.Count && Input.GetMouseButtonDown(0))
        {
            dialogue.ToNextTypeText();
        }
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
        characterName_Text.text = characterDatas[(int)list_Dialogue[i].character_Speak].characterName;

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
        target_Image.sprite = characterDatas[(int)list_Dialogue[i].character_Show].characterSprite;
        i++;
    }
}
