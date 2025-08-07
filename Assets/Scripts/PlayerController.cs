using UnityEngine;

// This script controls the player's movement.
// To use this script:
// 1. Create a 3D object for your player (e.g., a Cube or Capsule).
// 2. Attach this script to the player object.
// 3. Add a Rigidbody component to the player object. This is required for physics-based movement.
// 4. Adjust the speed property in the Inspector to change the movement speed.

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rb.velocity = movement * speed;
    }
}
