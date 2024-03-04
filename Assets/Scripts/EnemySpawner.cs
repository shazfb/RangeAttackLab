using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int initialMinEnemies = 10;
    public float enemiesIncreasePercentage = 0.2f; // 10% increase per wave
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;

    private int currentWave = 1;
    private int enemiesRemaining;
    private bool spawningEnabled = true;
    private bool countdownInProgress = false; // Flag to control spawning during countdown
    private int enemiesKilled = 0; // Counter for enemies killed in the current wave

    private int lastSpawnPointIndex = -1;

    void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (enemiesRemaining <= 0 && !countdownInProgress && enemiesKilled == 0)
        {
            currentWave++;
            StartWave();
        }

        if (countdownInProgress)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(false);
            }
        }
        
    }

    private void StartWave()
    {
        spawningEnabled = false; // Disable spawning initially
        enemiesRemaining = Mathf.RoundToInt(initialMinEnemies * Mathf.Pow(1 + enemiesIncreasePercentage, currentWave - 1)); // Increase enemies for next wave
        enemiesKilled = 0; // Reset enemies killed for the new wave
        waveText.text = "Wave: " + currentWave;

        StartCoroutine(CountdownAndSpawn());
    }

    private System.Collections.IEnumerator CountdownAndSpawn()
    {
        countdownInProgress = true;
        int countdownDuration = 10;
        while (countdownDuration > 0)
        {
            countdownText.text = "Next wave in: " + countdownDuration + " seconds";
            yield return new WaitForSeconds(1);
            countdownDuration--;
        }

        countdownText.text = "";
        countdownInProgress = false;

        spawningEnabled = true; // Enable spawning after countdown

        // Start spawning enemies
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (spawningEnabled && GameObject.FindGameObjectsWithTag("Enemy").Length < initialMinEnemies)
        {
            int nextSpawnPointIndex;

            // Choose a random spawn point index different from the last used one
            do
            {
                nextSpawnPointIndex = Random.Range(0, spawnPoints.Length);
            } while (nextSpawnPointIndex == lastSpawnPointIndex);

            lastSpawnPointIndex = nextSpawnPointIndex;

            Transform spawnPoint = spawnPoints[nextSpawnPointIndex];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemiesRemaining--;
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        enemiesRemaining--;

        if (enemiesRemaining <= 0 && !countdownInProgress && enemiesKilled == initialMinEnemies)
        {
            currentWave++;
            StartWave();
        }
    }
}