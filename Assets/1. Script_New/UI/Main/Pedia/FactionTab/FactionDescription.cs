using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactionDescription : MonoBehaviour
{
    public Image faction_Image;
    public TMP_Text factionName_Text;
    public TMP_Text factionDescription_Text;

    public void SetData(FactionData fd)
    {
        faction_Image.sprite = fd.faction_Sprite;
        factionName_Text.text = fd.factionName;
        factionDescription_Text.text = fd.factionDescription;
    }
}
