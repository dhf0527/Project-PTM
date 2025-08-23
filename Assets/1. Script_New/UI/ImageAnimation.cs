using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] float period;
    Image img;
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

            if(index >= sprites.Count)
                index -= sprites.Count;

            img.sprite = sprites[index];
            curTime = 0;
        }
    }
}
