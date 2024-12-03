using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    public float meleeRange = 2f; // Range of the melee attack
    public float meleeDamage = 30f; // Damage dealt by the melee weapon
    public float attackCooldown = 1.5f; // Time between attacks
    public float attackAngle = 45f; // Arc angle in degrees
    public Vector3 attackOffset = Vector3.zero; // Offset for attack position
    public float rotationSpeed = 5f; // Speed of rotation to face the player

    private LayerMask targetLayer; // Target layer for valid attacks
    private Transform target;
    private float lastAttackTime;
    private Animator animator;
    private DetectAlertChase detectAlertChase;
    private NavMeshAgent agent; // Reference to the NavMeshAgent
    private bool isAttacking = false; // Tracks if the NPC is currently attacking

    private void Start()
    {
        animator = GetComponent<Animator>();
        detectAlertChase = GetComponent<DetectAlertChase>();
        agent = GetComponent<NavMeshAgent>();

        if (detectAlertChase != null)
        {
            targetLayer = detectAlertChase.GetPlayerLayer(); // Use playerLayer from DetectAlertChase
        }

        if (animator != null)
        {
            // Match the animation speed with the attack cooldown
            animator.SetFloat("AttackSpeed", 1f / attackCooldown);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void AttemptAttack()
    {
        if (isAttacking || Time.time < lastAttackTime + attackCooldown)
            return;

        isAttacking = true;

        // Stop movement and rotate to face the player
        if (agent != null)
        {
            agent.isStopped = true;
        }

        if (target != null)
        {
            RotateTowardsTarget();
        }

        PerformAttack();

        // Play melee attack animation
        if (animator != null)
        {
            animator.CrossFade("Attack", 0.1f);
        }

        // Resume movement after the attack animation finishes
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void PerformAttack()
    {
        // Adjust position for attack detection
        Vector3 attackPosition = transform.position + attackOffset;

        // Find all colliders within melee range on the specified target layers
        Collider[] hitColliders = Physics.OverlapSphere(attackPosition, meleeRange, targetLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the target is within the specified angle in front of the NPC
            Vector3 directionToTarget = (hitCollider.transform.position - attackPosition).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= attackAngle / 2)
            {
                // Apply damage if the entity has an EntityHealth component
                EntityHealth targetHealth = hitCollider.GetComponent<EntityHealth>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(meleeDamage);
                    Debug.Log($"{gameObject.name} hit {hitCollider.name} for {meleeDamage} damage.");
                }
            }
        }

        lastAttackTime = Time.time;
    }

    private void ResetAttack()
    {
        isAttacking = false;

        // Resume movement
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    private void RotateTowardsTarget()
    {
        if (target == null)
            return;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void Update()
    {
        if (isAttacking)
        {
            // Prevent other animations from playing during the attack
            if (animator != null)
            {
                animator.CrossFade("Attack", 0.1f);
            }

            // Rotate to face the player during the attack
            RotateTowardsTarget();
        }
    }

    // Draw attack range and offset in the editor for visualization
    private void OnDrawGizmosSelected()
    {
        Vector3 attackPosition = transform.position + attackOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition, meleeRange);

        // Draw the attack arc
        Vector3 forward = transform.forward * meleeRange;
        Quaternion leftRotation = Quaternion.Euler(0, -attackAngle / 2, 0);
        Quaternion rightRotation = Quaternion.Euler(0, attackAngle / 2, 0);

        Vector3 leftDirection = leftRotation * forward;
        Vector3 rightDirection = rightRotation * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(attackPosition, attackPosition + leftDirection);
        Gizmos.DrawLine(attackPosition, attackPosition + rightDirection);
    }
}
