using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int maxEnemiesPerWave = 10;

    [Header("Health-Based Parameters")]
    public int enemiesAt100Percent = 5;
    public int enemiesAt50Percent = 7;
    public int enemiesAt25Percent = 10;
    public float waveDelayAt100Percent = 10f;
    public float waveDelayAt50Percent = 7f;
    public float waveDelayAt25Percent = 5f;
    public float spawnIntervalAt100Percent = 2f;
    public float spawnIntervalAt50Percent = 1.5f;
    public float spawnIntervalAt25Percent = 1f;

    private int currentWaveEnemyCount;
    private int activeEnemyCount;
    private bool isSpawning = false;

    private string currentHealthStage = "Full Health"; // Tracks current health stage for debugging
    private EntityHealth entityHealth; // Reference to EntityHealth component

    void Start()
    {
        // Get the EntityHealth component from the same GameObject
        entityHealth = GetComponent<EntityHealth>();
        if (entityHealth == null)
        {
            Debug.LogError("EntityHealth component not found on the BossSpawner GameObject!");
        }
    }

    void Update()
    {
        // Start spawning a new wave if no enemies are active
        if (!isSpawning && activeEnemyCount == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;

        int enemiesToSpawn = GetEnemiesToSpawn();
        float waveDelay = GetWaveDelay();
        float spawnInterval = GetSpawnInterval();

        currentWaveEnemyCount = 0;

        while (currentWaveEnemyCount < enemiesToSpawn)
        {
            int enemiesThisBatch = Random.Range(1, 4); // Spawn 1-3 enemies at a time
            for (int i = 0; i < enemiesThisBatch; i++)
            {
                if (currentWaveEnemyCount >= enemiesToSpawn)
                    break;

                SpawnEnemy();
                currentWaveEnemyCount++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(waveDelay);
        isSpawning = false;
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);

        // Add EntityHealth component and register callback
        EntityHealth entityHealth = newEnemy.GetComponent<EntityHealth>();
        if (entityHealth == null)
        {
            entityHealth = newEnemy.AddComponent<EntityHealth>();
        }

        entityHealth.rootObject = newEnemy;
        entityHealth.pointsOnDeath = 0; // No points awarded for these enemies
        entityHealth.OnDeath += HandleEnemyDeath;

        activeEnemyCount++;
    }

    void HandleEnemyDeath(GameObject enemy)
    {
        activeEnemyCount--;
        Debug.Log("Enemy defeated. Active enemies left: " + activeEnemyCount);
    }

    int GetEnemiesToSpawn()
    {
        if (entityHealth == null)
            return enemiesAt100Percent; // Fallback if EntityHealth is missing

        float bossHealth = entityHealth.CurrentHealth;
        float maxHealth = entityHealth.maxHealth;

        if (bossHealth <= maxHealth * 0.25f)
        {
            LogHealthStage("Critical Health (≤25%)");
            return enemiesAt25Percent;
        }
        else if (bossHealth <= maxHealth * 0.5f)
        {
            LogHealthStage("Half Health (≤50%)");
            return enemiesAt50Percent;
        }
        else
        {
            LogHealthStage("Full Health (>50%)");
            return enemiesAt100Percent;
        }
    }

    float GetWaveDelay()
    {
        if (entityHealth == null)
            return waveDelayAt100Percent; // Fallback if EntityHealth is missing

        float bossHealth = entityHealth.CurrentHealth;
        float maxHealth = entityHealth.maxHealth;

        if (bossHealth <= maxHealth * 0.25f)
            return waveDelayAt25Percent;
        else if (bossHealth <= maxHealth * 0.5f)
            return waveDelayAt50Percent;
        else
            return waveDelayAt100Percent;
    }

    float GetSpawnInterval()
    {
        if (entityHealth == null)
            return spawnIntervalAt100Percent; // Fallback if EntityHealth is missing

        float bossHealth = entityHealth.CurrentHealth;
        float maxHealth = entityHealth.maxHealth;

        if (bossHealth <= maxHealth * 0.25f)
            return spawnIntervalAt25Percent;
        else if (bossHealth <= maxHealth * 0.5f)
            return spawnIntervalAt50Percent;
        else
            return spawnIntervalAt100Percent;
    }

    void LogHealthStage(string newStage)
    {
        if (currentHealthStage != newStage)
        {
            currentHealthStage = newStage;
            Debug.Log($"Boss Health Stage: {currentHealthStage}");
        }
    }
}
