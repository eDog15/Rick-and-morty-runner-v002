using UnityEngine;

// This script moves the object it's attached to along with the world.
// Attach this to any object that should move towards the player, like enemies.
public class WorldMover : MonoBehaviour
{
    private float speed;

    void Start()
    {
        // Get the world speed from the TrackGenerator singleton
        if (TrackGenerator.Instance != null)
        {
            speed = TrackGenerator.Instance.worldSpeed;
        }
        else
        {
            Debug.LogError("WorldMover cannot find an instance of TrackGenerator in the scene.");
            // Disable this component if the TrackGenerator is missing
            enabled = false;
        }
    }

    void Update()
    {
        // Move the object backwards (towards the player at z=0)
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
}
