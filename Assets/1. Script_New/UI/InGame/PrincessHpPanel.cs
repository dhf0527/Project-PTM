using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrincessHpPanel : MonoBehaviour
{
    public Image character_Image;
    public Image hpBar_Image;
    public TMP_Text coolDown_Text;
    public TMP_Text hp_Text;

    [HideInInspector] public float rest_Time = 0;

    private void Update()
    {
        if (rest_Time > 0)
        {
            if (!coolDown_Text.gameObject.activeInHierarchy)
            {
                coolDown_Text.gameObject.SetActive(true);
                character_Image.color = new Color(100f / 255f, 100f / 255f, 100f / 255f);
            }

            rest_Time -= Time.deltaTime;
            coolDown_Text.text = string.Format("{0:0}", rest_Time);
        }
        //타이머가 다 됐을 때 처리
        else if (coolDown_Text.gameObject.activeInHierarchy)
        {
            coolDown_Text.gameObject.SetActive(false);
            character_Image.color = Color.white;
            DunGeonManager_New.instance.PrincessRivive();
        }
    }

    public void SetHpBar(BaseUnit princess)
    {
        hp_Text.text = $"{princess.Cur_Hp}/{princess.ud.hp}";
        hpBar_Image.fillAmount = princess.Cur_Hp / princess.ud.hp;
    }
}
