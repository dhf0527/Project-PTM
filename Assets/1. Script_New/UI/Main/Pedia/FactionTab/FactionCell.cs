using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactionCell : MonoBehaviour
{
    public FactionData factionData;

    public Image faction_Image;
    public TMP_Text faction_Text;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        faction_Image.sprite = factionData.faction_Sprite;
        faction_Text.text = factionData.factionName;
    }

    public void OnClick()
    {
        PediaManager.instance.SetFactionData(factionData);
    }
}
