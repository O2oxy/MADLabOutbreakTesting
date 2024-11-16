using UnityEngine;

public class CameraAimController : MonoBehaviour
{
    public GameObject mainCameraRight;
    public GameObject aimCameraRight;
    public GameObject mainCameraLeft;
    public GameObject aimCameraLeft;
    public Transform gunTransform;
    public Vector3 gunOffsetRight = new Vector3(0.5f, 0, 0); // Right offset
    public Vector3 gunOffsetLeft = new Vector3(-0.5f, 0, 0);  // Left offset

    private bool isToggled = false;

    void Start()
    {
        SetGunPosition(); // Initialize gun position
    }

    void Update()
    {
        // Toggle camera states
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isToggled = !isToggled;
            Debug.Log("Toggled state: " + isToggled);
            SetGunPosition(); // Update gun position on toggle
        }

        // Handle camera switching and aiming
        if (isToggled)
        {
            mainCameraLeft.SetActive(true);
            mainCameraRight.SetActive(false);
            aimCameraRight.SetActive(false);

            if (Input.GetMouseButton(1))
            {
                mainCameraLeft.SetActive(false);
                aimCameraLeft.SetActive(true);
                gunTransform.localPosition = gunOffsetLeft; // Left aim position
            }
            else
            {
                mainCameraLeft.SetActive(true);
                aimCameraLeft.SetActive(false);
                gunTransform.localPosition = gunOffsetLeft; // Left default position
            }

            if (Input.GetKeyDown(KeyCode.Q) && Input.GetMouseButton(1))
            {
                aimCameraLeft.SetActive(false);
                aimCameraRight.SetActive(true);
                gunTransform.localPosition = gunOffsetRight; // Right aim position
            }
        }
        else
        {
            mainCameraLeft.SetActive(false);
            aimCameraLeft.SetActive(false);
            mainCameraRight.SetActive(true);

            if (Input.GetMouseButton(1))
            {
                mainCameraRight.SetActive(false);
                aimCameraRight.SetActive(true);
                gunTransform.localPosition = gunOffsetRight; // Right aim position
            }
            else
            {
                mainCameraRight.SetActive(true);
                aimCameraRight.SetActive(false);
                gunTransform.localPosition = gunOffsetRight; // Right default position
            }

            if (Input.GetKeyDown(KeyCode.Q) && Input.GetMouseButton(1))
            {
                aimCameraLeft.SetActive(true);
                aimCameraRight.SetActive(false);
                gunTransform.localPosition = gunOffsetLeft; // Left aim position
            }
        }
    }

    void SetGunPosition()
    {
        // Initial position setting based on toggle state
        gunTransform.localPosition = isToggled ? gunOffsetLeft : gunOffsetRight;
    }
}
