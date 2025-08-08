using UnityEngine;

// This script controls the player's movement for a runner game.
// To use this script:
// 1. Create a 3D object for your player (e.g., a Cube or Capsule).
// 2. Attach this script to the player object.
// 3. Add a Rigidbody component to the player object. This is required for physics-based movement.
// 4. Adjust the forwardSpeed and sidewaysSpeed properties in the Inspector.

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10.0f;
    public float sidewaysSpeed = 5.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get horizontal input (A/D keys or left/right arrows)
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Calculate movement vectors
        Vector3 sidewaysMovement = new Vector3(moveHorizontal * sidewaysSpeed, 0.0f, 0.0f);
        Vector3 forwardMovement = new Vector3(0.0f, 0.0f, forwardSpeed);

        // Apply velocity to the Rigidbody
        // We combine forward movement with sideways movement.
        // We also preserve the current vertical velocity (rb.velocity.y) to allow for gravity and jumping.
        rb.velocity = forwardMovement + sidewaysMovement + new Vector3(0, rb.velocity.y, 0);
    }
}
