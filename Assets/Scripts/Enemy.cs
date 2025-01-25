using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] waypoint;

    private NavMeshAgent agent;
    private int waypointIndex = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetNextWaypoint());
        }
    }

    private Vector3 GetNextWaypoint()
    {
        if (waypointIndex >= waypoint.Length) return transform.position;

        Vector3 targetPoint = waypoint[waypointIndex].position;
        waypointIndex++;

        return targetPoint;
    }
}
