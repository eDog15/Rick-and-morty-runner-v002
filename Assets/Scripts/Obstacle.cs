using UnityEngine;

// A simple marker script for obstacle objects.
// Attach this to any obstacle prefab.
public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // For now, we just log a message.
            // Later, this could call a method on a PlayerHealth script.
            Debug.Log("Player collided with an obstacle!");

            // Optional: de-activate the obstacle to prevent multiple triggers
            // gameObject.SetActive(false);
        }
    }
}
