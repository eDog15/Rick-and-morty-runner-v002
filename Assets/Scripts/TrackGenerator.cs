using System.Collections.Generic;
using UnityEngine;

// This script generates an endless, moving corridor and populates it with obstacles.
public class TrackGenerator : MonoBehaviour
{
    public static TrackGenerator Instance { get; private set; }

    [Header("World Settings")]
    public float worldSpeed = 10f;

    [Header("Track Segments")]
    public GameObject trackSegmentPrefab;
    public int initialSegments = 5;

    [Header("Obstacles")]
    public List<GameObject> obstaclePrefabs;
    [Range(0, 1)]
    public float obstacleSpawnChance = 0.5f;

    private Queue<GameObject> activeSegments = new Queue<GameObject>();
    private float segmentLength;
    private float nextSpawnZ;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
            SpawnSegment(false); // Don't spawn obstacles on the very first segments
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

    void SpawnSegment(bool withObstacles)
    {
        GameObject newSegment = Instantiate(trackSegmentPrefab, transform);
        newSegment.transform.position = new Vector3(0, 0, nextSpawnZ);
        nextSpawnZ += segmentLength;

        if (withObstacles)
        {
            PopulateSegment(newSegment);
        }

        activeSegments.Enqueue(newSegment);
    }

    void RecycleOldestSegment()
    {
        GameObject oldestSegment = activeSegments.Dequeue();
        oldestSegment.transform.position = new Vector3(0, 0, nextSpawnZ - segmentLength);
        PopulateSegment(oldestSegment); // Populate the recycled segment with new obstacles
        activeSegments.Enqueue(oldestSegment);
    }

    void PopulateSegment(GameObject segment)
    {
        // Clear any old obstacles from the recycled segment first
        foreach (Transform child in segment.transform)
        {
            if (child.GetComponent<Obstacle>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        // Decide if we should spawn an obstacle at all
        if (Random.value > obstacleSpawnChance || obstaclePrefabs.Count == 0)
        {
            return;
        }

        // Find potential spawn points within the segment prefab
        Transform spawnPointsContainer = segment.transform.Find("ObstacleSpawnPoints");
        if (spawnPointsContainer == null || spawnPointsContainer.childCount == 0)
        {
            Debug.LogWarning("Track segment prefab is missing 'ObstacleSpawnPoints' container or it has no children.");
            return;
        }

        // Pick one random spawn point
        int pointIndex = Random.Range(0, spawnPointsContainer.childCount);
        Transform spawnPoint = spawnPointsContainer.GetChild(pointIndex);

        // Pick a random obstacle prefab
        int obstacleIndex = Random.Range(0, obstaclePrefabs.Count);
        GameObject obstaclePrefab = obstaclePrefabs[obstacleIndex];

        // Instantiate the obstacle and parent it to the spawn point
        Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
    }
}
