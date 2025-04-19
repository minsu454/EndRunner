using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Common.SceneEx;

public class SoundManager : MonoBehaviour, IInit
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public Dictionary<SfxType, AudioClip> sfxClipDic = new Dictionary<SfxType, AudioClip>();
    public Dictionary<BgmType, AudioClip> bgmClipDic = new Dictionary<BgmType, AudioClip>();

    public void OnCompleteSceneLoad(string sceneName) {
        switch (sceneName) {
            case "Game":
                PlayBGM(BgmType.Game);
                break;
        }
    }

    public void PlaySFX(SfxType type) {
        sfxSource.PlayOneShot(sfxClipDic[type]);
    }

    public void PlayBGM(BgmType type) {
        bgmSource.clip = bgmClipDic[type];
        bgmSource.Play();
    }

    public void SetBGM(float endValue) {
        bgmSource.DOFade(endValue * PlayerPrefs.GetFloat("BGM", 1), 0.1f);
    }

    public void SetVolume(string name, float volume) {
        if (name == "BGM") {
            bgmSource.volume = volume;
            PlayerPrefs.SetFloat("BGM", volume);
        }
        else if (name == "SFX") {
            sfxSource.volume = volume;
            PlayerPrefs.SetFloat("SFX", volume);
        }

    }

    public void Init()
    {
        GameObject bgmObj = new GameObject("BGM");
        GameObject sfxObj = new GameObject("SFX");

        bgmObj.transform.SetParent(transform);
        sfxObj.transform.SetParent(transform);

        bgmSource = bgmObj.AddComponent<AudioSource>();
        sfxSource = sfxObj.AddComponent<AudioSource>();

        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        sfxSource.playOnAwake = false;

        AudioClip[] sfxAudioClips = Resources.LoadAll<AudioClip>("Sounds/SFX");
        for (int i = 0; i < sfxAudioClips.Length; i++)
        {
            try
            {
                SfxType sfxType = (SfxType)Enum.Parse(typeof(SfxType), sfxAudioClips[i].name);
                sfxClipDic.Add(sfxType, sfxAudioClips[i]);
            }
            catch
            {
                Debug.LogWarning("Need SfxType Enum : " + sfxAudioClips[i].name);
            }
        }

        AudioClip[] bgmAudioClips = Resources.LoadAll<AudioClip>("Sounds/BGM");
        for (int i = 0; i < bgmAudioClips.Length; i++)
        {
            try
            {
                BgmType bgmType = (BgmType)Enum.Parse(typeof(BgmType), bgmAudioClips[i].name);
                bgmClipDic.Add(bgmType, bgmAudioClips[i]);
            }
            catch
            {
                Debug.LogWarning("Need BgmType Enum : " + bgmAudioClips[i].name);
            }
        }
        bgmSource.volume = PlayerPrefs.GetFloat("BGM", 1);
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 1);

        SceneJobLoader.Add(LoadPriorityType.Sound, OnCompleteSceneLoad);
    }
}

public enum SfxType {
    Die,
    Button,
    Laser,

}

public enum BgmType {
    Title,
    Game,
}
