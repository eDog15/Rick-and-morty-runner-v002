using UnityEngine;

// This script generates a simple track for the character to move on.
// To use this script:
// 1. Create a new empty GameObject in your scene.
// 2. Attach this script to the empty GameObject.
// 3. Adjust the trackLength and trackWidth properties in the Inspector.
// 4. Run the scene, and the track will be generated.

public class TrackGenerator : MonoBehaviour
{
    public int trackLength = 100;
    public int trackWidth = 5;

    void Start()
    {
        // Create the track
        GameObject track = GameObject.CreatePrimitive(PrimitiveType.Cube);
        track.name = "Track";
        track.transform.localScale = new Vector3(trackWidth, 0.1f, trackLength);
        track.transform.position = new Vector3(0, -0.5f, 0);

        // Add a simple material to the track to give it some color
        Renderer trackRenderer = track.GetComponent<Renderer>();
        trackRenderer.material.color = Color.gray;
    }
}
