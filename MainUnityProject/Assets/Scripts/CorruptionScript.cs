using System;
using UnityEngine;

public class CorruptionScript : MonoBehaviour
{
    public float speedNeeded;
    DeathScript deathScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScript = GameObject.FindWithTag("Player").GetComponent<DeathScript>();
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
        deathScript.DieGood();
    }
}
