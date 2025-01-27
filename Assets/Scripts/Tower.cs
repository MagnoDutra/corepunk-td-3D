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
    private bool canRotate = true;

    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected float attackCooldown = 1;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected virtual void Awake() { }

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

        List<Enemy> possibleTargets = new();

        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();

            possibleTargets.Add(newEnemy);
        }

        Enemy mostAdvancedEnemy = GetMostAdvancedEnemy(possibleTargets);

        if (mostAdvancedEnemy != null)
        {
            return mostAdvancedEnemy.transform;
        }

        return null;
    }

    private Enemy GetMostAdvancedEnemy(List<Enemy> targets)
    {
        Enemy mostAdvancedEnemy = null;
        float minRemainingDistance = float.MaxValue;

        foreach (Enemy enemy in targets)
        {
            float distance = enemy.DistanceToFinishLine();
            if (distance < minRemainingDistance)
            {
                minRemainingDistance = distance;
                mostAdvancedEnemy = enemy;
            }
        }

        return mostAdvancedEnemy;
    }

    public void EnableRotation(bool enable)
    {
        canRotate = enable;
    }

    protected virtual void RotateTowardsEnemy()
    {
        if (currentEnemy == null || !canRotate) return;

        // Aqui pegamos a direção
        Vector3 direction = DirectionToEnemyFrom(towerHead);

        // Aqui criamos um quaternion que é a quantidade necessária para ele olhar para aquele inimigo
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Aqui usamos o lerp do quaternion.
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Euler desceve angulos baseado em vector3
        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected Vector3 DirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.position - startPoint.position).normalized;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
