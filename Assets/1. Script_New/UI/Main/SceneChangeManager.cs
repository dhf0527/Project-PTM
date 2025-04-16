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
        SceneManager.LoadScene(mainSceneName);
        Time.timeScale = 1f;
    }

    public void OnToDungeonScene()
    {
        SceneManager.LoadScene(dungeonSceneName);
        Time.timeScale = 1f;
    }
}
