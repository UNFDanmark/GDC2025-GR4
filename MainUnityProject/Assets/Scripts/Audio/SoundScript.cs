using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    int amount = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioSource MakeNewSource()
    {
        GameObject obj = Instantiate(new GameObject("AudioObj" + amount++), transform);
        AudioSource newSource = obj.AddComponent<AudioSource>();
        return newSource;
    }
    
}
