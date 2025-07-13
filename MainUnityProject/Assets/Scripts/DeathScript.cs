using UnityEngine;

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

    public bool dead = false;

    SaveState saveState;

    GameObject player;
    public GameObject respawnCanvas;
        
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        respawnCanvas.SetActive(false);
        saveState = new SaveState(new Vector3(-7.08f, 11.89f, 3.4f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(SaveState state)
    {
        saveState = state;
    }

    [ContextMenu("DIEEEE")]
    public void Die()
    {
        dead = true;
        respawnCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
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
