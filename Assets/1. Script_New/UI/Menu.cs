using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public List<Slider> sliders;

    private void Start()
    {
        //슬라이더 값 맞추기
        for (int i = 0; i < sliders.Count; i++)
        {
            float dbValue;
            AudioManager.Instance.mixer.GetFloat(((EMixer)i).ToString(), out dbValue);
            sliders[i].value = Mathf.Pow(10, dbValue / 20f);
        }
    }

    public void OnSetVolume_Master(float volume)
    {
        AudioManager.Instance.OnSetVolume_Master(volume);
    }

    public void OnSetVolume_BGM(float volume)
    {
        AudioManager.Instance.OnSetVolume_BGM(volume);
    }

    public void OnSetVolume_SFX(float volume)
    {
        AudioManager.Instance.OnSetVolume_SFX(volume);
    }
}
