using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float turnSpeed = 15f; // Rotation speed
    Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
        mainCamera = Camera.main; // Gets the main camera
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse input for camera rotation (horizontal movement)
        float mouseX = Input.GetAxis("Mouse X");

        // Apply rotation based on the input
        float yaw = transform.eulerAngles.y + mouseX * turnSpeed;

        // Smoothly rotate the camera using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yaw, 0), turnSpeed * Time.deltaTime);
    }
}
