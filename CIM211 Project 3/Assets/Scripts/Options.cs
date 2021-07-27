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
        musicSlider.value = musicVolume;
        audioSlider.value = audioVolume;
    }

    public void ModifySliderValues()
    {
        musicVolume = musicSlider.value;
        audioVolume = audioSlider.value;
    }

    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("audioVolume", musicVolume);
    }
}
