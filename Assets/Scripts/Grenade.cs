using UnityEngine;

// This script controls the behavior of a grenade projectile.
// To use this script:
// 1. Create a grenade prefab (e.g., a Sphere).
// 2. Attach this script and a Rigidbody component to it. MAKE SURE 'Use Gravity' IS CHECKED on the Rigidbody.
// 3. Assign an explosion particle effect to the 'explosionEffect' field if you have one.
// 4. Adjust the public variables to control its behavior.

public class Grenade : MonoBehaviour
{
    [Header("Launch Force")]
    public float forwardForce = 15f;
    public float upwardForce = 5f;

    [Header("Explosion")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 10;
    public GameObject explosionEffect;

    private bool hasExploded = false;

    void Start()
    {
        // Give the grenade an initial launch velocity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * forwardForce + transform.up * upwardForce, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Explode on first impact
        if (!hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // --- Visual Effect ---
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // --- Find and Damage Targets ---
        // Find all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Damage enemies
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Apply explosion force to rigidbodies
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // --- Cleanup ---
        // Destroy the grenade object after the explosion
        Destroy(gameObject);
    }
}
