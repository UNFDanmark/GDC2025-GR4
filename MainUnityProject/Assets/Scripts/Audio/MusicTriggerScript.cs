using System;
using UnityEngine;

public class MusicTriggerScript : MonoBehaviour
{
    public AudioClip song;
    public float crossFadeTime;
    public float volume;
    public bool keepOffSet;

    MusicScript musicScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicScript = GameObject.FindWithTag("Player").GetComponent<MusicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            musicScript.SwitchMusic(song, crossFadeTime, volume, keepOffSet);
        }
    }
}
