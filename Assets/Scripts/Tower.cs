using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    protected float lastTimeAttacked;

    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10;

    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected float attackCooldown = 1;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected virtual void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemyWithinRange();
            return;
        }

        if (CanAttack())
        {
            Attack();
        }

        if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange)
        {
            currentEnemy = null;
        }

        RotateTowardsEnemy();

    }

    protected virtual void Attack()
    {
        Debug.Log("attack perfoermed at " + Time.time);
    }

    protected bool CanAttack()
    {
        if (Time.time > lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    protected Transform FindRandomEnemyWithinRange()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        if (enemiesAround.Length <= 0) return null;

        List<Transform> possibleTargets = new();

        foreach (Collider enemy in enemiesAround)
        {
            possibleTargets.Add(enemy.transform);
        }

        int randomIndex = Random.Range(0, possibleTargets.Count);

        return possibleTargets[randomIndex];
    }

    protected virtual void RotateTowardsEnemy()
    {
        if (currentEnemy == null) return;

        // Aqui pegamos a direção
        Vector3 direction = currentEnemy.position - towerHead.position;

        // Aqui criamos um quaternion que é a quantidade necessária para ele olhar para aquele inimigo
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Aqui usamos o lerp do quaternion.
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Euler desceve angulos baseado em vector3
        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
