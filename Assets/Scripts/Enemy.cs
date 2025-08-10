using UnityEngine;

// This script defines the behavior of a simple enemy.
public class Enemy : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    // Static variable to keep track of the number of enemies alive
    public static int enemyCount = 0;

    void Start()
    {
        currentHealth = maxHealth;
        enemyCount++;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        enemyCount--;
        Debug.Log("Enemy defeated! Enemies remaining: " + enemyCount);

        // Notify the GameManager to add score for the kill
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddKillScore();
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider belongs to a projectile that is not an enemy projectile
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            // This check will be needed once we have shooting enemies
            // if (!projectile.isEnemyProjectile)
            // {
                TakeDamage(projectile.damage);
                Destroy(collision.gameObject);
            // }
        }
    }
}
