using System.Collections.Generic;
using UnityEngine;

// This script generates an endless, moving corridor by recycling track segments.
public class TrackGenerator : MonoBehaviour
{
    public static TrackGenerator Instance { get; private set; }

    [Header("World Settings")]
    public float worldSpeed = 10f;

    [Header("Track Segments")]
    public GameObject trackSegmentPrefab;
    public int initialSegments = 5;

    private Queue<GameObject> activeSegments = new Queue<GameObject>();
    private float segmentLength;
    private float nextSpawnZ;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (trackSegmentPrefab == null)
        {
            Debug.LogError("Track Segment Prefab is not assigned!");
            return;
        }

        segmentLength = trackSegmentPrefab.GetComponentInChildren<Renderer>().bounds.size.z;
        nextSpawnZ = 0;

        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        foreach (GameObject segment in activeSegments)
        {
            segment.transform.position += Vector3.back * worldSpeed * Time.deltaTime;
        }

        if (activeSegments.Peek().transform.position.z <= -segmentLength)
        {
            RecycleOldestSegment();
        }
    }

    void SpawnSegment()
    {
        GameObject newSegment = Instantiate(trackSegmentPrefab, transform);
        newSegment.transform.position = new Vector3(0, 0, nextSpawnZ);
        nextSpawnZ += segmentLength;
        activeSegments.Enqueue(newSegment);
    }

    void RecycleOldestSegment()
    {
        GameObject oldestSegment = activeSegments.Dequeue();
        oldestSegment.transform.position = new Vector3(0, 0, nextSpawnZ - segmentLength);
        activeSegments.Enqueue(oldestSegment);
    }
}
