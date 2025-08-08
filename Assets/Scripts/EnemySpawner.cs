using System.Collections;
using UnityEngine;

// This script handles the spawning of enemies based on commands from the GameManager.
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    // This method will be called by the GameManager to start a wave.
    public void StartWave(int numberOfEnemies, float spawnRate)
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Enemy prefab or spawn points not set up in the EnemySpawner.");
            return;
        }
        StartCoroutine(SpawnWaveCoroutine(numberOfEnemies, spawnRate));
    }

    IEnumerator SpawnWaveCoroutine(int numberOfEnemies, float spawnRate)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Wait for the specified time before spawning the next enemy
            yield return new WaitForSeconds(1f / spawnRate);

            // Pick a random spawn point from the array
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnPointIndex];

            // Spawn the enemy at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
