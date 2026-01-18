using UnityEngine;

public class BehaviourValueChecker : MonoBehaviour
{
    [Header("References")]
    public GameObject enemy; // must have Transform + SimpleEnemyAttack

    [Header("Computed Values (Read Only)")]
    public float distanceToEnemy;
    public Vector3 vectorToEnemy;
    public Vector3 directionToEnemy;
    public Vector3 directionAwayFromEnemy;
    public float enemyCooldownTimer;

    private SimpleEnemyAttack enemyAttack;

    void Awake()
    {
        if (enemy != null)
            enemyAttack = enemy.GetComponent<SimpleEnemyAttack>();
    }

    void Update()
    {
        if (enemy == null)
            return;

        ComputeSpatialValues();
        ComputeEnemyValues();
    }

    // ─────────── COMPUTATIONS ───────────

    void ComputeSpatialValues()
    {
        vectorToEnemy = enemy.transform.position - transform.position;

        distanceToEnemy = vectorToEnemy.magnitude;

        if (distanceToEnemy > 0.0001f)
        {
            directionToEnemy = vectorToEnemy.normalized;
            directionAwayFromEnemy = -directionToEnemy;
        }
        else
        {
            directionToEnemy = Vector3.zero;
            directionAwayFromEnemy = Vector3.zero;
        }
    }

    void ComputeEnemyValues()
    {
        if (enemyAttack != null)
        {
            enemyCooldownTimer = enemyAttack.cooldownTimer;
        }
    }
}
