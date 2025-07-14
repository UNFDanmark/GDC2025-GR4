using UnityEngine;

public class SfxScript : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySfx(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
        
    }
    
}
