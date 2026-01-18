using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Settings")]
    public Image healthBarFill; // Assign the foreground image here


    [SerializeField] private GameObject player;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage(10f); // Adjust per hit
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Set fill amount (0 to 1)
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        ResetHealth();
        // switch (GameData.gameState)
        // {
        //     case 0:
        //         GameData.gameState = 1;
        //         break;
        //     case 1:
        //         GameData.gameState = 0;
        //         break;
        //     default:
        //         GameData.gameState = 0;
        //         break;
        // }

        player.transform.position = new Vector3(0, 1, -6);

        //gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        //gameObject.SetActive(true);
    }
}
