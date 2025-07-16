using System.Diagnostics;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public AudioClip startingSong;
    public float initVolume;
    public float initPitch;

    float crossFadeProgress;
    float crossFadeRate;
    float targetVolume;
    float previousVolume;
    AudioSource audioSource;
    AudioSource audioSourceOther;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<SoundScript>().MakeNewSource();
        audioSourceOther = GetComponent<SoundScript>().MakeNewSource();
        SwitchMusic(startingSong, 0, initVolume, false);
        StartMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (crossFadeProgress < 1f)
        {
            crossFadeProgress += crossFadeRate * Time.deltaTime;
            if (crossFadeProgress >= 1f)
            {
                (audioSource, audioSourceOther) = (audioSourceOther, audioSource);
                audioSourceOther.volume = 0;
                audioSource.volume = targetVolume;
                crossFadeProgress = 1f;
            }
            else
            {
                audioSourceOther.volume = crossFadeProgress * targetVolume;
                audioSource.volume = (1f - crossFadeProgress) * previousVolume;
            }
        }
    }
    

    public void StopMusic()
    {
        audioSource.Pause();
    }

    public void StartMusic()
    {
        audioSource.Play();
    }

    public void SetMusicPitch(float level)
    {
        audioSource.pitch = level;
    }

    public void SetMusicVolume(float level)
    {
        audioSource.volume = level;
    }

    public void SwitchMusic(AudioClip song, float crossFadeTime, float volume, bool keepOffset)
    {
        targetVolume = volume;
        if (crossFadeProgress != 1f || crossFadeTime == 0f)
        {
            crossFadeProgress = 1;
            audioSourceOther.volume = 0;
            audioSource.volume = volume;
            float time = audioSource.time;
            audioSource.clip = song;
            if(keepOffset) audioSource.time = time;
            StartMusic();
        }
        else
        {
            crossFadeRate = 1 / crossFadeTime;
            audioSourceOther.clip = song;
            audioSourceOther.Play();
            previousVolume = audioSource.volume;
            if(keepOffset) audioSourceOther.time = audioSource.time;
            crossFadeProgress = 0f;
        }
        
        
    }
}
