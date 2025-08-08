using UnityEngine;

// This script defines a portal that applies a specific effect to the player.
public class Portal : MonoBehaviour
{
    // A single, comprehensive enum for all possible portal effects.
    public enum PortalType
    {
        SwitchWeapon,
        IncreaseDamage,
        DecreaseDamage,
        IncreaseFireRate,
        DecreaseFireRate
    }

    [Header("Portal Configuration")]
    public PortalType portalType;

    [Header("Effect Values")]
    [Tooltip("The amount to change damage by. Always use a positive number.")]
    public int damageAmount = 1;

    [Tooltip("The amount to change fire rate by. Always use a positive number.")]
    public float fireRateAmount = 0.5f;

    [Tooltip("The weapon to switch to if the portal type is SwitchWeapon.")]
    public PlayerShooting.WeaponType weaponToSwitchTo = PlayerShooting.WeaponType.Shotgun;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                ApplyEffect(playerShooting);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyEffect(PlayerShooting player)
    {
        switch (portalType)
        {
            case PortalType.SwitchWeapon:
                player.SwitchWeapon(weaponToSwitchTo);
                break;

            case PortalType.IncreaseDamage:
                player.UpgradeDamage(Mathf.Abs(damageAmount));
                Debug.Log("Player damage increased.");
                break;

            case PortalType.DecreaseDamage:
                // We use -Mathf.Abs to ensure it's always a decrease, even if a negative number is entered.
                player.UpgradeDamage(-Mathf.Abs(damageAmount));
                Debug.Log("Player damage decreased.");
                break;

            case PortalType.IncreaseFireRate:
                player.UpgradeFireRate(Mathf.Abs(fireRateAmount));
                Debug.Log("Player fire rate increased.");
                break;

            case PortalType.DecreaseFireRate:
                player.UpgradeFireRate(-Mathf.Abs(fireRateAmount));
                Debug.Log("Player fire rate decreased.");
                break;
        }
    }
}
