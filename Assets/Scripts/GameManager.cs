using System.Collections;
using UnityEngine;

// This script manages the overall game flow, including waves and player progression.
public class GameManager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of the GameManager exists
    public static GameManager Instance { get; private set; }

    public EnemySpawner enemySpawner;
    public Wave[] waves;

    private int currentWaveIndex = 0;

    // A simple class to define the properties of an enemy wave
    [System.Serializable]
    public class Wave
    {
        public string name;
        public int numberOfEnemies;
        public float spawnRate;
        public float timeBetweenWaves = 5f;
    }

    private void Awake()
    {
        // Implement the Singleton pattern
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
        if (enemySpawner == null)
        {
            Debug.LogError("Enemy Spawner not assigned in the GameManager.");
            return;
        }
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        // Loop through all the waves defined in the inspector
        foreach (Wave wave in waves)
        {
            Debug.Log("Starting " + wave.name);
            enemySpawner.StartWave(wave.numberOfEnemies, wave.spawnRate);

            // Wait until all enemies in the current wave are defeated
            yield return new WaitUntil(() => Enemy.enemyCount == 0);

            Debug.Log(wave.name + " completed!");

            // --- Placeholder for spawning a portal ---
            // Here you could instantiate a portal prefab to give the player an upgrade.
            // Example: Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);

            // Wait for a specified time before starting the next wave
            yield return new WaitForSeconds(wave.timeBetweenWaves);
        }

        Debug.Log("All waves completed! Game Over!");
        // Here you could trigger a game over screen or a victory condition
    }
}
