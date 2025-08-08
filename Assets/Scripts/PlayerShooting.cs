using UnityEngine;

// This script handles the player's shooting mechanics and can be upgraded.
public class PlayerShooting : MonoBehaviour
{
    [Header("Setup")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Shooting Stats")]
    public float fireRate = 2f;
    public int numberOfProjectiles = 1;
    public float spreadAngle = 10f;
    public int projectileDamage = 1;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or fire point not assigned.");
            return;
        }

        float angleStep = spreadAngle / (numberOfProjectiles > 1 ? (float)(numberOfProjectiles - 1) : 1f);
        float startingAngle = -spreadAngle / 2f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float currentAngle = startingAngle + i * angleStep;
            if (numberOfProjectiles == 1)
            {
                currentAngle = 0; // Ensure single projectile fires straight.
            }

            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, currentAngle, 0);

            // Instantiate the projectile
            GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, rotation);

            // Set the damage for the projectile
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.damage = projectileDamage;
            }
        }
    }

    // --- PUBLIC UPGRADE METHODS ---

    public void UpgradeFireRate(float fireRateIncrease)
    {
        fireRate += fireRateIncrease;
        Debug.Log("Fire Rate upgraded to: " + fireRate);
    }

    public void IncreaseProjectiles(int amount)
    {
        numberOfProjectiles += amount;
        Debug.Log("Number of projectiles upgraded to: " + numberOfProjectiles);
    }

    public void UpgradeDamage(int amount)
    {
        projectileDamage += amount;
        Debug.Log("Projectile Damage upgraded to: " + projectileDamage);
    }
}
