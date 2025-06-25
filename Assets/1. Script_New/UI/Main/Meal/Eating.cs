using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Eating : MonoBehaviour
{
    public Meal meal;
    public Image loading_Bar;
    public TMP_Text loading_Text;

    [SerializeField] float loadingTime = 2;
    string origin_Text;

    private void OnEnable()
    {
        origin_Text = loading_Text.text;

        StartCoroutine(C_LoadingBar());
        StartCoroutine(C_LoadingText());
    }

    IEnumerator C_LoadingBar()
    {
        loading_Bar.fillAmount = 0;
        float t = 0;
        while (t < loadingTime)
        {
            t += Time.deltaTime;
            loading_Bar.fillAmount = t / loadingTime;
            yield return null;
        }
        meal.OpenGo(meal.gameObjects[3]);
        StopAllCoroutines();
        loading_Text.text = origin_Text;

        MainManager.instance.OnMeal(true);
    }

    IEnumerator C_LoadingText()
    {
        float textSpeed = 1f;
        float t = 0;
        while (t < loadingTime)
        {
            t += Time.deltaTime;

            if (t % textSpeed < (textSpeed / 3f))
                loading_Text.text = origin_Text + ".";
            else if(t%textSpeed < (textSpeed * 2 / 3f))
                loading_Text.text = origin_Text + "..";
            else
                loading_Text.text = origin_Text + "...";
            yield return null;
        }
    }

    public void Test_Complete()
    {
        meal.OpenGo(meal.gameObjects[3]);
        StopAllCoroutines();
        loading_Text.text = origin_Text;

        MainManager.instance.OnMeal(true);
    }
}
