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
                GameObject prefab = Resources.Load<GameObject>("Prefabs/SceneChangeManager");
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab);
                    instance = go.GetComponent<SceneChangeManager>();
                    DontDestroyOnLoad(go);
                }
                else
                    Debug.LogError("Resources 내부에 SceneChangeManager 없음");
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
        {
            Destroy(gameObject);
            return;
        }
    }

    public float fade_Time;

    public void OnToMainScene()
    {
        FadeManager.Instance.FadeAction(fade_Time, () =>
        {
            string loadSceneName = ConstData.sceneName_Main;

            AudioManager.Instance.PlayerBgm(loadSceneName);
            SceneManager.LoadScene(loadSceneName);
            Time.timeScale = 1f;
        }
        , 2f, () => CutSceneManager.instance.CheckDialogues());
    }

    public void OnToDungeonScene()
    {
        FadeManager.Instance.FadeAction(fade_Time, () =>
        {
            string loadSceneName = ConstData.sceneName_Dungeon;

            AudioManager.Instance.PlayerBgm(loadSceneName);
            SceneManager.LoadScene(loadSceneName);
            Time.timeScale = 1f;
        });
    }

    public void OnToTestScene()
    {
        FadeManager.Instance.FadeAction(fade_Time, () =>
        {
            SceneManager.LoadScene(ConstData.sceneName_Dungeon, LoadSceneMode.Single);
            SceneManager.LoadScene(ConstData.sceneName_Test, LoadSceneMode.Additive);
            AudioManager.Instance.PlayerBgm(ConstData.sceneName_Dungeon);

            GameManager.Instance.current_Dungeon = MainManager.instance.test_DungeonData;
            Time.timeScale = 1f;
        });
    }

    public void OnToIntroScene()
    {
        FadeManager.Instance.FadeAction(fade_Time, () =>
        {
            string loadSceneName = ConstData.sceneName_Intro;

            AudioManager.Instance.PlayerBgm(loadSceneName);
            SceneManager.LoadScene(loadSceneName);
            Time.timeScale = 1f;
        });
    }
}
