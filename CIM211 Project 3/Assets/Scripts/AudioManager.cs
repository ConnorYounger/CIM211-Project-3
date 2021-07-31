using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip music;

    public AudioSource[] audioSources;

    private void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        UpdateAudioVolume();
        UpdateMusicVolume();
    }

    public void UpdateAudioVolume()
    {
        if(audioSources.Length > 0)
        {
            foreach (AudioSource a in audioSources)
            {
                a.volume = PlayerPrefs.GetFloat("audioVolume");
            }
        }
    }

    public void UpdateMusicVolume()
    {
        if(audioSource)
            audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
    }
}
