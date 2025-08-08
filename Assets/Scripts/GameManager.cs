using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script acts as a "director" for the game, managing the flow of events.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Core Components")]
    public EnemySpawner enemySpawner;

    [Header("Portal Settings")]
    [Tooltip("A list of all possible portal prefabs the GameManager can spawn.")]
    public List<GameObject> portalPrefabs;
    [Tooltip("The location where portals will be spawned.")]
    public Transform portalSpawnPoint;

    [Header("Game Flow")]
    [Tooltip("Time in seconds to wait between clearing a pattern and starting the next one.")]
    public float timeBetweenPatterns = 5f;


    private void Awake()
    {
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
        // This is the main game loop that runs indefinitely.
        while (true)
        {
            // --- Step 1: Execute a random enemy spawn pattern ---
            int patternCount = enemySpawner.spawnPatterns.Count;
            if (patternCount == 0)
            {
                Debug.LogWarning("No spawn patterns defined in the EnemySpawner. Game loop will pause.");
                yield return new WaitUntil(() => enemySpawner.spawnPatterns.Count > 0); // Wait until patterns are added
            }

            int randomPatternIndex = Random.Range(0, patternCount);
            enemySpawner.ExecutePattern(randomPatternIndex);

            // --- Step 2: Wait for the pattern to be cleared ---
            // We wait until the pattern is fully spawned AND all enemies are defeated.
            // A simple way is to just wait for enemy count to be zero after a brief delay.
            yield return new WaitForSeconds(1f); // Small initial delay
            yield return new WaitUntil(() => Enemy.enemyCount == 0);
            Debug.Log("Pattern cleared!");

            // --- Step 3: Spawn a reward portal ---
            SpawnPortal();

            // --- Step 4: Wait before starting the next pattern ---
            yield return new WaitForSeconds(timeBetweenPatterns);
        }
    }

    void SpawnPortal()
    {
        if (portalPrefabs == null || portalPrefabs.Count == 0 || portalSpawnPoint == null)
        {
            Debug.LogWarning("Portal prefabs or spawn point not set up in GameManager. Skipping portal spawn.");
            return;
        }

        // Pick a random portal from the list
        int randomPortalIndex = Random.Range(0, portalPrefabs.Count);
        GameObject portalPrefab = portalPrefabs[randomPortalIndex];

        // Spawn the chosen portal
        Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);
        Debug.Log("Spawning portal: " + portalPrefab.name);
    }
}
