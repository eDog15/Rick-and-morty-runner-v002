using UnityEngine;

// This script controls the behavior of a projectile.
public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 20f;
    public int damage = 1; // The amount of damage this projectile deals

    [Header("Lifetime")]
    public float lifetime = 5f;

    void Start()
    {
        // Give the projectile an initial forward velocity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }

        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }
}
