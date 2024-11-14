using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectAlertChase : MonoBehaviour
{
    [Header("Detection and Chase Settings")]
    public float detectionRadius = 10f;       // Radius to detect the player
    public float alertRadius = 15f;           // Radius to alert nearby allies
    public float chaseSpeed = 5f;             // Speed of the AI when chasing the player
    public float patrolSpeed = 2f;            // Speed of the AI when patrolling
    public LayerMask playerLayer;             // Layer for detecting the player
    public LayerMask allyLayer;               // Layer for detecting allies

    private NavMeshAgent agent;
    private Transform playerTransform;
    private bool isChasing = false;
    private bool playerDetected = false;
    private MeleeAttack meleeAttack;          // Reference to MeleeAttack script

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed; // Start with patrol speed
        meleeAttack = GetComponent<MeleeAttack>(); // Get the MeleeAttack component
    }

    private void Update()
    {
        DetectPlayer();

        if (isChasing)
        {
            ChasePlayer();
            meleeAttack.AttemptAttack(); // Attempt melee attack if within range
        }
    }

    private void DetectPlayer()
    {
        // Check if player is within detection radius
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (playerColliders.Length > 0)
        {
            playerTransform = playerColliders[0].transform;
            playerDetected = true;
            isChasing = true;
            agent.speed = chaseSpeed;    // Switch to chase speed
            meleeAttack.SetTarget(playerTransform); // Set target for melee attack
            AlertAllies();               // Alert nearby allies
        }
        else if (playerDetected)
        {
            // Reset if player leaves detection radius
            playerDetected = false;
            isChasing = false;
            agent.speed = patrolSpeed; // Return to patrol speed
            agent.SetDestination(transform.position); // Stop moving
            meleeAttack.SetTarget(null); // Clear target for melee attack
        }
    }

    private void AlertAllies()
    {
        // Find all allies within the alert radius and alert them to start chasing the player
        Collider[] allyColliders = Physics.OverlapSphere(transform.position, alertRadius, allyLayer);
        foreach (Collider allyCollider in allyColliders)
        {
            DetectAlertChase allyAI = allyCollider.GetComponent<DetectAlertChase>();
            if (allyAI != null && !allyAI.isChasing) // Only alert if not already chasing
            {
                allyAI.StartChase(playerTransform);
            }
        }
    }

    public void StartChase(Transform target)
    {
        playerTransform = target;
        isChasing = true;
        agent.speed = chaseSpeed;        // Switch to chase speed
        meleeAttack.SetTarget(target);   // Set target for melee attack
    }

    private void ChasePlayer()
    {
        // If chasing, set the player's position as the destination
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection and alert radius for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
