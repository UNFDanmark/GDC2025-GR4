using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool hasGlider = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpGlider()
    {
        hasGlider = true;
    }

    public bool HasGlider()
    {
        return hasGlider;
    }
}
