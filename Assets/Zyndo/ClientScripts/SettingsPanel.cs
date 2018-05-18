using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsPanel : MonoBehaviour {

    public Slider MusicSlider, SFXSlider, VoiceSlider;
    public Slider ParallaxMultiplierSlider;

    public Toggle MusicToggle, SFXToggle, VoiceToggle, ParallaxToggle;

    float musicVolume, sfxVolume, voiceVolume;
    float parallaxMultiplierNum;

    float orginalParallaxMult;

    List<AudioSource> musicSources, sfxSources, voiceSources;

    // Use this for initialization
    void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", .5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", .5f);
        voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", .5f);

        parallaxMultiplierNum = PlayerPrefs.GetFloat("ParallaxMultiplier", 1f);

        orginalParallaxMult = ParallaxSettings.instance.ParallaxMulti;

        AudioPartGroup[] audioGroups = GameObject.FindObjectsOfType<AudioPartGroup>();

        musicSources = new List<AudioSource>();
        sfxSources = new List<AudioSource>();
        voiceSources = new List<AudioSource>();

        for (int i = 0; i < audioGroups.Length; i++)
        {
            if(audioGroups[i].type == AudioType.Music)
            {
                for(int j = 0; j < audioGroups[i].transform.childCount; j++)
                {
                    if(audioGroups[i].transform.GetChild(j).GetComponent<AudioSource>())
                    {
                        musicSources.Add(audioGroups[i].transform.GetChild(j).GetComponent<AudioSource>());
                    }
                }
            }
            if (audioGroups[i].type == AudioType.FX)
            {
                for (int j = 0; j < audioGroups[i].transform.childCount; j++)
                {
                    if (audioGroups[i].transform.GetChild(j).GetComponent<AudioSource>())
                    {
                        sfxSources.Add(audioGroups[i].transform.GetChild(j).GetComponent<AudioSource>());
                    }
                }
            }
            if (audioGroups[i].type == AudioType.Voice)
            {
                for (int j = 0; j < audioGroups[i].transform.childCount; j++)
                {
                    if (audioGroups[i].transform.GetChild(j).GetComponent<AudioSource>())
                    {
                        voiceSources.Add(audioGroups[i].transform.GetChild(j).GetComponent<AudioSource>());
                    }
                }
            }

        }

        MusicSlider.value = musicVolume * 10;
        SFXSlider.value = sfxVolume * 10;
        VoiceSlider.value = voiceVolume * 10;
        ParallaxMultiplierSlider.value = parallaxMultiplierNum * 5;

        MusicToggle.isOn = musicVolume > 0;
        SFXToggle.isOn = sfxVolume > 0;
        VoiceToggle.isOn = voiceVolume > 0;
        ParallaxToggle.isOn = parallaxMultiplierNum > 0;

        SetMusicVolume();
        SetSFXVolume();
        SetVoiceVolume();
        SetParallaxMultiplier();

    }


    public void SetMusicVolume()
    {
        musicVolume = MusicSlider.value / 10;

        for (int i = 0; i < musicSources.Count; i++)
        {
            musicSources[i].volume = musicVolume;
        }
    }

    public void SetMusicVolume(bool up)
    {
        MusicSlider.value += up ? 1 : -1;

        musicVolume = MusicSlider.value / 10;

        for (int i = 0; i < musicSources.Count; i++)
        {
            musicSources[i].volume = musicVolume;
        }
    }

    public void ToggleMusicVolume(Toggle toggle)
    {
        musicVolume = toggle.isOn ? 1 : 0;

        for (int i = 0; i < musicSources.Count; i++)
        {
            musicSources[i].volume = musicVolume;
        }
    }

    public void SetSFXVolume()
    {
        sfxVolume = SFXSlider.value / 10;

        for (int i = 0; i < sfxSources.Count; i++)
        {
            sfxSources[i].volume = sfxVolume;
        }
    }

    public void SetSFXVolume(bool up)
    {
        SFXSlider.value += up ? 1 : -1;

        sfxVolume = SFXSlider.value / 10;

        for (int i = 0; i < sfxSources.Count; i++)
        {
            sfxSources[i].volume = sfxVolume;
        }
    }

    public void ToggleSFXVolume(Toggle toggle)
    {
        sfxVolume = toggle.isOn ? 1 : 0;

        for (int i = 0; i < musicSources.Count; i++)
        {
            musicSources[i].volume = musicVolume;
        }
    }

    public void SetVoiceVolume()
    {
        voiceVolume = VoiceSlider.value / 10;

        for (int i = 0; i < voiceSources.Count; i++)
        {
            voiceSources[i].volume = voiceVolume;
        }
    }

    public void SetVoiceVolume(bool up)
    {
        VoiceSlider.value += up ? 1 : -1;

        voiceVolume = VoiceSlider.value / 10;

        for (int i = 0; i < voiceSources.Count; i++)
        {
            voiceSources[i].volume = voiceVolume;
        }
    }

    public void ToggleVoiceVolume(Toggle toggle)
    {
        voiceVolume = toggle.isOn ? 1 : 0;

        for (int i = 0; i < musicSources.Count; i++)
        {
            musicSources[i].volume = musicVolume;
        }
    }

    public void SetParallaxMultiplier()
    {
        parallaxMultiplierNum = ParallaxMultiplierSlider.value / 5;
        ParallaxSettings.instance.ParallaxMulti = orginalParallaxMult * parallaxMultiplierNum;
    }

    public void SetParallaxMultiplier(bool up)
    {
        ParallaxMultiplierSlider.value += up ? 1 : -1;

        parallaxMultiplierNum = ParallaxMultiplierSlider.value / 5;
        ParallaxSettings.instance.ParallaxMulti = orginalParallaxMult * parallaxMultiplierNum;
    }

    public void ToggleParallaxMultiplier(Toggle toggle)
    {
        parallaxMultiplierNum = toggle.isOn ? 1 : 0;
        ParallaxSettings.instance.ParallaxMulti = orginalParallaxMult * parallaxMultiplierNum;
    }

    void SetPlayerPrefs()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolume);

        PlayerPrefs.SetFloat("ParallaxMultiplier", parallaxMultiplierNum);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SetPlayerPrefs();
    }

    private void OnApplicationQuit()
    {
        SetPlayerPrefs();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SetPlayerPrefs();
    }



}
