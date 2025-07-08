using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    readonly static string mainSceneName = "MainScene";
    readonly static string dungeonSceneName = "Dungeon";


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

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
