using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillIcon : MonoBehaviour
{
    [SerializeField] int skill_Index; 

    public Image delay_Image;
    public Image coolDownIcon_Image;
    public Animation anim;
    Animation clickAnim;

    private void Awake()
    {
        clickAnim = GetComponent<Animation>();
        coolDownIcon_Image.gameObject.SetActive(false);
        delay_Image.fillAmount = 0;
    }

    private void Start()
    {
        GetComponent<Image>().sprite = DunGeonManager_New.instance.princess.skillDatas[skill_Index].skillIcon;
    }


    public void SetDelayImage(float fillAmount)
    {
        if (fillAmount <= 0)
            anim.Play();

        coolDownIcon_Image.gameObject.SetActive(fillAmount > 0);
        delay_Image.fillAmount = fillAmount;
    }

    public void OnClicked()
    {
        clickAnim.Play();
    }
}
