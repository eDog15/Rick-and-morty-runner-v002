using UnityEngine;

// This script defines a portal that upgrades the player upon entry.
public class Portal : MonoBehaviour
{
    public enum UpgradeType
    {
        FireRate,
        MultiShot,
        Damage
    }

    [Header("Upgrade Settings")]
    public UpgradeType upgradeType;

    [Header("Values")]
    public float fireRateIncrease = 0.5f;
    public int additionalProjectiles = 1;
    public int damageIncrease = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();

            if (playerShooting != null)
            {
                ApplyUpgrade(playerShooting);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyUpgrade(PlayerShooting player)
    {
        switch (upgradeType)
        {
            case UpgradeType.FireRate:
                player.UpgradeFireRate(fireRateIncrease);
                Debug.Log("Player picked up a Fire Rate upgrade.");
                break;
            case UpgradeType.MultiShot:
                player.IncreaseProjectiles(additionalProjectiles);
                Debug.Log("Player picked up a Multi-Shot upgrade.");
                break;
            case UpgradeType.Damage:
                player.UpgradeDamage(damageIncrease);
                Debug.Log("Player picked up a Damage upgrade.");
                break;
        }
    }
}
