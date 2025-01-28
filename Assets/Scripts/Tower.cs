using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    public Enemy currentEnemy;

    protected float lastTimeAttacked;

    [Header("Tower Setup")]
    [SerializeField] protected EnemyType enemyPriotiryType = EnemyType.None;
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10;
    private bool canRotate = true;

    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected float attackCooldown = 1;
    [SerializeField] protected LayerMask whatIsEnemy;

    [Space]
    [Tooltip("Enabling this allow tower to change target beetwen attacks")]
    [SerializeField] private bool dynamicTargetChange;
    private float targetCheckInterval = 0.1f;
    private float lastTimeCheckedTarget;

    protected virtual void Awake()
    {
        EnableRotation(true);
    }

    protected virtual void Update()
    {
        UpdateTargetIfNeeded();

        if (currentEnemy == null)
        {
            currentEnemy = FindEnemyWithinRange();
            return;
        }

        if (CanAttack())
        {
            Attack();
        }

        if (Vector3.Distance(currentEnemy.CenterPoint(), transform.position) > attackRange)
        {
            currentEnemy = null;
        }

        RotateTowardsEnemy();

    }

    private void UpdateTargetIfNeeded()
    {
        if (!dynamicTargetChange) return;

        if (Time.time > lastTimeAttacked + targetCheckInterval)
        {
            lastTimeCheckedTarget = Time.time;
            currentEnemy = FindEnemyWithinRange();
        }
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

    protected Enemy FindEnemyWithinRange()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        if (enemiesAround.Length <= 0) return null;

        List<Enemy> priorityTargets = new();
        List<Enemy> possibleTargets = new();

        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            EnemyType newEnemyType = newEnemy.GetEnemyType();

            if (newEnemyType == enemyPriotiryType)
                priorityTargets.Add(newEnemy);
            else
                possibleTargets.Add(newEnemy);
        }


        if (priorityTargets.Count > 0)
            return GetMostAdvancedEnemy(priorityTargets);

        if (possibleTargets.Count > 0)
            return GetMostAdvancedEnemy(possibleTargets);

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
        return (currentEnemy.CenterPoint() - startPoint.position).normalized;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
