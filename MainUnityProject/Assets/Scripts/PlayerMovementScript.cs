using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;
    public float walkingSpeed;
    public float jumpingHeight;
    public float gravityAcceleration;
    public float slideBufferTime;
    public float rateOfSlide;
    Rigidbody rb;

    public bool jumpable = false;
    public bool wallSlide = false;
    public bool touchingSurface = false;

    float timeSpentHanging;

    float jumpingForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        jumpAction.Enable();
        rb = GetComponent<Rigidbody>();
        jumpingForce = Mathf.Sqrt(2 * gravityAcceleration * jumpingHeight);
    } 

    // Update is called once per frame
    void Update()
    {
        ProcessWalking();
        ProcessGravity();
        if(jumpAction.WasPerformedThisFrame() && jumpable)
        {
            Jump();
        }
    }

    void ProcessSlidingDownWalls()
    {
        if (touchingSurface && wallSlide && !jumpable)
        {
            timeSpentHanging += Time.deltaTime;
        }
        else
        {
            timeSpentHanging = 0;
        }

        if (timeSpentHanging > slideBufferTime)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y - rateOfSlide * Time.deltaTime, pos.z);

        }
    }

    void ProcessGravity()
    {
        Vector3 vel = rb.linearVelocity;
        float gravity = gravityAcceleration * Time.deltaTime;
        rb.linearVelocity = new Vector3(vel.x, vel.y - gravity, vel.z);
    }

    void Jump()
    {
        Vector3 vel = rb.linearVelocity;
        rb.linearVelocity = new Vector3(vel.x, jumpingForce, vel.z);
    }

    void ProcessWalking()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 forward = transform.forward;
        Vector3 side = transform.right;
        Vector3 direction = moveInput.x * side + moveInput.y * forward;
        float coefficient = walkingSpeed;
        Vector3 walk = direction * coefficient;
        Vector3 vel = rb.linearVelocity;
        rb.linearVelocity = new Vector3(walk.x, vel.y, walk.z);
    }
    
    void OnTriggerEnter(Collider other)
    {
        print(other.transform.tag);
        if (other.transform.CompareTag("Jumpable"))
        {
            jumpable = true;
        }else if (other.transform.CompareTag("WallSlide"))
        {
            wallSlide = true;
        }else if (other.transform.CompareTag("Fill"))
        {
            touchingSurface = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Jumpable"))
        {
            jumpable = false;
        }else if (other.transform.CompareTag("WallSlide"))
        {
            wallSlide = false;
        }else if (other.transform.CompareTag("Fill"))
        {
            touchingSurface = false;
        }
    }
}
