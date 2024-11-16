using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("Health Bar Settings")]
    public Slider healthSlider;

    public void SetMaxHealth(float maxHealth)
    {
        if (healthSlider == null)
        {
            Debug.LogError("Health Slider is not assigned!");
            return;
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; // Initialize to full health
    }

    public void UpdateHealth(float currentHealth)
    {
        if (healthSlider == null)
        {
            Debug.LogError("Health Slider is not assigned!");
            return;
        }
        healthSlider.value = currentHealth;
    }
}
