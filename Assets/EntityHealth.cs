using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject rootObject; // Optional: Specify the root object to destroy

    void Start()
    {
        currentHealth = maxHealth;

        // If no specific root object is assigned, default to the top-level parent
        if (rootObject == null)
        {
            rootObject = transform.root.gameObject;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Remaining Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(rootObject.name + " has died.");

        // Destroy the specified root object, which could be the top-level parent or a custom object
        Destroy(rootObject);

        // Optional: Add additional death effects or cleanup here
    }
}
