using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;
    public float walkingSpeed;
    public float jumpingHeight;
    public float gravityAcceleration;
    
    Rigidbody rb;

    float jumpingForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        jumpAction.Enable();
        rb = GetComponent<Rigidbody>();
        jumpingForce = 2 * gravityAcceleration * jumpingHeight;
    } 

    // Update is called once per frame
    void Update()
    {
        ProcessWalking();
        ProcessGravity();
        if(jumpAction.WasPerformedThisFrame())
        {
            print("jump!");
            Jump();
        }
    }

    void ProcessGravity()
    {
        rb.AddForce(0, -gravityAcceleration, 0, ForceMode.Acceleration);
    }

    void Jump()
    {
        rb.AddForce(0, jumpingForce, 0, ForceMode.VelocityChange);
    }

    void ProcessWalking()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 forward = transform.forward;
        Vector3 side = transform.right;
        Vector3 direction = moveInput.x * side + moveInput.y * forward;
        float coefficient = walkingSpeed;
        rb.linearVelocity = direction * coefficient;
    }
}
