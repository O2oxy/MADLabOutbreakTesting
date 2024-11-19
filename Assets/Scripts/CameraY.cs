using UnityEngine;

public class CameraY : MonoBehaviour
{
    public float maxVerticalAngle = 45f;
    private float verticalRotation = 0f;

    void Update()
    {
        // Cap Time.deltaTime to avoid large input variations
        float deltaTime = Mathf.Clamp(Time.deltaTime, 0, 0.02f);

        // Mouse input for vertical rotation
        float mouseY = Input.GetAxis("Mouse Y") * CameraXZ.mouseSensitivity * deltaTime;

        // Log input and rotation for debugging
        //Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y"));
        //Debug.Log("Vertical Rotation Before: " + verticalRotation);

        // Update vertical rotation and clamp it within the limits
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // Apply the vertical rotation using quaternions
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        //Debug.Log("Vertical Rotation After: " + verticalRotation);
    }
}
