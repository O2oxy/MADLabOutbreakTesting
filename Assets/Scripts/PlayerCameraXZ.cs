using UnityEngine;

public class PlayerCameraXZ : MonoBehaviour
{
    [Header("Rotation Settings")]
    public static float mouseSensitivity = 100f; // Shared sensitivity

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
