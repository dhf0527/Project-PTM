using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CartoonCutSceneImages : MonoBehaviour
{
    public List<Image> cutSceneImages;
    float width;

    private void Awake()
    {
        width = GetComponent<RectTransform>().rect.width;
    }

    public void MoveCutScene()
    {
        InitPos();

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(cutSceneImages[0].rectTransform.DOAnchorPos(-Vector2.right * width, 1f).SetEase(Ease.OutQuart))
        .Join(cutSceneImages[1].rectTransform.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutQuart))
        .OnComplete(OnCompleteMoveCutScene);

        moveSequence.Play();
    }

    void OnCompleteMoveCutScene()
    {
        CutSceneManager.instance.OnNextCutScene();
        InitPos();
    }

    void InitPos()
    {
        cutSceneImages[0].rectTransform.anchoredPosition = Vector2.zero;
        cutSceneImages[1].rectTransform.anchoredPosition = Vector2.right * width;
    }
}
