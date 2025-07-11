using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{
    public InputAction moveAction;
    public float speed = 4;

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 forward = transform.forward;
        Vector3 side = transform.right;
        Vector3 direction = moveInput.x * side + moveInput.y * forward;
        float coefficient = speed;
        rb.linearVelocity = direction * coefficient;
        
    }
}
