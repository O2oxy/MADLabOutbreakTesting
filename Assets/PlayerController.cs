using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = 9.8f;
    public float groundLevel = 0f;

    private Vector3 velocity;

    void Update()
    {
        // Capture AWSD input for movement
        float moveX = Input.GetAxis("Horizontal"); // "A" and "D"
        float moveZ = Input.GetAxis("Vertical");   // "W" and "S"

        // Calculate movement relative to the player's orientation
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Apply gravity
        if (transform.position.y > groundLevel)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            // Keep player grounded
            velocity.y = 0;
            transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
        }

        // Apply gravity-affected movement
        transform.position += velocity * Time.deltaTime;
    }
}
