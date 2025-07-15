using UnityEngine;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    public struct SaveState
    {
        public Vector3 playerPosition;

        public SaveState(Vector3 playerPosition)
        {
            this.playerPosition = playerPosition;
        }
    }

    bool dead = false;

    SaveState saveState;

    GameObject player;
    public GameObject respawnCanvas;
        
    public GameObject endCanvas;
    
    public float fadeToWhiteSpeed;
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
        saveState = new SaveState(new Vector3(-7.08f, 11.89f, 3.4f));
        
        fadeToWhiteImage = endCanvas.GetComponentInChildren<Image>();
        musicScript = GetComponent<MusicScript>();
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
        musicScript.SetMusic(endingSong);
    }

    public void Respawn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        respawnCanvas.SetActive(false);
        dead = false;
        player.transform.position = saveState.playerPosition;
        player.transform.rotation = Quaternion.identity;
    }
}
