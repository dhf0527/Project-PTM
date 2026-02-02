using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [System.Serializable]
    enum SceneType
    {
        intro, main, dungeon
    }
    [SerializeField] SceneType sceneType;

    void Start()
    {
        switch (sceneType)
        {
            case SceneType.intro:
                GetComponent<Button>().onClick.AddListener(SceneChangeManager.Instance.OnToIntroScene);
                break;
            case SceneType.main:
                GetComponent<Button>().onClick.AddListener(SceneChangeManager.Instance.OnToMainScene);
                break;
            case SceneType.dungeon:
                GetComponent<Button>().onClick.AddListener(SceneChangeManager.Instance.OnToDungeonScene);
                break;
            default:
                Debug.LogError("SceneChangeButton: sceneType ¿À·ù");
                break;
        }
    }

}
