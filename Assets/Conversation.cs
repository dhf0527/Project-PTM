using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversation : MonoBehaviour
{
    [SerializeField]
    Button NextButton;

    [SerializeField]
    Text ScriptText;

    [SerializeField]
    Text NameText;

    [SerializeField]
    Sprite[] CharacterSprite;

    [SerializeField]
    int Scene;

    string[] ShowScene;
    string[] ShowSceneSprite;
    [SerializeField]
    int ScriptChangeIndex=0;

    public int CharPerSecond;
    int index;
    [SerializeField]
    Text TargetMsg;

    string[] Scene11 =
    { "모두가 거짓된 평화에 익숙해진 지금,\n세계를 구할 수 있는 건 칼리번, 너와 나뿐이야!",
    "푸흥...",
    "...그런 김빠지는 소리 하지 마!",
    "하지만 너 말이 맞아.\n우리 둘이서 마왕을 홀로 상대하는 건 아주 바보 같은 짓이야.",
    "모험가들을 모아야 돼!\n하지만 아무 모험가는 안돼.\n아주 엄격한 기준과 면접을 통해...",
    "아직 멀리 가진 못하셨을 것이다!\n공주님을 찾아라!",
    "헉...! 아바마마의 성기사단이다!\n벌써 날 찾고 계시나 봐!\n하지만 절대 내 말을 믿지 않을 게 뻔해...",
    "...에, 에잇! 이봐 거기 너!",
    "...네? 저요?",
    "그래 애매하게 주인공처럼 생긴 너!\n너 아까 길드 문 닫았다고 울고 있던 모험가지?",
    "아, 안 울었거든요!\n그리고 길드도 문 닫았으니 이제 모험가도 아니라구요.",
    "아무래도 됐고, 너\n오늘부터 용병해라.",
    "예? 용병이요?",
    "그래! 넌 이제 왕국의 공주가 인정한 왕실 용병이야! 축하해!\n자 이제 저 성기사를 막아!",
    "와! 저 왕실에서 일해보는 게 꿈이었어요!\n...잠깐, 누굴 막으라구요?",
    "공주님을 찾았다!\n도망 가시기 전에 잡아라!",
    "으앗...! 들켰다!\n아, 아무튼 넌 이제 내 용병이니까 어떻게든 아저씨를 막아!",
    "예?? 저건 왕국 성기사잖아요!\n제가 절대로 이길 수 없다구요!",
    "누가 너보고 성기사 아저씨를 쓰러트리래?\n시간만이라도 벌라구!",
    "모험가들이여! 도망치는 공주님을 잡아라!\n잡아오는 자에겐 보상금을 주겠다!",
    "뭐? 보상금??\n안 그래도 요즘 일도 없었는데 잘 됐네!",
    "아이고!\n괴팍한 공주한테 고용돼서 무의미하게 죽게 생겼네!!",
    "누가 괴팍하다는 거야, 이 겁쟁이가!\n...아무래도 현금을 좀 쥐여줘야 용병으로서의 자각이 좀 생기겠는걸.",
    "야! 이거 받아!",
    "와 돈이다!	누구냐!\n우리 고용주... 아니 공주님을 위협하는 녀석들이!",
    "음, 아주 괘씸하지만 조금은 모험가 다운 자세를 갖췄네.",
    "모험가! 도적떼를 잠시 막고 있어.\n내가 지원군을 모아올 테니까!!"
    };
    string[] SceneSprite11 = 
        {"케이","칼리번","케이","케이","케이",
    "성기사","케이","케이","검사","케이",
    "검사","케이","검사","케이","검사",
    "성기사","케이","검사","케이","성기사",
    "도적","검사","케이","케이","검사",
    "케이","케이"};


    // Start is called before the first frame update
    void Start()
    {
        GetScene(Scene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetScene(int Scene)
    {
        switch(Scene)
        {
            case 11:
                {
                    ShowScene = Scene11;
                    ShowSceneSprite = SceneSprite11;
                    break;
                }

        }

        SceneStart();
    }
    
    public void Tesst()
    {
        Debug.Log("asdf");
    }


    private bool All = false;
    private int CPSTemp;
    public void SceneStart()
    {
        if(All)
        {
            CPSTemp = CharPerSecond;
            CharPerSecond = 1000;
            return;
        }
        CharPerSecond = 20;
        Debug.Log(ScriptChangeIndex);
        //NextButton.enabled = false;
        TargetMsg.text = ShowScene[ScriptChangeIndex];
        TargetMsg.text = TargetMsg.text.Replace("\\n", "\n");
        NameText.text = ShowSceneSprite[ScriptChangeIndex];
        EffectStart();
    }



    public void EffectStart()
    {
        All = true;
        NextButton.enabled = false;
        ScriptText.text = "";
        index = 0;

        Invoke("Effecting", (float)1 / CharPerSecond);
    }

    void Effecting()
    {
        if (ScriptText.text == TargetMsg.text)
        {
            EffectEnd();
            return;
        }

        ScriptText.text += TargetMsg.text[index];
        index++;

        Invoke("Effecting", (float)1 /CharPerSecond);

    }
    
    void EffectEnd()
    {
        All = false;
        NextButton.enabled = true;
        ScriptChangeIndex = ScriptChangeIndex + 1;
    }
}
