using UnityEngine;
using System.Collections;
using TMPro;

public class SimpleEnemyAttack : MonoBehaviour
{
    [Header("References")]
    public GameObject attackPrefab;           // Prefab to spawn
    public Transform attackSpawnPoint;        // Where the prefab spawns
    public TextMeshProUGUI cooldownText;     // Assign TMP text here

    [Header("Attack Timing")]
    public float attackInterval = 2.5f;
    public float attackDuration = 0.4f;      // How long the prefab exists

    public static float lastEnemyAttackTime;


    // ─────────── State ───────────
    private bool isAttacking;
    public float cooldownTimer;

    void Awake()
    {
        cooldownTimer = attackInterval;
    }

    void Start()
    {
        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        // Update cooldown timer every frame
        if (!isAttacking)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
            UpdateCooldownText();
        }
        else
        {
            cooldownText.text = "!";
        }
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            TryAttack();
        }
    }

    public void TryAttack()
    {
        if (isAttacking)
            return;

        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // RECORD enemy attack time (for learning data)
        lastEnemyAttackTime = Time.time;

        // Spawn the attack prefab
        if (attackPrefab != null && attackSpawnPoint != null)
        {
            GameObject spawned = Instantiate(
                attackPrefab, 
                attackSpawnPoint.position, 
                attackSpawnPoint.rotation
            );

            Destroy(spawned, attackDuration);
        }

        cooldownText.text = "!";

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        cooldownTimer = attackInterval;
    }

    void UpdateCooldownText()
    {
        if (cooldownText != null)
            cooldownText.text = ((int)cooldownTimer + 1).ToString();
    }

    public bool IsAttacking() => isAttacking;
}
