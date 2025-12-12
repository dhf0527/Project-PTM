using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeWolrdMap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 startPos;
    private float startTime;
    private Vector2 endPos;
    private float endTime;

    public float minSwipeSpeed = 800f; // 픽셀/초 기준 속도

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        startTime = Time.unscaledTime; //
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPos = eventData.position;
        endTime = Time.unscaledTime;

        DetectSwipe();
    }

    void DetectSwipe()
    {
        Vector2 swipe = endPos - startPos;
        float duration = endTime - startTime;

        if (duration <= 0f) return;

        float speed = swipe.magnitude / duration; // 속도 계산

        // 속도가 기준보다 느리면 스와이프 취급 안 함
        if (speed < minSwipeSpeed)
            return;

        // 방향 판정
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            if (swipe.x > 0)
                OnSwipeRight();
            else
                OnSwipeLeft();
        }
    }

    void OnSwipeLeft() 
    {
        GetComponent<WorldMapBackgrounds>().DayToNight();
    }
    void OnSwipeRight() 
    {
        GetComponent<WorldMapBackgrounds>().NightToDay();
    }
}
