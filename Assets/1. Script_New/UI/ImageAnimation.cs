using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<Sprite> night_Sprites;
    [SerializeField] float period;
    [SerializeField] float period_dissolve;
    Image img;
    [SerializeField] Image pre_img;
    [SerializeField] GameObject next_Obj;
    [SerializeField] GameObject back_Obj;

    float curTime;
    int index = 0;

    bool isNight;
    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        curTime += Time.unscaledDeltaTime;

        if (curTime > period)
        {
            index++;

            if (index >= sprites.Count)
                index -= sprites.Count;

            SetSprite();
            curTime = 0;
        }

        if (pre_img)
        {
            Color tmp_Color = pre_img.color;
            tmp_Color.a = 1 - curTime / period_dissolve;
            pre_img.color = tmp_Color;
        }
    }

    public void SetSprite()
    {
        if (isNight)
        {
            img.sprite = night_Sprites[(index + 1) % night_Sprites.Count];
            if (pre_img)
                pre_img.sprite = night_Sprites[index];
        }
        else
        {
            img.sprite = sprites[(index + 1) % sprites.Count];
            if (pre_img)
                pre_img.sprite = sprites[index];
        }
    }

    public void SetIsNight(bool setBool)
    {
        isNight = setBool;
        next_Obj.SetActive(!setBool);
        back_Obj.SetActive(setBool);
    }
}
