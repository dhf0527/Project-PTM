using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMoveByTouch : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] float moveSpeed;
   
    Vector3 lastPos;

    private void Start()
    {
        lastPos = Camera.main.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float movePos_X = Camera.main.transform.position.x + -eventData.delta.x * moveSpeed * Time.deltaTime;
        DunGeonManager_New.instance.cameraMove.MoveCamera(movePos_X);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DunGeonManager_New.instance.cameraMove.isChasePrincess = false;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        //cameraMove.isChasePrincess = true;
    }
}
