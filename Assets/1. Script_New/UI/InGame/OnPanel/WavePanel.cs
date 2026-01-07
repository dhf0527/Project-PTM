using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WavePanel : MonoBehaviour
{
    Animator anim;
    [Header("0: 현재 웨이브, 1: 다음 웨이브")]
    public TMP_Text[] wave_Texts = new TMP_Text[2];
    int wave = 0;

    static readonly string DoMove = "Move";

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }

    public void ToNextWave(int cur_Wave)
    {
        OnWaveTextSet(cur_Wave);
        wave++;
        anim.SetTrigger(DoMove);
    }
    
    public void OnWaveTextSet(int cur_Wave)
    {
        wave = cur_Wave;
        wave_Texts[0].text = $"WAVE {wave + 1}";
        wave_Texts[1].text = $"WAVE {wave + 2}";
    }
}
