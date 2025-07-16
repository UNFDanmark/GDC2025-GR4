using System;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public DeathScript.SaveState saveState;
    
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            deathScript.Save(saveState);
        }
        
    }
}
