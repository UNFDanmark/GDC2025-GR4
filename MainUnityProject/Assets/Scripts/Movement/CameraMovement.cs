using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    // Mouse movement
    public float mouseSensitivityHorizontalScale; // How much to scale mouse sensitivity in the horizontal direction with
    public float mouseSensitivityVerticalScale; // How much to scale mouse sensitivity in the vertical direction with
    public float mouseSensitivity;
    public float fovSpeedModifier;
    public float defaultFov;
    public float maxFov;
    public float headBobbingSpeed;
    public float headBobbingAmplitudeVertical;
    public float headBobbingAmplitudeHorizontal;
    // Things found in world that dont change (references)
    GameObject mainCamera; // 1st person camera
    Camera mainCameraComponent;
    Rigidbody rb;
    DeathScript deathScript;
    JumpDetectionScript jumpDetectionScript;

    float headBobbingProgression;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Initialises the references and captures mouse
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCameraComponent = mainCamera.GetComponent<Camera>();
        deathScript = GameObject.FindWithTag("God").GetComponent<DeathScript>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        jumpDetectionScript = GetComponentInChildren<JumpDetectionScript>();
    }

    // Update is called once per frame
    // Rotates camera and player correctly to move view around
    void Update()
    {
        if (deathScript.dead)
        {
            return;
        }
        
        ProcessLookingAround();
        FovSpeedChanger();
        HeadBobbing();
    }

    void ProcessLookingAround()
    {
        float mouseSensitivityVertical = mouseSensitivity * mouseSensitivityVerticalScale;
        float mouseSensitivityHorizontal = mouseSensitivity * mouseSensitivityHorizontalScale;
        
        Vector2 mouseMovement = Mouse.current.delta.value;
        float horizontalRot = mouseMovement.x * mouseSensitivityHorizontal;
        float verticalRot = - mouseMovement.y * mouseSensitivityVertical;
        transform.Rotate(0, horizontalRot, 0);
        
        float xRot = mainCamera.transform.localRotation.eulerAngles.x+verticalRot;

        if (xRot > 270f)
        {
            xRot -= 360;
        }

        Mathf.Clamp(xRot, -90, 90);

        mainCamera.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }
    void FovSpeedChanger ()
    {
        float speedAlignment = Vector3.Dot(rb.linearVelocity, mainCamera.transform.forward);
        mainCameraComponent.fieldOfView = defaultFov + Mathf.Clamp(speedAlignment * fovSpeedModifier, 0, maxFov-defaultFov);
    }

    void HeadBobbing()
    {
        if (jumpDetectionScript.CanJump())
        {
            print("bobbing");
            headBobbingProgression += rb.linearVelocity.magnitude * headBobbingSpeed * Time.deltaTime;
            float backAndForth = Mathf.Cos(headBobbingProgression) * headBobbingAmplitudeHorizontal;
            float upAndDown = (1 - Mathf.Sin(2 * headBobbingProgression)) * headBobbingAmplitudeVertical;
            mainCamera.transform.localPosition = new Vector3(backAndForth, upAndDown, 0);
        }
    }
}
