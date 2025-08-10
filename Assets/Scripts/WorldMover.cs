using UnityEngine;

// This script moves the object it's attached to along with the world.
// It includes a speed multiplier to allow for variations in movement speed.
// Attach this to any object that should move towards the player, like enemies.
public class WorldMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Multiplier for the base world speed. >1 for faster, <1 for slower.")]
    public float speedMultiplier = 1f;

    private float baseWorldSpeed;

    void Start()
    {
        // Get the world speed from the TrackGenerator singleton
        if (TrackGenerator.Instance != null)
        {
            baseWorldSpeed = TrackGenerator.Instance.worldSpeed;
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
        // Calculate the final speed for this object
        float finalSpeed = baseWorldSpeed * speedMultiplier;

        // Move the object backwards (towards the player at z=0)
        transform.position += Vector3.back * finalSpeed * Time.deltaTime;
    }
}
