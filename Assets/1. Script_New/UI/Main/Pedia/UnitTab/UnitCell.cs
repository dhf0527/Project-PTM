using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCell : MonoBehaviour
{
    public UnitData ud;

    public TMP_Text name_Text;
    public Image unit_Image;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        unit_Image.sprite = ud.unit_Sprite;
        name_Text.text = ud.unit_Name;
    }

    public void OnClick()
    {
        PediaManager.instance.SetUnitData(ud);
    }
}
