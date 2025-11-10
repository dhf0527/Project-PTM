using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] float period;
    [SerializeField] float period_dissolve;
    Image img;
    [SerializeField]Image pre_img;
    float curTime;
    int index = 0;

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

            img.sprite = sprites[(index + 1) % sprites.Count];
            if(pre_img)
                pre_img.sprite = sprites[index];
            curTime = 0;
        }

        if (pre_img)
        {
            Color tmp_Color = pre_img.color;
            tmp_Color.a = 1 - curTime / period_dissolve;
            pre_img.color = tmp_Color;
        }
        

        
    }
}
