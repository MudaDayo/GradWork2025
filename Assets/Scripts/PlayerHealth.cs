using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Settings")]
    public Image healthBarFill;

    [Header("References")]
    public PlayerController3D playerController;


    [Header("Hit Response")]
    public Rigidbody rb;
    public float hitTeleportDistance = 1.5f; // distance pushed away from hit

    void Awake()
    {
        if (playerController == null)
        playerController = GetComponent<PlayerController3D>();


        currentHealth = maxHealth;
        UpdateHealthBar();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10f, other.transform.position);
        }
    }

    public void TakeDamage(float amount, Vector3 hitSource)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthBar();
        TeleportFromHit(hitSource);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void TeleportFromHit(Vector3 hitSource)
{
    Vector3 dir = (transform.position - hitSource).normalized;
    dir.y = 0f;

    if (rb != null)
        rb.linearVelocity = Vector3.zero;

    transform.position += dir * hitTeleportDistance;

    // lock attacks after hit
    if (playerController != null)
        playerController.LockAttack(1f);
}


    void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        switch (GameData.gameState)
        {
            case 0:
                GameData.playerDeaths++;
                break;
            case 1:
                GameData.simpleBotDeaths++;
                break;
            case 2:
                GameData.trainedBotDeaths++;
                break;
        }

        transform.position = new Vector3(0, 1, -6);
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        //gameObject.SetActive(true);
    }
}
