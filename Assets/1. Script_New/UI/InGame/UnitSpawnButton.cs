using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class UnitSpawnButton : MonoBehaviour
{
    [HideInInspector] public Unit unit;
    [HideInInspector] public ItemData item;

    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text cost_Text;
    [SerializeField] Image unit_Image;
    [SerializeField] Image lock_Image;
    [SerializeField] Image coolDown_Image;
    [SerializeField] Animation anim;

    [HideInInspector] public bool isCoolDown;
    float coolTime;
    float cur_CoolTime;

    private void Update()
    {
        if (isCoolDown)
        {
            if (cur_CoolTime > 0)
            {
                cur_CoolTime -= Time.deltaTime;
                coolDown_Image.fillAmount = (cur_CoolTime / coolTime);
            }
            else
            {
                isCoolDown = false;
                anim.Play();
                AudioManager.instance.PlayerSfx(SFX_Enum.UnitCoolDown);
            }
        }
    }

    //UI¿¬µ¿
    public void SetUI()
    {
        if (unit == null)
        {
            GetComponent<Button>().interactable = false;
            level_Text.text = $"";
            cost_Text.text = $"";
            return;
        }

        GetComponent<Button>().interactable = true;
        level_Text.text = $"Lv.{unit.ud.level}";
        cost_Text.text = unit.Cost.ToString();

        unit_Image.sprite = unit.ud.unit_Sprite;
        lock_Image.gameObject.SetActive(false);
        coolDown_Image.gameObject.SetActive(false);
    }

    public void SetCoolDown()
    {
        isCoolDown = true;
        coolTime = unit.SpawnCoolDown;
        cur_CoolTime = coolTime;
        coolDown_Image.gameObject.SetActive(true);
    }
}
