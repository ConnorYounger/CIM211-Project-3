using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip music;

    public AudioSource[] audioSources;
    public float[] audioSourcesVolume;

    [Header("Ambiant Sounds")]
    public AudioClip[] ambiantSounds;
    public AudioSource ambiantSoundsAudioSource;
    public float ambiantPlayTime;
    private float playTime;

    private void Start()
    {
        SetStartVolumes();

        playTime = ambiantPlayTime;

        if(ambiantSoundsAudioSource)
            StartCoroutine("PlayAmbiantSound");
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

    IEnumerator PlayAmbiantSound()
    {
        yield return new WaitForSeconds(playTime);

        int rand = Random.Range(0, ambiantSounds.Length);
        ambiantSoundsAudioSource.clip = ambiantSounds[rand];
        ambiantSoundsAudioSource.Play();

        float r = Random.Range(-(ambiantPlayTime / 2), (ambiantPlayTime / 2));
        playTime = ambiantPlayTime + r + ambiantSoundsAudioSource.clip.length;

        StartCoroutine("PlayAmbiantSound");
    }

    public void UpdateMusicVolume()
    {
        if(audioSource)
            audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
    }
}
