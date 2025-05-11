using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCell : MonoBehaviour
{
    public bool isLock;
    public UnitData ud;

    public TMP_Text faction_Text;
    public TMP_Text name_Text;
    public Image unit_Image;
    public GameObject silhouetteMask_go;
    public GameObject whiteMask_go;

    private void Awake()
    {
        Init();
        SetLock();
    }

    void Init()
    {
        faction_Text.text = ud.faction == Faction.Guild ? "Áß¾Ó ¿Õ±¹"
            : ud.faction == Faction.Fairy ? "¿äÁ¤ ½£"
            : ud.faction == Faction.Demon ? "¸¶¿Õ±º"
            : "¹¦Áö±â";
        name_Text.text = isLock ? "???" : ud.unit_Name;
        unit_Image.sprite = ud.unit_Sprite;
    }

    public void SetLock()
    {
        silhouetteMask_go.SetActive(isLock);
        whiteMask_go.SetActive(isLock);
        GetComponent<Button>().interactable = !isLock;
    }

    public void OnClick()
    {
        PediaManager.instance.SetHeroData(ud);
        PediaManager.instance.OnHeroDescription();
    }
}
