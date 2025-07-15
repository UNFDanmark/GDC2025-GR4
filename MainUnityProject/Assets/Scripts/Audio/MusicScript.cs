using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public AudioClip startingSong;
    public float initVolume;
    public float initPitch;
    
    AudioSource audioSource;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<SoundScript>().MakeNewSource();
        SetMusicVolume(initVolume);
        SetMusicPitch(initPitch);
        SetMusic(startingSong);
        StartMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetMusic(AudioClip song)
    {
        audioSource.clip = song;
        StartMusic();
        
    }
}
