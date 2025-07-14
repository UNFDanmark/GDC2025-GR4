using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectableScript : MonoBehaviour
{
    public InputAction collectAction;
    InventoryManager inventoryManager;
    SfxScript sfxScript;
    public AudioClip pickUpSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectAction.Enable();
        inventoryManager = GameObject.FindWithTag("Player").GetComponent<InventoryManager>();
        sfxScript = GameObject.FindWithTag("Player").GetComponent<SfxScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DuringInRange();
            if (collectAction.IsPressed())
            {
                PickUp();
            }
        }
    }

    void DuringInRange()
    {
        //TODO
    }

    void PickUp()
    {
        inventoryManager.PickUpGlider();
        Destroy(gameObject);
        sfxScript.PlaySfx(pickUpSound);
    }
}
