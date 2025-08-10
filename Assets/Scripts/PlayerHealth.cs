using UnityEngine;

// This script manages the player's health and handles damage from collisions.
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    private bool isDead = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // The GameManager is now responsible for handling the Game Over sequence
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandleGameOver();
        }

        // We can also disable the player object entirely
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
            return;
        }

        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            TakeDamage(1);
        }
    }
}
