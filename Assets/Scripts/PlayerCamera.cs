using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Rotation Settings")]
    public static float mouseSensitivity = 100f; // Shared sensitivity
    Camera mainCamera;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), mouseSensitivity * Time.fixedDeltaTime);
    }
}
