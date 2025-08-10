using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
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
}
