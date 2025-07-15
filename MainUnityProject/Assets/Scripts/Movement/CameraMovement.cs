using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CameraMovement : MonoBehaviour
{
    // Mouse movement
    [Header("Mouse Sensitivity")]
    public float mouseSensitivityHorizontalScale; // How much to scale mouse sensitivity in the horizontal direction with
    public float mouseSensitivityVerticalScale; // How much to scale mouse sensitivity in the vertical direction with
    public float mouseSensitivity;
    
    [Header("FOV effects")]
    public float fovSpeedModifier;
    public float speedForFov;
    public float fovAdjustmentRate;
    public float defaultFov;
    public float maxFov;
    
    [Header("Headbobbing")]
    public float headBobbingSpeed;
    public float headBobbingAmplitudeVertical;
    public float headBobbingAmplitudeHorizontal;
    public float headBobbingReturnSpeed;
    
    // Things found in world that dont change (references)
    GameObject mainCamera; // 1st person camera
    Camera mainCameraComponent;
    Rigidbody rb;
    DeathScript deathScript;
    JumpDetectionScript jumpDetectionScript;
    PlayerMovementScript playerMovementScript;

    float headBobbingProgression;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Initialises the references and captures mouse
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCameraComponent = mainCamera.GetComponent<Camera>();
        deathScript = GetComponent<DeathScript>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        jumpDetectionScript = GetComponentInChildren<JumpDetectionScript>();
        playerMovementScript = GetComponent<PlayerMovementScript>();
        
    }

    // Update is called once per frame
    // Rotates camera and player correctly to move view around
    void Update()
    {
        if (deathScript.IsDead())
        {
            mainCameraComponent.fieldOfView = defaultFov;
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

        float target;
        
        if (speedAlignment < speedForFov)
        {
            target = defaultFov;
        }
        else
        {
            target = defaultFov + Mathf.Clamp((speedAlignment - speedForFov) * fovSpeedModifier, 0, maxFov-defaultFov);
        }

        float current = mainCameraComponent.fieldOfView;

        float correction = target - current;

        float correctionAmount = fovAdjustmentRate * Time.deltaTime;

        if (correction < correctionAmount)
        {
            mainCameraComponent.fieldOfView = target;
        }
        else
        {
            mainCameraComponent.fieldOfView += correctionAmount;
        }


    }

    void HeadBobbing()
    {
        if (jumpDetectionScript.CanJump() && rb.linearVelocity.magnitude != 0f)
        {
            headBobbingProgression += rb.linearVelocity.magnitude * headBobbingSpeed * Time.deltaTime;
            float backAndForth = Mathf.Cos(headBobbingProgression) * headBobbingAmplitudeHorizontal;
            float upAndDown = (1 - Mathf.Sin(2 * headBobbingProgression)) * headBobbingAmplitudeVertical;
            mainCamera.transform.localPosition = new Vector3(backAndForth, upAndDown, 0);
        }
        else if(mainCamera.transform.localPosition.x != 0f)
        {
            headBobbingProgression += headBobbingReturnSpeed * Time.deltaTime;
            float backAndForth = Mathf.Cos(headBobbingProgression) * headBobbingAmplitudeHorizontal;
            float upAndDown = (1 - Mathf.Sin(2 * headBobbingProgression)) * headBobbingAmplitudeVertical;
            if (Mathf.Sign(backAndForth) != Mathf.Sign(mainCamera.transform.localPosition.x))
            {
                mainCamera.transform.localPosition = Vector3.zero;
                headBobbingProgression = 0;
            }
            else
            {
                mainCamera.transform.localPosition = new Vector3(backAndForth, upAndDown, 0);
            }
        }
       //tried fixing it because i had some good ideas, either don't know how to execute or doesn't work because of ??? reasons :3 sorry :3
      
    }
}
