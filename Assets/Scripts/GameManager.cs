using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public int totalWaves = 5;
    public int totalEnemyCount = 10; // Example: each wave will have 10 enemies total.

    private float spawnRange = 24;
    public int enemyCount;
    private int currentWave = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0 && currentWave < totalWaves)
        {
            currentWave++;
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {

        int remainingCount = totalEnemyCount;
        List<GameObject> waveEnemies = new List<GameObject>();

        // Randomly split enemy types per wave
        int enemy1Count = 0;
        int enemy2Count = 0;

        while (remainingCount > 0)
        {
            int enemyType = Random.Range(0, 2); // Randomly choose either enemy 1 or enemy 2

            if (enemyType == 0 && remainingCount >= 1) // Enemy 1 (worth 1 count)
            {
                enemy1Count++;
                remainingCount -= 1;
            }
            else if (enemyType == 1 && remainingCount >= 2) // Enemy 2 (worth 2 counts)
            {
                enemy2Count++;
                remainingCount -= 2;
            }
        }

        // Now spawn the enemies for this wave
        SpawnEnemyType(enemyPrefab1, enemy1Count);
        SpawnEnemyType(enemyPrefab2, enemy2Count);

    }

    void SpawnEnemyType(GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        var spawnPosX = Random.Range(-spawnRange, spawnRange);
        var spawnPosZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnPosX, 0.57f, spawnPosZ);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
