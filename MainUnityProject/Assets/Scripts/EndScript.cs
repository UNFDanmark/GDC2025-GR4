using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    public GameObject endCanvas;
    public float fadeToWhiteSpeed;
    Image fadeToWhiteImage;
    
    bool isEnding = false;
    DeathScript deathScript;
    float fadeToWhite = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeToWhiteImage = endCanvas.GetComponentInChildren<Image>();
        deathScript = GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnding)
        {
            print(fadeToWhite);
            fadeToWhite += fadeToWhiteSpeed * Time.deltaTime;
            fadeToWhiteImage.color = new Color(1, 1, 1, fadeToWhite);
        }
        
    }

    [ContextMenu("END YOUR LIFE")]
    public void TriggerEnding ()
    {
        endCanvas.SetActive(true);
        isEnding = true;
        deathScript.DieGood();
        
    }
}
