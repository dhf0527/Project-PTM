using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PassiveDetail : MonoBehaviour
{
    TMP_Text detailText;

    string earring = "ÀÌ¸ם";
    string after_text;

    private void Awake()
    {
        detailText = GetComponent<TMP_Text>();
        after_text = Regex.Replace(detailText.text, $"\'{earring}\'", $"<color=red>{earring}</color>");
            detailText.text = after_text;
    }
}
