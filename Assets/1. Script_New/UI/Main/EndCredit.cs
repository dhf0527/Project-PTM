using System.Collections;
using UnityEngine;

public class EndCredit : MonoBehaviour
{
    public RectTransform container;
    public int totalPageCount;
    [SerializeField] float pageChangeDuration;
    int currentPageIndex = 0;
    float pageWidth;
    bool isMoving = false;

    private void Start()
    {
        pageWidth = GetComponent<RectTransform>().rect.width;
    }

    public void OnScreenTouch()
    {
        if (isMoving || currentPageIndex >= totalPageCount - 1)
            return;

        currentPageIndex++;
        StartCoroutine(C_SlideNext());
    }

    IEnumerator C_SlideNext()
    {
        isMoving = true;
        Vector3 startPos = container.anchoredPosition;
        Vector3 endPos = new Vector3(-pageWidth * currentPageIndex, startPos.y, startPos.z);

        float curTime = 0;
        float duration = 0.5f;

        while(curTime < duration)
        {
            curTime += Time.unscaledDeltaTime;
            container.anchoredPosition = Vector3.Lerp(startPos, endPos, curTime / duration);
            yield return null;
        }

        container.anchoredPosition = endPos;
        isMoving = false;
    }
}
