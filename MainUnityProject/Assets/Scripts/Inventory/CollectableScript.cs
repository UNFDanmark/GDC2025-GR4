using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectableScript : MonoBehaviour
{
    public InputAction collectAction;
    public AudioClip pickUpSound;
    public string gliderPickUpText;
    
    InventoryManager inventoryManager;
    TextScript textScript;
    AudioSource audioSource;
    bool isInRange = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectAction.Enable();
        inventoryManager = GameObject.FindWithTag("Player").GetComponent<InventoryManager>();
        audioSource = GameObject.FindWithTag("Player").GetComponent<SoundScript>().MakeNewSource();
        textScript = GameObject.FindWithTag("Player").GetComponent<TextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            DuringInRange();
            if (collectAction.IsPressed())
            {
                PickUp();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            EnterRange();
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            ExitRange();
        }
    }

    void ExitRange()
    {
        textScript.StopText();
    }

    void EnterRange()
    {
        textScript.ShowText(gliderPickUpText);
    }

    void DuringInRange()
    {
        
    }

    void PickUp()
    {
        inventoryManager.PickUpGlider();
        audioSource.PlayOneShot(pickUpSound);
        ExitRange();
        Destroy(gameObject);
    }
}
