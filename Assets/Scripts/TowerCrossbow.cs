using UnityEngine;

public class TowerCrossbow : Tower
{
    private CrossbowVisuals visuals;

    [Header("Crossbow Details")]
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

            Debug.Log(hitInfo);
            Debug.DrawLine(gunPoint.position, hitInfo.point);

            visuals.PlayAttackFX(gunPoint.position, hitInfo.point);
            visuals.PlayReloadFX(attackCooldown);
        }
    }
}
