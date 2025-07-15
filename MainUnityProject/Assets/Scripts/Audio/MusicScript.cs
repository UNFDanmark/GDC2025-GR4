using UnityEngine;

public class MusicScript : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip startingSong;
    public float initVolume;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<SoundScript>().MakeNewSource();
        SetMusicVolume(initVolume);
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
