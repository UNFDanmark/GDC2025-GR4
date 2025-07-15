using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    List<AudioSource> sources = new List<AudioSource>();
    DeathScript deathScript;
    int amount = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScript = GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deathScript.IsDead())
        {
            for (int i = 0; i < sources.Count; i++)
            {
                sources[i].Stop();
            }
        }
    }

    public AudioSource MakeNewSource()
    {
        GameObject obj = Instantiate(new GameObject("AudioObj" + amount++), transform);
        AudioSource newSource = obj.AddComponent<AudioSource>();
        sources.Add(newSource);
        return newSource;
    }
}
