using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Core Components")]
    public EnemySpawner enemySpawner;

    [Header("Portal Settings")]
    public List<GameObject> portalPrefabs;
    public Transform portalSpawnPoint;

    [Header("Game Flow")]
    public float timeBetweenPatterns = 5f;
    private bool isGameOver = false;

    [Header("Scoring & Currency")]
    public int scorePerKill = 100;
    public int scorePerSecond = 10;
    [Tooltip("How many score points are worth 1 gold coin.")]
    public int scoreToGoldConversionRate = 1000;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI goldEarnedText; // Text to show gold earned this run

    private int currentKillScore;
    private float sessionTimer;
    private const string GoldPlayerPrefsKey = "PlayerTotalGold";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Time.timeScale = 1f;
    }

    void Start()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("Enemy Spawner not assigned in the GameManager.");
            return;
        }

        if(gameOverScreen != null) gameOverScreen.SetActive(false);

        currentKillScore = 0;
        sessionTimer = 0f;
        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;
        sessionTimer += Time.deltaTime;
        UpdateScoreUI();
    }

    public void AddKillScore()
    {
        if (isGameOver) return;
        currentKillScore += scorePerKill;
    }

    private int GetCurrentTotalScore()
    {
        int timeScore = (int)(sessionTimer * scorePerSecond);
        return currentKillScore + timeScore;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GetCurrentTotalScore().ToString();
        }
    }

    public void HandleGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        int finalScore = GetCurrentTotalScore();
        Debug.Log("Game Over! Final Score: " + finalScore);

        // --- Currency Conversion & Saving ---
        int goldEarned = finalScore / scoreToGoldConversionRate;
        int totalGold = PlayerPrefs.GetInt(GoldPlayerPrefsKey, 0);
        totalGold += goldEarned;
        PlayerPrefs.SetInt(GoldPlayerPrefsKey, totalGold);
        PlayerPrefs.Save(); // Ensure data is written to disk
        Debug.Log("Earned " + goldEarned + " gold. Total gold: " + totalGold);

        // --- Show Game Over Screen ---
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            if (finalScoreText != null) finalScoreText.text = "Final Score: " + finalScore;
            if (goldEarnedText != null) goldEarnedText.text = "Gold Earned: " + goldEarned;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator GameLoop()
    {
        while (!isGameOver)
        {
            int patternCount = enemySpawner.spawnPatterns.Count;
            if (patternCount > 0)
            {
                int randomPatternIndex = Random.Range(0, patternCount);
                enemySpawner.ExecutePattern(randomPatternIndex);
            }

            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => Enemy.enemyCount == 0);

            if (isGameOver) yield break;

            Debug.Log("Pattern cleared!");
            SpawnPortal();

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
        int randomPortalIndex = Random.Range(0, portalPrefabs.Count);
        GameObject portalPrefab = portalPrefabs[randomPortalIndex];
        Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);
    }
}
