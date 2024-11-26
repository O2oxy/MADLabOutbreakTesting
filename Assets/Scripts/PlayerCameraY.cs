using UnityEditor.Rendering;
using UnityEngine;

public class PlayerCameraY : MonoBehaviour
{
    public float maxVerticalAngle = 45f;
    private float verticalRotation = 0f;

    void Update()
    {
        // Use mouse sensitivity from CameraXZ
        float mouseY = Input.GetAxis("Mouse Y") * PlayerCameraXZ.mouseSensitivity * Time.deltaTime;

        // Update vertical rotation and clamp it within the limits
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // Apply the vertical rotation to the camera only
        transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }
}
