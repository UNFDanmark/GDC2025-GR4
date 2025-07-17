using System.Net.Mime;
using TMPro;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public GameObject textCanvas;
    public float textFadeTime;
    public float textStayTime;
    TMP_Text textComponent;
    DeathScript deathScript;
    bool isShowing;
    bool isRemoving;
    float currentFade;
    float textCooldown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent = textCanvas.GetComponentInChildren<TMP_Text>();
        deathScript = GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deathScript.IsDead())
        {
            textCanvas.SetActive(false);
            isRemoving = false;
            isShowing = false;
            currentFade = 0;
            textComponent.color = new Color(1, 1, 1, 0);
            return;
        }

        if (textCooldown > 0)
        {
            textCooldown -= Time.deltaTime;
        }
        else if(!(textCooldown < -100))
        {
            StopText();
            textCooldown = -200;
        }
        if (isShowing)
        {
            currentFade += Time.deltaTime / textFadeTime;
            textComponent.color = new Color(1, 1, 1, currentFade);
            if (currentFade > 1)
            {
                currentFade = 1;
                isShowing = false;
            }
        }
        if (isRemoving)
        {
            currentFade -= Time.deltaTime / textFadeTime;
            textComponent.color = new Color(1, 1, 1, currentFade);
            if (currentFade < 0)
            {
                currentFade = 0;
                isRemoving = false;
            }
        }
        
    }
    
    public void ShowText(string text)
    {
        print("showing");
        textComponent.text = text;
        isShowing = true;
        isRemoving = false;
        textCanvas.SetActive(true);
        textCooldown = textStayTime;
    }

    public void StopText()
    {
        print("stopping");
        isShowing = false;
        isRemoving = true;
    }
}
