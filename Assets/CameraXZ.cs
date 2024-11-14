using UnityEngine;

public class CameraXZ : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float mouseSensitivity = 100f;

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse input for horizontal rotation (left and right)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera around the player on the y-axis
        transform.Rotate(Vector3.up * mouseX);
    }
}
