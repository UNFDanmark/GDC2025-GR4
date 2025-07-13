using System;
using UnityEngine;

public class CorruptionScript : MonoBehaviour
{
    public float speedNeeded;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.impulse.magnitude >= speedNeeded)
            {
                OnSmash();
            }
        }
    }

    void OnSmash()
    {
        print("smash");
    }
}
