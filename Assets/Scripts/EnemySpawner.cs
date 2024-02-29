using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int minEnemies = 10;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;

    private void Start()
    {
        for (int i = 0; i < minEnemies; i++)
        {
            SpawnEnemy();
        }

        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < minEnemies)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
