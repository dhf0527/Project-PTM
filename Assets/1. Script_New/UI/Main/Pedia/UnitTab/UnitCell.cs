using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCell : MonoBehaviour
{
    public UnitData ud;
    [SerializeField] GameObject lockImage_go;

    public TMP_Text name_Text;
    public Image unit_Image;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        unit_Image.sprite = ud.unit_Sprite;

        if (ud.isHardMode)
        {
            if (PlayerPrefs.GetInt(ConstData.hardMode_Unlock) != 0)
            {
                GetComponent<Button>().interactable = true;
                name_Text.text = ud.unit_Name;
                lockImage_go.SetActive(false);
            }
            else
            {
                GetComponent<Button>().interactable = false;
                name_Text.text = "???";
                lockImage_go.SetActive(true);
            }
        }
        else
            name_Text.text = ud.unit_Name;
    }

    public void OnClick()
    {
        PediaManager.instance.SetUnitData(ud);
        animator.SetTrigger("Anim");
    }
}
