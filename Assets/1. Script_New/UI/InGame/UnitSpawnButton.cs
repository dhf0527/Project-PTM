using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnButton : MonoBehaviour
{
    [HideInInspector] public Unit unit;

    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text cost_Text;
    [SerializeField] Image unit_Image;
    [SerializeField] Image lock_Image;
    [SerializeField] Image coolDown_Image;

    private void Start()
    {
        SetUI();
    }

    //UI¿¬µ¿
    public void SetUI()
    {
        if (unit == null)
            return;

        level_Text.text = $"Lv.{unit.ud.level}";
        cost_Text.text = $"{unit.ud.cost}";
        unit_Image.sprite = unit.ud.unit_Sprite;
        lock_Image.gameObject.SetActive(false);
        coolDown_Image.gameObject.SetActive(false);
    }
}
