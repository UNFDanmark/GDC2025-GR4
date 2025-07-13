using System;
using UnityEngine;

public class MortalityScript : MonoBehaviour
{
    DeathScript deathScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScript = GameObject.FindWithTag("God").GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.CompareTag("Deadly"))
        {
            deathScript.Die();
        }
    }
}
