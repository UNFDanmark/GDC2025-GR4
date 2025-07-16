using System;
using UnityEngine;

public class HeartbeatScript : MonoBehaviour
{
    public float heartbeatTime;
    public float heartCooldown;
    AudioSource audioSource;
    public AudioClip heartbeat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GameObject.FindWithTag("Player").GetComponent<SoundScript>().MakeNewSource();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (heartCooldown > 0)
        {
            heartCooldown -= Time.deltaTime;
        }
        else
        {
            heartCooldown = heartbeatTime;
            audioSource.PlayOneShot(heartbeat);
        }
    }
}
