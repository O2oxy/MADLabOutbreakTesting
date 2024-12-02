using UnityEngine;
using UnityEngine.AI;

public class DetectAlertChase : MonoBehaviour
{
    [Header("Detection and Chase Settings")]
    public float detectionRadius = 10f; // Radius to detect the player
    public float alertRadius = 15f; // Radius to alert nearby allies
    public float chaseSpeed = 5f; // Speed of the AI when chasing the player
    public float patrolSpeed = 2f; // Speed of the AI when patrolling
    public LayerMask playerLayer; // Layer for detecting the player
    public LayerMask allyLayer; // Layer for detecting allies

    private NavMeshAgent agent;
    private Transform playerTransform;
    private bool isChasing = false;
    private bool playerDetected = false;
    private MeleeAttack meleeAttack; // Reference to MeleeAttack script
    private Animator animator; // Reference to Animator

    // Animation state names
    private const string IdleAnim = "Idle";
    private const string WalkAnim = "Walk forward";
    private const string ChaseAnim = "Chase forward";
    private const string AttackAnim = "Attack";

    private string currentAnimation = ""; // Tracks the current animation

    public LayerMask GetPlayerLayer() => playerLayer; // Provide playerLayer for MeleeAttack

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
        agent.speed = patrolSpeed; // Start with patrol speed
        meleeAttack = GetComponent<MeleeAttack>(); // Get the MeleeAttack component

        // Set initial animation speeds
        SetAnimationSpeed(patrolSpeed);
        ChangeAnimations(IdleAnim); // Start in idle state
    }

    private void Update()
    {
        DetectPlayer();

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            PatrolOrIdle();
        }
    }

    private void PatrolOrIdle()
    {
        if (agent.velocity.sqrMagnitude > 0.1f) // Zombie is moving
        {
            SetAnimationSpeed(patrolSpeed); // Adjust animation speed for patrol
            ChangeAnimations(WalkAnim);
        }
        else // Zombie is idle
        {
            ChangeAnimations(IdleAnim);
        }
    }

    private void DetectPlayer()
    {
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (playerColliders.Length > 0)
        {
            playerTransform = playerColliders[0].transform;
            playerDetected = true;
            isChasing = true;
            agent.speed = chaseSpeed; // Switch to chase speed
            meleeAttack.SetTarget(playerTransform);

            SetAnimationSpeed(chaseSpeed); // Adjust animation speed for chase
            ChangeAnimations(ChaseAnim); // Play chase animation
        }
        else if (playerDetected)
        {
            playerDetected = false;
            isChasing = false;
            agent.speed = patrolSpeed; // Return to patrol speed
            agent.SetDestination(transform.position); // Stop moving
            meleeAttack.SetTarget(null); // Clear target

            SetAnimationSpeed(patrolSpeed); // Adjust animation speed for patrol
            ChangeAnimations(WalkAnim); // Return to walk
        }
    }

    private void ChasePlayer()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
            if (Vector3.Distance(transform.position, playerTransform.position) <= meleeAttack.meleeRange)
            {
                ChangeAnimations(AttackAnim); // Attack animation
                meleeAttack.AttemptAttack(); // Perform attack
            }
            else
            {
                ChangeAnimations(ChaseAnim); // Continue chasing
            }
        }
    }

    /// <summary>
    /// Checks if the current animation is already playing.
    /// </summary>
    /// <param name="animationName">The name of the animation to check.</param>
    /// <returns>True if the animation is already playing, otherwise false.</returns>
    private bool CheckAnimations(string animationName)
    {
        return currentAnimation == animationName;
    }

    /// <summary>
    /// Changes the animation if it's not already playing.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    private void ChangeAnimations(string animationName)
    {
        if (animator != null && !CheckAnimations(animationName))
        {
            animator.CrossFade(animationName, 0.2f); // Smooth transition
            currentAnimation = animationName; // Update the current animation
        }
    }

    /// <summary>
    /// Sets the animation speed in the Animator based on movement speed.
    /// </summary>
    /// <param name="speed">The speed to apply to animations.</param>
    private void SetAnimationSpeed(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("AnimationSpeed", speed / 2f); // Normalize speed (adjust factor if needed)
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
