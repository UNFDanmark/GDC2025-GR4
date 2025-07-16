using UnityEngine;

public class JumpDetectionScript : MonoBehaviour
{
    public float coyoteTime;
    public float coyoteDisableAfterJump;
    float coyoteCooldown;
    float justJumpedTime;
    bool canJump;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coyoteCooldown -= Time.deltaTime;
        justJumpedTime -= coyoteDisableAfterJump;
    }

    public bool CanJump()
    {
        return canJump || (coyoteCooldown > 0 && coyoteDisableAfterJump < 0);
    }

    public void Jump()
    {
        coyoteCooldown = 0;
        justJumpedTime = coyoteDisableAfterJump;
    }

    public bool OnJumpableGround()
    {
        return canJump;
    }
    
    // Triggers when player touches a collider with IsTrigger sat to true, triggers during physics step
    void OnTriggerStay(Collider other)
    {
        
        // Does collider have Jumpable tag? If so set jumpable variable (set to false during start of physics step)
        if (other.transform.CompareTag("Obstacle") || other.transform.CompareTag("Rock") || other.transform.CompareTag("Grass"))
        {
            canJump = true;
            coyoteCooldown = coyoteTime;
        }
    }

    // Sets the intermediate checking values, called during physics step
    void FixedUpdate()
    {
        canJump = false;
    }
}
