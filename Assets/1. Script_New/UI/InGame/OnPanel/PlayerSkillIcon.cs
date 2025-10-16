using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillIcon : MonoBehaviour
{
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
