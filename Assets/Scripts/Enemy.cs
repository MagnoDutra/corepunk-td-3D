using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    None, Fast, Basic
}

public class Enemy : MonoBehaviour, IDamageable
{
    public int healthPoints = 4;

    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;


    [Header("Movement")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float turnSpeed = 10;

    [Space]
    private float totalDistance;

    private NavMeshAgent agent;
    private int waypointIndex = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    public void SetupEnemy(List<Waypoint> newWaypoints)
    {
        waypoints = new();
        foreach (var waypoint in newWaypoints)
        {
            waypoints.Add(waypoint.transform);
        }

        CollectTotalDistance();
    }

    private void CollectTotalDistance()
    {
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            float distance = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalDistance += distance;
        }
    }

    private void Update()
    {
        FaceTarget(agent.steeringTarget);
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetNextWaypoint());
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {
        Vector3 directionToTarget = newTarget - transform.position;
        directionToTarget.y = 0; // Ignorar rotacao no y

        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 GetNextWaypoint()
    {
        if (waypointIndex >= waypoints.Count) return transform.position;

        Vector3 targetPoint = waypoints[waypointIndex].position;

        if (waypointIndex > 0)
        {
            float distance = Vector3.Distance(waypoints[waypointIndex].position, waypoints[waypointIndex - 1].position);
            totalDistance = totalDistance - distance;
        }

        waypointIndex++;

        return targetPoint;
    }

    public void TakeDamage(int amount)
    {
        healthPoints -= amount;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;

    public Vector3 CenterPoint() => centerPoint.position;

    public EnemyType GetEnemyType() => enemyType;
}
