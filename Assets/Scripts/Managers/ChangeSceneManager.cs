using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void GoToDungeon1()
    {
        SceneManager.LoadScene("Dungeon1");
    }

    public void GoToDungeon2()
    {
        SceneManager.LoadScene("Dungeon2");
    }

    public void GoToDungeon3()
    {
        SceneManager.LoadScene("Dungeon4");
    }

    public void GoToDungeon4()
    {
        SceneManager.LoadScene("Dungeon3");
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
