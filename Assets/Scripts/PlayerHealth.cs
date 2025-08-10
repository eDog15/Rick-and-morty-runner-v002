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
        // Here you would typically update the UI with the starting health
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current health: " + currentHealth);

        // Here you would update the UI

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Game Over!");

        // Disable player components to stop movement and shooting
        // This is a simple way to end the run.
        GetComponent<PlayerLaneMovement>().enabled = false;
        GetComponent<PlayerShooting>().enabled = false;

        // Here you would typically trigger a Game Over screen
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        // Check for collision with an Enemy
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            TakeDamage(1);
            // Destroy the enemy on collision to prevent multiple damage triggers
            Destroy(collision.gameObject);
            return; // Exit after handling collision
        }

        // Check for collision with an Obstacle
        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            TakeDamage(1);
            // Optionally destroy the obstacle as well, or just let the player phase through
            // For now, we assume obstacles are solid and stay
        }
    }
}
