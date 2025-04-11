using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public int startBGMIndex;
    #region BGM
    [Header("BGM (clips 순서 유의)")]
    public AudioClip[] bgm_Clips;

    AudioSource bgm_Player;
    #endregion
    #region SFX
    [Header("SFX (clips 순서 유의)")]
    public AudioClip[] sfx_Clips;

    //채널 개수(한 번에 재생할 수 있는 효과음의 개수)
    public int channels;
    AudioSource[] sfx_Players;
    int channel_Index = 0;
    #endregion
    public AudioMixer mixer;
    public Slider[] sliders;

    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
        PlayerBgm((BGM_Enum)startBGMIndex);

        LoadVolumes();
        for (int i = 0; i < sliders.Length; i++)
        {
            float dbValue;
            mixer.GetFloat(((EMixer)i).ToString(), out dbValue);
            sliders[i].value = Mathf.Pow(10, dbValue / 20f);
        }
    }

    public void Init()
    {
        //bgm 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;

        bgm_Player = bgmObject.AddComponent<AudioSource>();
        bgm_Player.playOnAwake = false;
        bgm_Player.loop = true;
        bgm_Player.outputAudioMixerGroup = mixer.FindMatchingGroups(EMixer.BGM.ToString())[0];

        //sfx 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfx_Players = new AudioSource[channels];

        for (int i = 0; i < channels; i++)
        {
            sfx_Players[i] = sfxObject.AddComponent<AudioSource>();
            sfx_Players[i].playOnAwake = false;
            sfx_Players[i].outputAudioMixerGroup = mixer.FindMatchingGroups(EMixer.SFX.ToString())[0];
        }
    }

    public void PlayerBgm(BGM_Enum bgm_Enum)
    {
        bgm_Player.clip = bgm_Clips[(int)bgm_Enum];
        bgm_Player.volume = 0.5f;
        

        bgm_Player.Play();
    }

    public void PlayerSfx(SFX_Enum sfx_enum)
    {
        for (int i = 0; i < channels; i++)
        {
            //마지막으로 사용한 channel의 index부터 탐색
            int loopIndex = (i + channel_Index) % channels;

            //사용중이지 않은 채널 탐색
            if (sfx_Players[loopIndex].isPlaying)
                continue;

            //채널에 클립 부여
            channel_Index = loopIndex;
            sfx_Players[loopIndex].clip = sfx_Clips[(int)sfx_enum];

            //오디오 재생
            sfx_Players[loopIndex].Play();
            break;
        }
    }

    public AudioSource PlayerSfx_Source(SFX_Enum sfx_enum)
    {
        for (int i = 0; i < channels; i++)
        {
            //마지막으로 사용한 channel의 index부터 탐색
            int loopIndex = (i + channel_Index) % channels;

            //사용중이지 않은 채널 탐색
            if (sfx_Players[loopIndex].isPlaying)
                continue;

            //채널에 클립 부여
            channel_Index = loopIndex;
            sfx_Players[loopIndex].clip = sfx_Clips[(int)sfx_enum];

            //오디오 재생
            sfx_Players[loopIndex].Play();
            return sfx_Players[loopIndex];
        }
        return null;
    }

    //int 인덱스를 통해 sfx를 재생하는 함수 (버튼 OnClick에 사용)
    public void PlaySfx_By_Int(int sfx_int)
    {
        PlayerSfx((SFX_Enum)sfx_int);
    }

    public void OnSetVolume_Master(float volume)
    {
        //오디오 믹서 값이 -80 ~ 0의 log이므로 0.0001~ 의 log10 * 20을 사용
        mixer.SetFloat(EMixer.Master.ToString(), Mathf.Log10(volume) * 20);
    }

    public void OnSetVolume_BGM(float volume)
    {
        //오디오 믹서 값이 -80 ~ 0의 log이므로 0.0001~ 의 log10 * 20을 사용
        mixer.SetFloat(EMixer.BGM.ToString(), Mathf.Log10(volume) * 20);
    }

    public void OnSetVolume_SFX(float volume)
    {
        //오디오 믹서 값이 -80 ~ 0의 log이므로 0.0001~ 의 log10 * 20을 사용
        mixer.SetFloat(EMixer.SFX.ToString(), Mathf.Log10(volume) * 20);
    }

    //음량 설정을 저장하는 함수
    public void OnSaveVolumes()
    {
        for (int i = 0; i < Enum.GetValues(typeof(EMixer)).Length; i++)
        {
            float dbValue;
            mixer.GetFloat(((EMixer)i).ToString(), out dbValue);
            PlayerPrefs.SetFloat(((EMixer)i).ToString(), dbValue);
        }
    }

    //음량 설정을 불러오는 함수
    public void LoadVolumes()
    {
        for (int i = 0; i < Enum.GetValues(typeof(EMixer)).Length; i++)
        {
            float dbValue = PlayerPrefs.GetFloat(((EMixer)i).ToString());
            mixer.SetFloat(((EMixer)i).ToString(), dbValue);
        }
    }
}
