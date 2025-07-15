using System;
using UnityEngine;

public class MortalityScript : MonoBehaviour
{
    public float ImpulseForDeath;
    
    DeathScript deathScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScript = GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        print(other.impulse.magnitude);
        if (other.impulse.magnitude > ImpulseForDeath)
        {
            deathScript.DieBad();
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
