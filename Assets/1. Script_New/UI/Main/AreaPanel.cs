using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreaPanel : MonoBehaviour
{
    [SerializeField] Image top_Image;
    [SerializeField] List<Image> side1_Images;
    [SerializeField] List<Image> side2_Images;
    [SerializeField] TMP_Text title_Text;

    [Header("길드, 요정, 마계, 묘지")]
    [SerializeField] List<Sprite> top_Sprites;
    [SerializeField] List<Sprite> side1_Sprites;
    [SerializeField] List<Sprite> side2_Sprites;

    public void SetAreaPanelData(Faction factionIndex)
    {
        top_Image.sprite = top_Sprites[(int)factionIndex];
        foreach (var item in side1_Images)
            item.sprite = side1_Sprites[(int)factionIndex];
        foreach (var item in side2_Images)
            item.sprite = side2_Sprites[(int)factionIndex];

        switch (factionIndex)
        {
            case Faction.Guild:
                title_Text.text = "중앙 왕국";
                break;
            case Faction.Fairy:
                title_Text.text = "요정 숲";
                break;
            case Faction.Demon:
                title_Text.text = "마계";
                break;
            case Faction.Graveyard:
                title_Text.text = "버려진 묘지";
                break;
            default:
                Debug.Log("세력 오류");
                break;
        }
    }
}
