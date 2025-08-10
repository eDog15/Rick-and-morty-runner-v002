using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Enum to define the available weapon types
    public enum WeaponType
    {
        Pistol,
        Shotgun,
        GrenadeLauncher
    }

    [Header("Weapon Settings")]
    public WeaponType currentWeapon = WeaponType.Pistol;

    [Header("Prefabs")]
    public GameObject projectilePrefab;
    public GameObject grenadePrefab; // Prefab for the grenade

    [Header("General Setup")]
    public Transform firePoint;

    [Header("General Stats")]
    public float fireRate = 2f;
    public int projectileDamage = 1;
    public bool isAutoFire = true; // Toggle for automatic shooting

    [Header("Shotgun-Specific Stats")]
    public int shotgunProjectileCount = 3;
    public float shotgunSpreadAngle = 15f;

    private float nextFireTime = 0f;

    void Update()
    {
        // If auto-fire is on, shoot whenever the cooldown is ready.
        if (isAutoFire && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                ShootPistol();
                break;
            case WeaponType.Shotgun:
                ShootShotgun();
                break;
            case WeaponType.GrenadeLauncher:
                ShootGrenadeLauncher();
                break;
        }
    }

    void ShootPistol()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = projectileDamage;
        }
    }

    void ShootShotgun()
    {
        if (projectilePrefab == null || firePoint == null) return;

        float angleStep = shotgunSpreadAngle / (shotgunProjectileCount > 1 ? (float)(shotgunProjectileCount - 1) : 1f);
        float startingAngle = -shotgunSpreadAngle / 2f;

        for (int i = 0; i < shotgunProjectileCount; i++)
        {
            float currentAngle = startingAngle + i * angleStep;
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, currentAngle, 0);

            GameObject projGO = Instantiate(projectilePrefab, firePoint.position, rotation);
            Projectile projectile = projGO.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.damage = projectileDamage;
            }
        }
    }

    void ShootGrenadeLauncher()
    {
        if (grenadePrefab == null || firePoint == null) return;

        Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
    }

    // --- PUBLIC WEAPON MANAGEMENT ---

    public void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log("Switched to weapon: " + newWeapon.ToString());
    }

    // --- PUBLIC UPGRADE METHODS ---

    public void UpgradeFireRate(float fireRateIncrease)
    {
        fireRate += fireRateIncrease;
        Debug.Log("Fire Rate upgraded to: " + fireRate);
    }

    public void UpgradeDamage(int amount)
    {
        projectileDamage += amount;
        Debug.Log("Projectile Damage upgraded to: " + projectileDamage);
    }
}
