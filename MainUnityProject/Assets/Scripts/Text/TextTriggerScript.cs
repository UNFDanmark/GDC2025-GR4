using System;
using UnityEngine;

public class TextTriggerScript : MonoBehaviour
{
    public string textShown;
    
    TextScript textScript;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textScript = GameObject.FindWithTag("Player").GetComponent<TextScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textScript.ShowText(textShown);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textScript.StopText();
        }   
    }
}
