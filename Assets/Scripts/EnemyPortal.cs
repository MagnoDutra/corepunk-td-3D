using UnityEngine;
using System.Collections.Generic;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private List<Waypoint> waypointList;
    [SerializeField] private float spawnCooldown;
    private float spawnTimer;

    public List<GameObject> enemiesToCreate;

    private void Awake()
    {
        CollectWaypoints();
    }

    private void Update()
    {
        if (CanMakeNewEnemy())
        {
            CreateEnemy();
        }
    }

    private bool CanMakeNewEnemy()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            spawnTimer = spawnCooldown;
            return true;
        }

        return false;
    }

    public List<GameObject> GetEnemyList() => enemiesToCreate;

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, transform.position, Quaternion.identity);

        newEnemy.GetComponent<Enemy>().SetupEnemy(waypointList);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject choosenEnemy = enemiesToCreate[randomIndex];

        enemiesToCreate.Remove(choosenEnemy);

        return choosenEnemy;
    }

    [ContextMenu("Collect Waypoints")]
    private void CollectWaypoints()
    {
        waypointList = new();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Waypoint waypoint))
            {
                waypointList.Add(waypoint);
            }
        }
    }
}
