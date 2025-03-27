using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

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

    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
        PlayerBgm(BGM_Enum.Map_1);
    }

    public void Init()
    {
        //bgm 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;

        bgm_Player = bgmObject.AddComponent<AudioSource>();
        bgm_Player.playOnAwake = false;
        bgm_Player.loop = true;

        //sfx 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfx_Players = new AudioSource[channels];

        for (int i = 0; i < channels; i++)
        {
            sfx_Players[i] = sfxObject.AddComponent<AudioSource>();
            sfx_Players[i].playOnAwake = false;
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

    //int 인덱스를 통해 sfx를 재생하는 함수 (버튼 OnClick에 사용)
    public void PlaySfx_By_Int(int sfx_int)
    {
        PlayerSfx((SFX_Enum)sfx_int);
    }
}
