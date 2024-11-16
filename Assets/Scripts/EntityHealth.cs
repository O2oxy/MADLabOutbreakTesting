using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject rootObject; // Root object to destroy
    public PlayerHealthBar healthBar; // Reference to the PlayerHealthBar script

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize health bar if assigned
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        // Default root object to the top-level parent if not assigned
        if (rootObject == null)
        {
            rootObject = transform.root.gameObject;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Remaining Health: " + currentHealth);

        // Update health bar
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(rootObject.name + " has died.");
        Destroy(rootObject);
    }
}
