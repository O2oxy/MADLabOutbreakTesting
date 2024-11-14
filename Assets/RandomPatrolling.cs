using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrolling : MonoBehaviour
{
    public float patrolRadius = 10f;       // Distance for patrol area
    public float waitTime = 2f;            // Time to wait at each point
    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private bool isWaiting = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewPatrolPoint();
    }

    private void Update()
    {
        // Check if AI reached its destination and not currently waiting
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
                StartCoroutine(WaitAndPatrol());
        }
    }

    private IEnumerator WaitAndPatrol()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        SetNewPatrolPoint();
        isWaiting = false;
    }

    private void SetNewPatrolPoint()
    {
        // Generate a random point within a patrol radius
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        // Find a valid NavMesh point closest to the random point
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            agent.SetDestination(targetPosition);
        }
    }
}
