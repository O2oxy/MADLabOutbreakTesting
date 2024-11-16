using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    [Header("References")]
    public Gun gunScript;       // Reference to the Gun script

    private TMP_Text ammoText;  // TextMeshPro component attached to this GameObject

    void Start()
    {
        // Get the TextMeshPro component on the same GameObject
        ammoText = GetComponent<TMP_Text>();

        if (ammoText == null)
        {
            Debug.LogError("AmmoDisplay: No TextMeshPro component found on this GameObject!");
        }

        // Validate the Gun script reference
        if (gunScript == null)
        {
            Debug.LogError("AmmoDisplay: No Gun script assigned!");
        }

        UpdateAmmoDisplay();
    }

    void Update()
    {
        // Continuously update the ammo display
        UpdateAmmoDisplay();
    }

    /// <summary>
    /// Updates the UI text to reflect the current ammo count and magazine size from the Gun script.
    /// </summary>
    private void UpdateAmmoDisplay()
    {
        if (ammoText != null && gunScript != null)
        {
            ammoText.text = $"{gunScript.ammoCount} / {gunScript.magazineSize}";
        }
    }
}
