using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    bool hasGlider = false;
    public GameObject gliderObj;
    public GameObject gliderIconCanvas;

    DeathScript deathScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScript = GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deathScript.IsDead())
        {
            gliderIconCanvas.SetActive(false);
            return;
        }
        else
        {
            gliderObj.SetActive(hasGlider);
        }
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

    public void SetGlider(bool state)
    {
        hasGlider = state;
        gliderObj.SetActive(state);
        gliderIconCanvas.SetActive(state);
    }
}
