using UnityEngine;

public class MeleeWeaponPlayer : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    public float meleeRange = 2f;                  // Range of the melee attack
    public float meleeDamage = 30f;                // Damage dealt by the melee weapon
    public float attackCooldown = 1.5f;            // Time between attacks
    public float attackAngle = 45f;                // Arc angle in degrees
    public LayerMask targetLayers;                 // Layer mask to specify valid targets

    private float lastAttackTime;

    void Update()
    {
        // Check for input and cooldown time before attacking
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Melee Attack");
            AttemptAttack();
            lastAttackTime = Time.time;
        }
    }

    private void AttemptAttack()
    {
        // Find all colliders within melee range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange, targetLayers);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the target is within the specified angle in front of the player
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= attackAngle / 2)
            {
                // Apply damage if the entity has an EntityHealth component
                EntityHealth targetHealth = hitCollider.GetComponent<EntityHealth>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(meleeDamage);
                    Debug.Log("Player hit " + hitCollider.name + " for " + meleeDamage + " damage.");
                }
            }
        }

        // Optional: Add attack animations or sound effects here
    }

    // Optional: Draw attack range and arc in the editor for visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        // Draw the attack arc
        Vector3 forward = transform.forward * meleeRange;
        Quaternion leftRotation = Quaternion.Euler(0, -attackAngle / 2, 0);
        Quaternion rightRotation = Quaternion.Euler(0, attackAngle / 2, 0);

        Vector3 leftDirection = leftRotation * forward;
        Vector3 rightDirection = rightRotation * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection);
    }
}
