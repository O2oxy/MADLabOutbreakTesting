using UnityEngine;
using System;

public class EntityHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Score Settings")]
    public int pointsOnDeath = 10; // Points awarded for killing this entity

    private ScoreManager scoreManager;

    public GameObject rootObject; // Optional: Specify the root object to destroy
    public event Action<GameObject> OnDeath; // Event for death callback

    void Start()
    {
        currentHealth = maxHealth;

        // Assign the root object if not set
        if (rootObject == null)
        {
            rootObject = transform.root.gameObject;
        }

        // Locate the ScoreManager in the scene
        scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in the scene!");
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

        // Award points if ScoreManager exists
        if (scoreManager != null)
        {
            scoreManager.AddScore(pointsOnDeath);
        }

        OnDeath?.Invoke(rootObject); // Trigger the death event
        // Destroy the root object
        Destroy(rootObject);
    }
}
