using System.Collections.Generic;
using UnityEngine;

// This script procedurally generates an endless corridor.
// To use this script:
// 1. Create a prefab that represents one segment of your corridor (floor and walls).
// 2. Create an empty GameObject in your scene and attach this script to it.
// 3. Assign your player object to the 'player' field.
// 4. Assign your corridor segment prefab to the 'trackSegmentPrefab' field.
// 5. Adjust the number of visible segments to control how far the track generates.

public class TrackGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject trackSegmentPrefab;
    public int visibleSegments = 5; // Number of segments to keep active at a time

    private float segmentLength = 0f;
    private float spawnZ = 0f;
    private List<GameObject> activeSegments = new List<GameObject>();

    void Start()
    {
        if (player == null || trackSegmentPrefab == null)
        {
            Debug.LogError("Player or Track Segment Prefab not assigned in TrackGenerator.");
            return;
        }

        // It's crucial that the prefab has a defined size. We get it from the MeshRenderer bounds.
        // Ensure your prefab has a MeshRenderer or this will fail.
        segmentLength = trackSegmentPrefab.GetComponentInChildren<Renderer>().bounds.size.z;

        // Spawn initial segments
        for (int i = 0; i < visibleSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // Check if the player has moved past the second segment
        if (player.position.z - segmentLength > spawnZ - (visibleSegments * segmentLength))
        {
            SpawnSegment();
            DeleteOldestSegment();
        }
    }

    private void SpawnSegment()
    {
        GameObject newSegment = Instantiate(trackSegmentPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        newSegment.transform.SetParent(transform);
        activeSegments.Add(newSegment);
        spawnZ += segmentLength;
    }

    private void DeleteOldestSegment()
    {
        Destroy(activeSegments[0]);
        activeSegments.RemoveAt(0);
    }
}
