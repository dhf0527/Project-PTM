using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAudio : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public SFX_Enum sfx_enum;
    public bool isButtonDown;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Button>() && !GetComponent<Button>().interactable)
            return;

        if(!isButtonDown)
            AudioManager.instance.PlayerSfx(sfx_enum);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>() && !GetComponent<Button>().interactable)
            return;

        if (isButtonDown)
            AudioManager.instance.PlayerSfx(sfx_enum);
    }
}
