using UnityEngine;

public class CameraXZ : MonoBehaviour
{
    [Header("Rotation Settings")]
    public static float mouseSensitivity = 100f; // Shared sensitivity
    public Transform playerBody; // Reference to the player object

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Cap Time.deltaTime to avoid large input variations
        float deltaTime = Mathf.Clamp(Time.deltaTime, 0, 0.02f);

        // Mouse input for horizontal rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * deltaTime;

        // Log input and rotation for debugging
        //Debug.Log("Mouse X: " + Input.GetAxis("Mouse X"));
        //Debug.Log("Player Rotation Before: " + playerBody.eulerAngles);

        // Rotate the player horizontally
        playerBody.Rotate(Vector3.up * mouseX);

        //Debug.Log("Player Rotation After: " + playerBody.eulerAngles);
    }
}
