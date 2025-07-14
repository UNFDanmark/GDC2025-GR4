using System;
using System.Numerics;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovementScript : MonoBehaviour
{
    // InputActions for moving (WASD) and jumping (Space), jump button also used for glider
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction sprintAction;
    
    //Audio & Sfx
    public AudioClip jumpSound;
    public AudioClip gliderPulloutSound;
    
    // Physics for when in walk mode
    public float walkingSpeed;
    public float timeToFullWalk;
    public float jumpingHeight; // Height of a full jump
    public float walkGravity;
    
    // Physics for when in glide mode
    public float glideGravity;
    public float glideMinSpeed;
    public float glideMaxSpeed;
    public float glideInitSpeed; // Speed sat to this during glide start
    public float glideAccelerationPerDegreeUp; // Per degree the player is pointing up when gliding their speed will accelerate by this (for normal function must be negative)
    public float glideAccelerationPerDegreeDown; // Same as above but down
    public float glideDrag; // Percentage of velocity loss per second (f.x. if 0.2, aka 20%, then after 1 second something moving 25 m/s gonna be moving 25*(1-0.2)=20 m/s, and then 20*(1-0.2)=16 m/s)
    public float glideAdjustmentDegreeRate;
    public float glideBreakRate;
    public float sprintSpeed;
    
    // Things found in world that dont change (references)
    Rigidbody rb; // Rigidbody of player
    GameObject mainCamera; // 1st person camera
    DeathScript deathScript;
    JumpDetectionScript canJumpScript;
    InventoryManager inventoryManager;
    SfxScript sfxScript;
    
    
    // Stuff calculated at start and then never reassigned (constants)
    float jumpingForce; // Velocity needed to jump to jumpingHeight while being under the influence of walkGravity
    float walkingAcceleration;
    
    bool gliding = false; // In walk mode (false) or glide mode (true) ? - decides the movement logic to be done
    bool canBreak = false;
    float glidingSpeed; // When gliding speed is kept, this speed is seperate from the velocity because it isnt affected by glideGravity
    
    /*
     * ===================================================================================
     * START AND UPDATE
     * base entry point functions for Unity, advanced logic seperated into other functions
     * ===================================================================================
     */
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Initializes the stuff
    void Start()
    {
        // Initialize the InputActions
        moveAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        
        // Initialize the references
        rb = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        deathScript = GameObject.FindWithTag("God").GetComponent<DeathScript>();
        canJumpScript = GetComponentInChildren<JumpDetectionScript>();
        inventoryManager = GetComponent<InventoryManager>();
        sfxScript = GetComponent<SfxScript>();
        
        
        // Initialize the constants
        jumpingForce = Mathf.Sqrt(2 * walkGravity * jumpingHeight);
        walkingAcceleration = walkingSpeed / timeToFullWalk;
    }

    // Update is called once per frame
    // Not commented because its mostly self-explanatory
    void Update()
    {
        if (deathScript.dead)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        
        if (jumpAction.WasPerformedThisFrame())
        {
            if (CanJump())
            {
                Jump();
            }
            else if(!gliding)
            {
                BeginGlide();
            }
        }

        if (gliding && !jumpAction.IsPressed())
        {
            canBreak = true;
        }

        if (canBreak && jumpAction.IsPressed())
        {
            GlideBreak();
        }

        if (gliding)
        {
            ProcessGlide();
        }
        else
        {
            ProcessWalking();
        }
    }
    
    /*
     * ================================================================================
     * GLIDING MODE
     * functions for the handling of when in gliding mode (BeginGlide and ProcessGlide)
     * ================================================================================
     */

    void GlideBreak()
    {
        glidingSpeed *= (1 - glideBreakRate * Time.deltaTime);
        print("break: " + glidingSpeed);
    }

    // Switch to gliding mode and set initial gliding speed
    void BeginGlide()
    {
        if (inventoryManager.HasGlider())
        {
            gliding = true;
            if (rb.linearVelocity.magnitude < glideInitSpeed)
            {
                glidingSpeed = glideInitSpeed;
            }
            else
            {
                glidingSpeed = rb.linearVelocity.magnitude;
            }
            sfxScript.PlaySfx(gliderPulloutSound);
        }
    }

    // Handle gliding logic from frame to frame
    void ProcessGlide()
    {
        // Figure out angle of pivot (straight forward is 0, down is 0-90, up is 360-270)
        float degree = mainCamera.transform.localRotation.eulerAngles.x;
        
        // If looking upwards
        if (degree > 270f) 
        {
            // Convert to amount of degree looking up
            float increase = 360 - degree;

            float acceleration = glideAccelerationPerDegreeUp * increase;
            
            // Add (most likely subtract) to/from speed, multiplying with deltaTime to make it frame independent
            glidingSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            // Convert to amount of degree looking down (already in those values its just for consistency)
            float decrease = degree;
            
            float acceleration = glideAccelerationPerDegreeUp * decrease;
            
            // Add to speed, multiplying with deltaTime to make it frame independent
            glidingSpeed += glideAccelerationPerDegreeDown * decrease * Time.deltaTime;
        }

        // Apply drag
        glidingSpeed *= (1 - glideDrag * Time.deltaTime);

        // Clamp between min and max speed
        glidingSpeed = Mathf.Clamp(glidingSpeed, glideMinSpeed, glideMaxSpeed);

        // Gliding vector, aka the way player is looking multiplied by glidingSpeed;
        Vector3 desiredGlideDirection = mainCamera.transform.forward;
        
        Vector3 gravity = new Vector3(0, -glideGravity, 0);

        Vector3 actualGlideDirection = (rb.linearVelocity - gravity).normalized;

        Vector3 newGlideDirection = Vector3.RotateTowards(actualGlideDirection, desiredGlideDirection, glideAdjustmentDegreeRate * Mathf.Deg2Rad * Time.deltaTime, 10000);

        Vector3 newGlide = newGlideDirection * glidingSpeed;
        
        // Gravity vector, pointing down (into y)
        
        
        // Add 2 vectors and make it the velocity of player
        rb.linearVelocity = newGlide + gravity;
        
        
        
        
    }
    
    /*
     * ============================================================================
     * WALKING MODE
     * functions for the handling of when in walking mode (Jump and ProcessWalking)
     * ============================================================================
     */
    
    // Set upwards velocity to be equal to jumpingForce, and keep rest of velocity equal
    void Jump()
    {
        Vector3 vel = rb.linearVelocity;
        rb.linearVelocity = new Vector3(vel.x, jumpingForce, vel.z);
        sfxScript.PlaySfx(jumpSound);
    }

    // Handle walking logic from frame to frame
    void ProcessWalking()
    {
        // Gravity acceleration to apply in a frame
        float gravity = walkGravity * Time.deltaTime;
        
        // 2D vector for movement (W = (0, 1), A = (-1, 0), S = (0, -1), D = (1, 0), WD = (sqrt(2), sqrt(2)), and so on)
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        float speed;

        if (moveInput == Vector2.up && sprintAction.IsPressed())
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }

        // Forward direction of player (NOT camera so this vector is aligned with ground and has no y section)
        Vector3 forwardFake = transform.forward;
        Vector2 forward = new Vector2(forwardFake.x, forwardFake.z);
        
        // Rightwards direction of player (NOT camera so this vector is aligned with ground and has no y section)
        Vector3 rightFake = transform.right;
        Vector2 right = new Vector2(rightFake.x, rightFake.z);
        
        // "Real world" (3D) direction vector for movement, with A and D (moveInput x component) multiplying with the rightwards vector (since left is negative in the moveInput)
        // And W and S (moveInput y component) multiplying with the forwards vector (since S is negative in the moveInput)
        Vector2 direction = moveInput.x * right + moveInput.y * forward;
        
        

        Vector3 vel = rb.linearVelocity;
        
        Vector2 currentWalk =  new Vector2(vel.x, vel.z);

        Vector2 desiredWalk = direction * speed;
        
        // Actual movement vector for walking, direction multiplied by speed
        Vector2 correction = desiredWalk - currentWalk;

        float correctionDistance = walkingAcceleration * Time.deltaTime;
        
        Vector2 newWalk;
        
        if (correctionDistance < correction.magnitude)
        {
             newWalk = currentWalk + correctionDistance * correction.normalized;
        }
        else
        {
            newWalk = desiredWalk;
        }


        if (CanJump())
        {
            gravity = 0;
        }
        
        
        // Replacing X and Z coordinates (those that go along with the ground) equal to the parts from the movement vector, and applying gravity to Y (upwards/downwards) velocity
        rb.linearVelocity = new Vector3(newWalk.x, vel.y - gravity, newWalk.y);
    }
    
    /*
     * ===========================================================================================================
     * STATE AND COLLISION DETECTION
     * functions for setting and reading state regarding the player, aka what surfaces they are touching and such
     *
     * For understanding section it's important to know a specific aspect of how Unity works, specifically the
     * order of function calls: Every frame unity performs a certain amount of physics steps (can be 2, 0,
     * 10, or whatever amount, it tries to run 60 (if I recall the number correctly) physics steps every second
     * no matter what), so if FPS is lower than 60 there will be some frames with more than 1 physics step and
     * likewise if FPS is higher than 60 there will be some frames with 0 physics steps
     *
     * ANYWAY, during every physic step, the "Regular Update Step" or whatever its called isnt run, so we know all
     * physics functions are run together, without any of our other functions mixed in, therefore, to check if a
     * specific thing or function is called during the last physics step we can in the physics step set a value
     * to false or equivilant "it has not been called" value, and then when the function we want to look for is
     * called we set it to true in that function
     *
     * our other functions wont ever have to check if they are between the "setting to false while waiting" and
     * "setting to true once recieved" cuz it all happens in Physics step
     *
     * During the physics step both FixedUpdate and all collision functions are run (OnTriggerStay being one),
     * where FixedUpdate is the first thing to run, therefor we can use it for our "setting thing to
     * intermediate value for physics step"
     * ===========================================================================================================
     */

    void StopGliding()
    {
        gliding = false;
        canBreak = false;
    }

    // Is jumping possible? (Was made at time when figuring it out in the moment more complicated than just reading jumpable)
    bool CanJump()
    {
        return canJumpScript.CanJump();
    }
    
    // Triggers when player touches a collider with IsTrigger sat to true, triggers during physics step
    void OnTriggerEnter(Collider other)
    {

        // Does collider have NoGlide tag? If so switch to walking mode (if already walking mode resetting it does nothing)
        if (other.transform.CompareTag("Obstacle"))
        {
            StopGliding();
            
        }
    }
}
