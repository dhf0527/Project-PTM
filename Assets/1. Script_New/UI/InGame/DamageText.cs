using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("0없음 1물리 2마법 3화염")]
    [SerializeField] List<Color> colors;

    public TMP_Text dmg_Text;
    [HideInInspector] public Vector3 pos;

    private void OnEnable()
    {
        Invoke("DisableDamageText", 1.5f);
    }

    private void Update()
    {
        transform.position = pos;
        //transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
    }

    public void DisableDamageText()
    {
        FxManager.Instance.DisableDamageText(this);
    }

    public void SetText(float damage, AttackType attackType)
    {
        dmg_Text.text = ((int)damage).ToString();

        switch (attackType)
        {
            case AttackType.None:
                dmg_Text.colorGradient = new VertexGradient(colors[0], colors[0], Color.white, Color.white);
                break;
            case AttackType.Physical:
                dmg_Text.colorGradient = new VertexGradient(colors[1], colors[1], Color.white, Color.white);
                break;
            case AttackType.Magical:
                dmg_Text.colorGradient = new VertexGradient(colors[2], colors[2], Color.white, Color.white);
                break;
            case AttackType.Fire:
                dmg_Text.colorGradient = new VertexGradient(colors[3], colors[3], Color.white, Color.white);

                break;
            default:
                break;
        }
    }
}
