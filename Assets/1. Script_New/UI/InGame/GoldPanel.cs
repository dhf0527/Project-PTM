using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    [SerializeField] TMP_Text gold_Text;
    [SerializeField] TMP_Text goldPerSec_Text;

    public void SetGoldText()
    {
        gold_Text.text = $"{(int)DunGeonManager_New.instance.Cur_Gold}/{DunGeonManager_New.instance.Max_Gold}";
        goldPerSec_Text.text = $"+{DunGeonManager_New.instance.Gold_Per_Sec}/s";
    }
}
