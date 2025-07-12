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
    public float glideGravity;
    public float glideDampPerDegreeUp;
    public float glideIncreasePerDegreeUp;
    
    Rigidbody rb;
    float jumpingForce;
    GameObject mainCamera;
    
    bool jumpable = true;
    bool gliding = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        jumpAction.Enable();
        rb = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        jumpingForce = Mathf.Sqrt(2 * gravityAcceleration * jumpingHeight);
    }

    // Update is called once per frame
    void Update()
    {

        if (jumpAction.WasPerformedThisFrame())
        {
            if (CanJump())
            {
                Jump();
            }
            else
            {
                gliding = true;
            }
        }

        if (gliding)
        {
            ProcessGlide();
        }
        else
        {
            ProcessWalking();
            ProcessGravity();
        }
    }

    void ProcessGlide()
    {
        float degree = mainCamera.transform.localRotation.eulerAngles.x;
        Vector3 vel = rb.linearVelocity;
        if (degree > 270f)
        {
            float increase = 360 - degree;
            vel += mainCamera.transform.forward * rb.linearVelocity.magnitude * (increase * glideDampPerDegreeUp * Time.deltaTime);
            print("increase: " + increase);
        }
        else
        {
            float decrease = degree;
            vel = mainCamera.transform.forward * rb.linearVelocity.magnitude;
            vel += vel.normalized * (decrease * glideIncreasePerDegreeUp * Time.deltaTime);
            print("decrease: " + decrease);
        }

        vel.y -= glideGravity * Time.deltaTime;

        rb.linearVelocity = vel;
    }

    void ProcessGravity()
    {
        Vector3 vel = rb.linearVelocity;
        float gravity = gravityAcceleration * Time.deltaTime;
        rb.linearVelocity = new Vector3(vel.x, vel.y - gravity, vel.z);
    }

    bool CanJump()
    {
        return jumpable;
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
    
    void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Jumpable"))
        {
            jumpable = true;
        }

        if (other.transform.CompareTag("NoGlide"))
        {
            gliding = false;
        }
    }

    void FixedUpdate()
    {
        jumpable = false;
    }
}
