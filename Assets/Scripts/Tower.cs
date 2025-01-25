using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [Header("Tower Setup")]
    [SerializeField] private Transform towerHead;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        RotateTowardsEnemy();
    }

    private void RotateTowardsEnemy()
    {
        // Aqui pegamos a direção
        Vector3 direction = currentEnemy.position - towerHead.position;

        // Aqui criamos um quaternion que é a quantidade necessária para ele olhar para aquele inimigo
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Aqui usamos o lerp do quaternion.
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Euler desceve angulos baseado em vector3
        towerHead.rotation = Quaternion.Euler(rotation);
    }
}
