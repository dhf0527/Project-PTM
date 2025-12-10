using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ReplaceWord : MonoBehaviour
{
    string Color_Red { get { return ColorToHex(color_Red_1); } }
    //= "#FBA3A3";
    string Color_Blue { get { return ColorToHex(color_Blue_1); } }
    //= "#A6CFF7";

    public Color color_Red_1;
    public Color color_Blue_1;

    public string ReplaceWordColor(string input_Text)
    {
        //*b(단어)* = 파란색, *r(단어)* = 빨간색 단어 색상 변경
        foreach (var words_Blue in FindWords_Blue(input_Text))
        {
            string escapedWord = Regex.Escape(words_Blue);
            input_Text = Regex.Replace(input_Text, $"\\*b{escapedWord}\\*", ColorText(words_Blue, Color_Blue));

        }
        foreach (var words_Red in FindWords_Red(input_Text))
        {
            string escapedWord = Regex.Escape(words_Red);
            input_Text = Regex.Replace(input_Text, $"\\*r{escapedWord}\\*", ColorText(words_Red, Color_Red));
        }
        return input_Text;
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

    string ColorText(string word, string color_Code)
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
