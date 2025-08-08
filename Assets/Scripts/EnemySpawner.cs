using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --- Data Structures for Spawn Patterns ---

[System.Serializable]
public class SpawnEvent
{
    [Tooltip("The type of enemy to spawn. For now, we only have one.")]
    public GameObject enemyPrefab;

    [Tooltip("Index of the spawn point in the EnemySpawner's list.")]
    public int spawnPointIndex;

    [Tooltip("Delay in seconds before this enemy spawns (after the previous event).")]
    public float delay;
}

[System.Serializable]
public class SpawnPattern
{
    public string patternName;
    public List<SpawnEvent> spawnEvents;
}


// This script executes pre-defined spawn patterns.
public class EnemySpawner : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("A list of all possible locations where enemies can spawn.")]
    public List<Transform> spawnPoints;

    [Header("Patterns")]
    [Tooltip("A list of all spawn patterns this spawner can execute.")]
    public List<SpawnPattern> spawnPatterns;

    // This method will be called by the GameManager to execute a specific pattern.
    public void ExecutePattern(int patternIndex)
    {
        if (spawnPatterns == null || spawnPatterns.Count == 0 || patternIndex >= spawnPatterns.Count)
        {
            Debug.LogError("Spawn patterns not set up or index is out of bounds.");
            return;
        }

        SpawnPattern pattern = spawnPatterns[patternIndex];
        StartCoroutine(ExecutePatternCoroutine(pattern));
    }

    IEnumerator ExecutePatternCoroutine(SpawnPattern pattern)
    {
        Debug.Log("Executing spawn pattern: " + pattern.patternName);
        foreach (SpawnEvent spawnEvent in pattern.spawnEvents)
        {
            // Wait for the specified delay
            yield return new WaitForSeconds(spawnEvent.delay);

            // Check if the spawn point index is valid
            if (spawnEvent.spawnPointIndex >= spawnPoints.Count)
            {
                Debug.LogWarning("Invalid spawn point index in pattern: " + pattern.patternName);
                continue; // Skip this event if the spawn point is invalid
            }

            // Get the prefab and spawn point
            GameObject prefabToSpawn = spawnEvent.enemyPrefab;
            Transform spawnPoint = spawnPoints[spawnEvent.spawnPointIndex];

            if (prefabToSpawn != null && spawnPoint != null)
            {
                // Spawn the enemy
                Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("Enemy prefab or spawn point is null for an event in pattern: " + pattern.patternName);
            }
        }
        Debug.Log("Finished spawn pattern: " + pattern.patternName);
    }
}
