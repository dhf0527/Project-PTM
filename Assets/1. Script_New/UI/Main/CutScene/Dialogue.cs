using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [Header("초당 표시될 텍스트의 수")]
    [SerializeField] float textPerSec;
    [HideInInspector] public bool isTyping;

    CutSceneManager cutSceneManager;
    TMP_Text dialogue_Text;
    Coroutine co_TypeText;

    AudioSource text_AudioSource;
    string target_Text;

    private void Awake()
    {
        cutSceneManager = FindObjectOfType<CutSceneManager>();
        dialogue_Text = GetComponent<TMP_Text>();
    }

    IEnumerator C_TypeText(string input_text)
    {
        isTyping = true;

        input_text = input_text.Replace("\\n", "\n");

        target_Text = input_text;
        dialogue_Text.text = string.Empty;
        StringBuilder sb = new StringBuilder();

        //타이핑 효과음 재생
        text_AudioSource = AudioManager.Instance.PlayerSfx_Source(SFX_Enum.Dialogue2);
        text_AudioSource.loop = true;

        for (int i = 0; i < input_text.Length; i++)
        {
            sb.Append(input_text[i]);

            //리치 텍스트는 한 번에 표시
            if (input_text[i] == '<')
            {
                i++;
                while (input_text[i] != '>')
                    sb.Append(input_text[i++]);
                sb.Append(input_text[i]);
            }

            dialogue_Text.text = sb.ToString();

            //텍스트가 출력될 속도
            yield return new WaitForSecondsRealtime(1 / textPerSec);
        }

        //타이핑 효과음 정지
        text_AudioSource.loop = false;
        text_AudioSource.Stop();
        isTyping = false;
    }

    public void StartTypeText(string input_Text)
    {
        co_TypeText = StartCoroutine(C_TypeText(input_Text));
    }

    public void ToNextTypeText()
    {
        //타이핑 효과가 나오는 중일 때
        if (isTyping)
            EndTyping();
        //타이핑 효과가 끝난 후일 때
        else
        {
            //다음 대사 출력
            cutSceneManager.PrintNextDialogue();
        }

        AudioManager.Instance.PlayerSfx(SFX_Enum.Touch);
    }

    public void EndTyping()
    {
        isTyping = false;
        //코루틴 정지
        StopCoroutine(co_TypeText);
        //텍스트 즉시 표시
        dialogue_Text.text = target_Text;
        text_AudioSource.loop = false;
        text_AudioSource.Stop();
    }
}
