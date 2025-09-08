using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PassivePanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text detailText;

    const string earring = "이명";
    const string reborn = "부활 상태";
    const string cursedFlame = "저주 받은 불꽃";
    const string cuttedArmor = "절단된 갑옷";
    const string reapedSpirit = "수확한 영혼";

    string Color_Red { get { return ColorToHex(color_Red_1); } }
    /*= "#CD3B3B";*/
    string Color_Blue {get{ return ColorToHex(color_Blue_1); }}
    /*= "#3A43CD";*/

    public Color color_Red_1;
    public Color color_Blue_1;

    public void SetNameText(string setText)
    {
        nameText.text = setText;
    }

    public void SetDetailText(string setText)
    {
        //' 사이의 단어(키워드)들을 찾아 저장
        List<string> keyWords = FindWords(setText);

        foreach (var keyWord in keyWords)
        {
            //각 키워드를 찾아서 대체
            setText = Regex.Replace(setText, $"\'{keyWord}\'", $"<u>{keyWord}</u>");
            //키워드에 대한 설명(무한 루프 방지)
            if(keyWord != nameText.text)
                WordDetail(keyWord);
        }

        //*b(단어)* = 파란색, *r(단어)* = 빨간색 단어 색상 변경
        foreach (var words_Blue in FindWords_Blue(setText))
            setText = Regex.Replace(setText, $"\\*b{words_Blue}\\*", ColorText(words_Blue, Color_Blue));
        foreach (var words_Blue in FindWords_Red(setText))
            setText = Regex.Replace(setText, $"\\*r{words_Blue}\\*", ColorText(words_Blue, Color_Red));

        detailText.text = setText;
    }

    // '(따옴표) 사이의 단어들을 찾아 반환하는 함수 ex)'이명', '부활 상태'
    List<string> FindWords(string input_Text)
    {
        List<string> words = new List<string>();

        MatchCollection matches = Regex.Matches(input_Text, "'(.*?)'");

        foreach (Match match in matches)
            words.Add(match.Groups[1].Value);

        return words;
    }
    
    List<string> FindWords_Red(string input_Text)
    {
        List<string> words = new List<string>();

        MatchCollection matches = Regex.Matches(input_Text, "\\*r(.*?)\\*");

        foreach (Match match in matches)
            words.Add(match.Groups[1].Value);

        return words;
    }
    List<string> FindWords_Blue(string input_Text)
    {
        List<string> words = new List<string>();

        MatchCollection matches = Regex.Matches(input_Text, "\\*b(.*?)\\*");

        foreach (Match match in matches)
            words.Add(match.Groups[1].Value);

        return words;
    }

    void WordDetail(string word)
    {
        string passive_Name = word;
        string passive_Detail;

        switch (word)
        {
            case earring:
                passive_Detail = $"명중률, 회피율 {ColorText("-20",Color_Red)}";
                break;
            case reborn:
                passive_Detail = $"이동 속도, 공격 속도 {ColorText("+100%", Color_Blue)}, 초당 최대 체력의 {ColorText("10% 감소", Color_Red)}";
                break;
            case cursedFlame:
                passive_Detail = $"0.5초당 {ColorText("2",Color_Red)}의 피해. 받는 모든 회복 효과가 {ColorText("절반",Color_Red)}으로 {ColorText("감소",Color_Red)}.";
                break;
            case cuttedArmor:
                passive_Detail = $"방어도 {ColorText("-50%", Color_Red)}";
                break;
            case reapedSpirit:
                passive_Detail = $"공격력 {ColorText("+20", Color_Blue)}, 명중률 {ColorText("+40", Color_Blue)}";
                break;
            default:
                return;
        }

        DunGeonManager_New.instance?.unitUnlock.detailPanel.MakeNewDetail(passive_Name ,passive_Detail);
        PediaManager.instance?.unitDetail.MakeNewDetail(passive_Name, passive_Detail);
    }

    string ColorText(string word ,string color_Code)
    {
        return $"<color={color_Code}>{word}</color>";
    }

    string ColorToHex(Color color)
    {
        return "#" + Mathf.RoundToInt(color.r * 255f).ToString("X2") +
                    Mathf.RoundToInt(color.g * 255f).ToString("X2") +
                    Mathf.RoundToInt(color.b * 255f).ToString("X2");
    }
}
