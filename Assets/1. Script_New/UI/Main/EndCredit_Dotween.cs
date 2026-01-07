using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

public class EndCredit_Dotween : MonoBehaviour
{
    [SerializeField] Image darkMask;
    [SerializeField] RectTransform container;
    [SerializeField] Image fadeOutMask;

    [SerializeField] int totalPageCount;
    [SerializeField] float pageChangeDuration;
    [SerializeField] Ease slideEase = Ease.OutQuint;

    [SerializeField] List<CanvasGroup> pageGroups = new List<CanvasGroup>();
    [SerializeField] CanvasGroup final_CanvasGroup;

    int currentPageIndex = 0;
    float pageWidth;
    bool isMoving = false;

    enum EndCreditStep {illust, devs, final};
    EndCreditStep curStep = EndCreditStep.illust;

    private void OnEnable()
    {
        Init();

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        isMoving = true;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, pageChangeDuration)
            .SetEase(Ease.InCubic)
            .OnComplete(() => isMoving = false);

        
    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.01f, () => { pageWidth = GetComponent<RectTransform>().rect.width; });
    }

    void Init()
    {
        curStep = EndCreditStep.illust;
        currentPageIndex = 0;
        container.anchoredPosition = Vector3.zero;
        container.gameObject.SetActive(true);
        darkMask.gameObject.SetActive(false);
        fadeOutMask.gameObject.SetActive(false);
        //개발자목록 페이지 투명화
        for (int i = 0; i < pageGroups.Count; i++)
            pageGroups[i].alpha = 0f;
        //최종 페이지 투명화
        final_CanvasGroup.alpha = 0f;
    }

    public void OnScreenTouch()
    {
        if (isMoving)
            return;

        //일러스트 단계
        if(curStep == EndCreditStep.illust)
        {
            float duration = pageChangeDuration;

            darkMask.gameObject.SetActive(true);
            float darkmask_a = darkMask.color.a;
            darkMask.color = new Color(0, 0, 0, 0);

            isMoving = true;
            pageGroups[0].DOFade(1f, duration)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    curStep = EndCreditStep.devs;
                    pageGroups[0].interactable = true;
                    isMoving = false;
                });
            darkMask.GetComponent<Image>().DOFade(darkmask_a, duration);
        }
        //개발자 목록 단계
        else if(curStep == EndCreditStep.devs)
        {
            //마지막 페이지
            if (currentPageIndex >= totalPageCount - 1)
            {
                float duration = pageChangeDuration;
                isMoving = true;

                final_CanvasGroup.gameObject.SetActive(true);

                Sequence seq = DOTween.Sequence();

                //마지막 페이지 페이드 아웃
                seq.Append(pageGroups[currentPageIndex].DOFade(0f, duration)
                    .SetEase(Ease.OutCubic));
                //최종 페이지 페이드 인
                seq.Append(final_CanvasGroup.DOFade(1f, duration)
                    .SetEase(Ease.OutCubic));
                seq.OnComplete(() =>
                    {
                        curStep = EndCreditStep.final;
                        isMoving = false;
                    });

                return;
            }

            int prev_index = currentPageIndex;
            currentPageIndex++;
            MoveToPage(prev_index);
        }
        //최종 인사 단계
        else
        {
            fadeOutMask.gameObject.SetActive(true);
            fadeOutMask.color = new Color(0, 0, 0, 0);

            Sequence seq = DOTween.Sequence();
            seq.Append(fadeOutMask.DOFade(1f, pageChangeDuration).SetEase(Ease.InCubic));
            //페이드 아웃 이후 2초 대기
            seq.AppendInterval(2f);
            seq.OnComplete(() => gameObject.SetActive(false));
        }
    }

    void MoveToPage(int pre_index)
    {
        isMoving = true;
        float targetX = -pageWidth * currentPageIndex;

        //위치 이동
        container.DOAnchorPos(new Vector2(targetX, 0), pageChangeDuration)
            .SetEase(slideEase)
            .OnComplete(() => isMoving = false);

        //이전 페이지 페이드 아웃
        pageGroups[pre_index].DOFade(0f, pageChangeDuration * 0.5f);

        //현재 페이지 페이드 인
        pageGroups[currentPageIndex].DOFade(1f, pageChangeDuration);
    }
}
