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
    { "��ΰ� ������ ��ȭ�� �ͼ����� ����,\n���踦 ���� �� �ִ� �� Į����, �ʿ� �����̾�!",
    "Ǫ��...",
    "...�׷� ������� �Ҹ� ���� ��!",
    "������ �� ���� �¾�.\n�츮 ���̼� ������ Ȧ�� ����ϴ� �� ���� �ٺ� ���� ���̾�.",
    "���谡���� ��ƾ� ��!\n������ �ƹ� ���谡�� �ȵ�.\n���� ������ ���ذ� ������ ����...",
    "���� �ָ� ���� ���ϼ��� ���̴�!\n���ִ��� ã�ƶ�!",
    "��...! �ƹٸ����� �������̴�!\n���� �� ã�� ��ó� ��!\n������ ���� �� ���� ���� ���� �� ����...",
    "...��, ����! �̺� �ű� ��!",
    "...��? ����?",
    "�׷� �ָ��ϰ� ���ΰ�ó�� ���� ��!\n�� �Ʊ� ��� �� �ݾҴٰ� ��� �ִ� ���谡��?",
    "��, �� ����ŵ��!\n�׸��� ��嵵 �� �ݾ����� ���� ���谡�� �ƴ϶󱸿�.",
    "�ƹ����� �ư�, ��\n���ú��� �뺴�ض�.",
    "��? �뺴�̿�?",
    "�׷�! �� ���� �ձ��� ���ְ� ������ �ս� �뺴�̾�! ������!\n�� ���� �� ����縦 ����!",
    "��! �� �սǿ��� ���غ��� �� ���̾����!\n...���, ���� �����󱸿�?",
    "���ִ��� ã�Ҵ�!\n���� ���ñ� ���� ��ƶ�!",
    "����...! ���״�!\n��, �ƹ�ư �� ���� �� �뺴�̴ϱ� ��Ե� �������� ����!",
    "��?? ���� �ձ� ������ݾƿ�!\n���� ����� �̱� �� ���ٱ���!",
    "���� �ʺ��� ����� �������� ����Ʈ����?\n�ð����̶� ����!",
    "���谡���̿�! ����ġ�� ���ִ��� ��ƶ�!\n��ƿ��� �ڿ��� ������� �ְڴ�!",
    "��? �����??\n�� �׷��� ���� �ϵ� �����µ� �� �Ƴ�!",
    "���̰�!\n������ �������� ���ż� ���ǹ��ϰ� �װ� �����!!",
    "���� �����ϴٴ� �ž�, �� �����̰�!\n...�ƹ����� ������ �� �㿩��� �뺴���μ��� �ڰ��� �� ����ڴ°�.",
    "��! �̰� �޾�!",
    "�� ���̴�!	������!\n�츮 �����... �ƴ� ���ִ��� �����ϴ� �༮����!",
    "��, ���� ���������� ������ ���谡 �ٿ� �ڼ��� �����.",
    "���谡! �������� ��� ���� �־�.\n���� �������� ��ƿ� �״ϱ�!!"
    };
    string[] SceneSprite11 = 
        {"����","Į����","����","����","����",
    "�����","����","����","�˻�","����",
    "�˻�","����","�˻�","����","�˻�",
    "�����","����","�˻�","����","�����",
    "����","�˻�","����","����","�˻�",
    "����","����"};


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
