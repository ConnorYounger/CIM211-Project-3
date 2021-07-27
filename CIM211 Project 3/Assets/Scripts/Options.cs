using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public float musicVolume;
    public float audioVolume;

    public Slider musicSlider;
    public Slider audioSlider;

    public void UpdateVolumeSliders()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        audioVolume = PlayerPrefs.GetFloat("audioVolume");

        musicSlider.value = musicVolume;
        audioSlider.value = audioVolume;
    }

    public void ModifyAudioVolume()
    {
        audioVolume = audioSlider.value;
    }

    public void ModifyMusicVolume()
    {
        musicVolume = musicSlider.value;
    }

    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("audioVolume", audioVolume);
    }
}
