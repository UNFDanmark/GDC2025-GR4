using UnityEngine;

public class JumpDetectionScript : MonoBehaviour
{
    bool canJump;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanJump()
    {
        return canJump;
    }
    
    // Triggers when player touches a collider with IsTrigger sat to true, triggers during physics step
    void OnTriggerStay(Collider other)
    {
        
        // Does collider have Jumpable tag? If so set jumpable variable (set to false during start of physics step)
        if (other.transform.CompareTag("Obstacle"))
        {
            canJump = true;
        }
    }

    // Sets the intermediate checking values, called during physics step
    void FixedUpdate()
    {
        canJump = false;
    }
}
