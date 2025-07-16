using System;
using UnityEngine;

public class MortalityScript : MonoBehaviour
{
    public float deathImpulse;
    public float boneBreakImpulse;
    public float hitSoundImpulse;
    
    DeathScript deathScript;
    AudioSource hittingStuffAudioSource;

    public AudioClip hittingObstacleSound;
    public AudioClip boneBreakSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScript = GetComponent<DeathScript>();
        hittingStuffAudioSource = GetComponent<SoundScript>().MakeNewSource();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle")) return;
        print(other.impulse.magnitude);
        if (other.impulse.magnitude > deathImpulse)
        {
            hittingStuffAudioSource.PlayOneShot(boneBreakSound);
            deathScript.DieBad();
        }else if (other.impulse.magnitude > hitSoundImpulse)
        {
            hittingStuffAudioSource.PlayOneShot(hittingObstacleSound);
            if (other.impulse.magnitude > boneBreakImpulse)
            {
                hittingStuffAudioSource.PlayOneShot(boneBreakSound);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Deadly"))
        {
            deathScript.DieBad();
        }
    }
}
