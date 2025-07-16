using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    [Serializable]
    public struct SaveState
    {
        public GameObject playerPosition;
        public bool hasGlider;
    }

    bool dead = false;

    SaveState saveState;

    GameObject player;
    InventoryManager inventoryManager;
    public GameObject respawnCanvas;
        
    public GameObject endCanvas;
    public RectTransform scrollTransform;
    
    public float fadeToWhiteSpeed;
    public float endingSongCrossFadeTime;
    public float endingSongVolume;
    public SaveState spawnState;
    public float scrollSpeed;
    Image fadeToWhiteImage;
    MusicScript musicScript;
    public AudioClip endingSong;
    
    bool isEnding = false;
    float fadeToWhite = 0f;
    // Start is called once bef
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        respawnCanvas.SetActive(false);
        saveState = spawnState;
        
        
        fadeToWhiteImage = endCanvas.GetComponentInChildren<Image>();
        inventoryManager = GetComponent<InventoryManager>();
        musicScript = GetComponent<MusicScript>();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        print(respawnCanvas.activeSelf);
        if (isEnding)
        {
            print(fadeToWhite);
            fadeToWhite += fadeToWhiteSpeed * Time.deltaTime;
            fadeToWhiteImage.color = new Color(1, 1, 1, fadeToWhite);
            scrollTransform.anchoredPosition =
                new Vector2(0, scrollTransform.anchoredPosition.y + scrollSpeed * Time.deltaTime);
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    public void Save(SaveState state)
    {
        saveState = state;
    }

    [ContextMenu("DIEEEE")]
    public void DieBad()
    {
        dead = true;
        respawnCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    
    [ContextMenu("END YOUR LIFE")]
    public void DieGood()
    {
        endCanvas.SetActive(true);
        isEnding = true;
        dead = true;
        musicScript.SwitchMusic(endingSong, endingSongCrossFadeTime, endingSongVolume, false);
    }

    public void Respawn()
    {
        print("TryingToRespawn");
        Cursor.lockState = CursorLockMode.Locked;
        respawnCanvas.SetActive(false);
        print("CanvasGone");
        print(respawnCanvas.activeSelf);
        dead = false;
        player.transform.position = saveState.playerPosition.transform.position;
        player.transform.rotation = saveState.playerPosition.transform.rotation;
        inventoryManager.SetGlider(saveState.hasGlider);
    }
}
