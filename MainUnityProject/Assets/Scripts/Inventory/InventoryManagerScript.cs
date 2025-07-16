using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool hasGlider = false;
    public GameObject gliderObj;
    public GameObject gliderIconCanvas;
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
        gliderObj.SetActive(true);
        gliderIconCanvas.SetActive(true);
    }

    public bool HasGlider()
    {
        return hasGlider;
    }
}
