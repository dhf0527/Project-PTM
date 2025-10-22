using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
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

    [SerializeField] float fade_Time;

    readonly static string mainSceneName = "MainScene";
    readonly static string dungeonSceneName = "Dungeon";
    readonly static string testSceneName = "TestScene";

    public void OnToMainScene()
    {
        AudioManager.Instance.PlayerBgm(BGM_Enum.WorldMap);
        SceneManager.LoadScene(mainSceneName);
        Time.timeScale = 1f;
    }

    public void OnToDungeonScene()
    {
        AudioManager.Instance.PlayerBgm(BGM_Enum.Map_1);
        SceneManager.LoadScene(dungeonSceneName);
        Time.timeScale = 1f;
    }

    public void OnToTestScene()
    {
        AudioManager.Instance.PlayerBgm(BGM_Enum.Map_1);
        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(dungeonSceneName, LoadSceneMode.Additive);

        //테스트 씬 오류를 방지하기 위해 임시로 첫 번째 던전의 값을 삽입
        GameManager.Instance.current_Dungeon = MainManager.instance.dungeonDatas[0];
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

        AudioManager.Instance.PlayerBgm(BGM_Enum.WorldMap);
        SceneManager.LoadScene(mainSceneName);
        Time.timeScale = 1f;
    }
}
