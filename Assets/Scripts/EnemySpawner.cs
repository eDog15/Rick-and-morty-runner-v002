using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --- Data Structures for Spawn Patterns ---

[System.Serializable]
public class SpawnEvent
{
    [Tooltip("The type of enemy to spawn. For now, we only have one.")]
    public GameObject enemyPrefab;

    [Tooltip("Delay in seconds before this enemy spawns (after the previous event).")]
    public float delay;
}

[System.Serializable]
public class SpawnPattern
{
    public string patternName;
    public List<SpawnEvent> spawnEvents;
}


// This script executes pre-defined spawn patterns within a dynamic area.
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [Tooltip("The center of the spawn area, relative to this object.")]
    public Vector3 areaCenter = new Vector3(0, 0, 30);
    [Tooltip("The size (width and height) of the spawn area.")]
    public Vector2 areaSize = new Vector2(5, 3);

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
            yield return new WaitForSeconds(spawnEvent.delay);

            GameObject prefabToSpawn = spawnEvent.enemyPrefab;
            if (prefabToSpawn == null)
            {
                Debug.LogWarning("Enemy prefab is null for an event in pattern: " + pattern.patternName);
                continue;
            }

            // Calculate a random position within the spawn area
            float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
            Vector3 spawnPos = transform.position + areaCenter + new Vector3(randomX, randomY, 0);

            // Spawn the enemy at the calculated random position
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
        Debug.Log("Finished spawn pattern: " + pattern.patternName);
    }

    // Draw a gizmo in the editor to visualize the spawn area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Green, semi-transparent
        Vector3 gizmoCenter = transform.position + areaCenter;
        Vector3 gizmoSize = new Vector3(areaSize.x, areaSize.y, 0.1f);
        Gizmos.DrawCube(gizmoCenter, gizmoSize);
    }
}
