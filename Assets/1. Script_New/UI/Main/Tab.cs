using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public GameObject activeObj;

    public Color SelectedColor;
    public float fade_Time;

    Image img;
    Toggle toggle;

    Color origin_Color;

    private void Awake()
    {
        img = GetComponent<Image>();
        toggle = GetComponent<Toggle>();

        origin_Color = img.color;
    }

    private void Start()
    {
        SetTab(toggle.isOn);
    }

    public void SetTab(bool isOn)
    {
        activeObj.SetActive(isOn);

        if (isOn)
        {
            if (fade_Time > 0)
                StartCoroutine(C_SetColor());
            else
            img.color = SelectedColor;
        }
        else
        {
            StopAllCoroutines();
            img.color = origin_Color;
        }
    }

    IEnumerator C_SetColor()
    {
        float t = 0;
        while(t < fade_Time)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, fade_Time);
            float color_comp = Mathf.Lerp(origin_Color.r, SelectedColor.r, t / fade_Time);
            img.color = new Color(color_comp, color_comp, color_comp);
            yield return new WaitForEndOfFrame();
        }
    }
}
