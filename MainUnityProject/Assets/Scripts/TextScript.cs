using System.Net.Mime;
using TMPro;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public GameObject textCanvas;
    TMP_Text textComponent;
    public float textFade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowText(string text)
    {
        textComponent.text = text;
        
    }
}
