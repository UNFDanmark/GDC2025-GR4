using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectableScript : MonoBehaviour
{
    public InputAction collectAction;
    InventoryManager inventoryManager;
    TextScript textScript;
    SfxScript sfxScript;
    public AudioClip pickUpSound;
    bool isInRange = false;
    bool hasBeenInRange = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectAction.Enable();
        inventoryManager = GameObject.FindWithTag("Player").GetComponent<InventoryManager>();
        sfxScript = GameObject.FindWithTag("Player").GetComponent<SfxScript>();
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
        textScript.ShowText("press E bruh");
    }

    void DuringInRange()
    {
        
    }

    void PickUp()
    {
        inventoryManager.PickUpGlider();
        ExitRange();
        Destroy(gameObject);
    }
}
