using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    GameObject mainCamera;
    public float mouseSensitivityHorizontalScale;
    public float mouseSensitivityVerticalScale;
    public float mouseSensitivity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
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
}
