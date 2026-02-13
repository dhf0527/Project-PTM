using DG.Tweening;
using System;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    private static FadeManager instance;
    public static FadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/FadeManager");
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab);
                    instance = go.GetComponent<FadeManager>();
                    DontDestroyOnLoad(go);
                }
                else
                    Debug.LogError("Resources 내부에 FadeManager 없음");
            }
            return instance;
        }
        
    }

    CanvasGroup canvasGroup;
    bool isFading;

    private void Awake()
    {
        // 싱글톤 중복 방지
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

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeAction(float duration, Action midAction, float interval = 0.5f, Action endAction = null)
    {
        //중복 실행 방지
        if (isFading)
            return;
        isFading = true;

        canvasGroup.blocksRaycasts = true;

        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(canvasGroup.DOFade(1, duration))
        .AppendCallback(() => midAction?.Invoke())
        .AppendInterval(interval)
        .AppendCallback(()=>
        {
            canvasGroup.blocksRaycasts = false;
        })
        .Append(canvasGroup.DOFade(0, duration))
        .OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = false;
            isFading = false;
            endAction?.Invoke();
        })
        .SetUpdate(true);
    }

    public void FadeAction(float fadeOutDuration, float fadeInDuration, Action midAction, float interval = 0.5f, Action endAction = null)
    {
        //중복 실행 방지
        if (isFading)
            return;
        isFading = true;

        canvasGroup.blocksRaycasts = true;

        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(canvasGroup.DOFade(1, fadeOutDuration))
        .AppendCallback(() => midAction?.Invoke())
        .AppendInterval(interval)
        .Append(canvasGroup.DOFade(0, fadeInDuration))
        .OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = false;
            isFading = false;
            endAction?.Invoke();
        })
        .SetUpdate(true);
    }
}
