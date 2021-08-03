using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip music;

    public AudioSource[] audioSources;
    public float[] audioSourcesVolume;

    private void Start()
    {
        SetStartVolumes();
    }

    void SetStartVolumes()
    {
        if (audioSources.Length > 0)
        {
            audioSourcesVolume = new float[audioSources.Length];

            for(int i =  0; i < audioSources.Length; i++)
            {
                audioSourcesVolume[i] = audioSources[i].volume;
            }
        }

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
            for(int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].volume = audioSourcesVolume[i] * PlayerPrefs.GetFloat("audioVolume");
            }

            //foreach (AudioSource a in audioSources)
            //{
            //    a.volume = PlayerPrefs.GetFloat("audioVolume");
            //}
        }
    }

    public void UpdateMusicVolume()
    {
        if(audioSource)
            audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
    }
}
