using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    /*
    private static SceneChangeManager instance;
    public static SceneChangeManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SceneChangeManaer");
                instance = go.AddComponent<SceneChangeManager>();
            }

            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    */

    [SerializeField] float fade_Time;

    public void OnToMainScene()
    {
        string loadSceneName = ConstData.sceneName_Main;

        AudioManager.Instance.PlayerBgm(loadSceneName);
        SceneManager.LoadScene(loadSceneName);
        Time.timeScale = 1f;
    }

    public void OnToDungeonScene()
    {
        string loadSceneName = ConstData.sceneName_Dungeon;

        AudioManager.Instance.PlayerBgm(loadSceneName);
        SceneManager.LoadScene(loadSceneName);
        Time.timeScale = 1f;
    }

    public void OnToTestScene()
    {
        SceneManager.LoadScene(ConstData.sceneName_Dungeon, LoadSceneMode.Single);
        SceneManager.LoadScene(ConstData.sceneName_Test, LoadSceneMode.Additive);
        AudioManager.Instance.PlayerBgm(ConstData.sceneName_Dungeon);

        GameManager.Instance.current_Dungeon = MainManager.instance.test_DungeonData;
        Time.timeScale = 1f;
    }

    public void OnToIntroScene()
    {
        string loadSceneName = ConstData.sceneName_Intro;
        
        AudioManager.Instance.PlayerBgm(loadSceneName);
        SceneManager.LoadScene(loadSceneName);
        Time.timeScale = 1f;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnToMainScene_Fade()
    {
        StartCoroutine(C_Fade());
    }

    IEnumerator C_Fade()
    {
        //페이드인
        Image fadeMask = SearchManager.Instance.Search(SearchKey.FadeMask).GetComponent<Image>();
        fadeMask.gameObject.SetActive(true);
        float curTime = 0;
        Color tmp_Color;
        while(curTime < fade_Time)
        {
            curTime += Time.unscaledDeltaTime;

            //투명도 조절
            tmp_Color = fadeMask.color;
            tmp_Color.a = curTime / fade_Time;
            fadeMask.color = tmp_Color;

            yield return null;
        }
        tmp_Color = fadeMask.color;
        tmp_Color.a = 1;
        fadeMask.color = tmp_Color;

        OnToMainScene();
    }
}
