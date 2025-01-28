using UnityEngine;

public class TowerCrossbow : Tower
{
    private CrossbowVisuals visuals;

    [Header("Crossbow Details")]
    [SerializeField] private int damage;
    [SerializeField] private Transform gunPoint;

    protected override void Awake()
    {
        base.Awake();

        visuals = GetComponent<CrossbowVisuals>();
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;

            Enemy enemyTarget = null;
            if (hitInfo.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                enemyTarget = currentEnemy;
            }

            visuals.PlayAttackFX(gunPoint.position, hitInfo.point, enemyTarget);
            visuals.PlayReloadFX(attackCooldown);
        }
    }
}
