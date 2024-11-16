using UnityEngine;

public class CameraY : MonoBehaviour
{
    // Variables
    public float mouseSensitivity = 100f;
    public float maxVerticalAngle = 45f;
    private float verticalRotation = 0f;

    void Update()
    {
        // Mouse input for vertical rotation (up and down)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Update vertical rotation and clamp it within the limits
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // Apply the vertical rotation to the camera only
        transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }
}
