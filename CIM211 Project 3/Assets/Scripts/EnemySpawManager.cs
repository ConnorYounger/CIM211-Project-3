using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawManager : MonoBehaviour
{
    [Header("Refrences")]
    public LevelStats levelStats;

    public GameObject enemyPrefab;

    public EnemySpawner[] spawnPoints;

    public LootPool[] lootPools;

    [Header("Wave Stats")]
    public int currentWave;
    public int baseWaveEnemyCount = 5;
    public float waveEnemyCountMultiplier = 0.5f;
    public int waveEnemySpawnedCount;
    public int waveEnemyKilledCount;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = levelStats.currentWave;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            SpawnNewWave();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentWave++;
        }

        if(Input.GetKeyDown(KeyCode.DownArrow) && currentWave > 0)
        {
            currentWave--;
        }
    }

    void NewWave()
    {
        currentWave++;
        SpawnNewWave();
    }

    void SpawnNewWave()
    {
        waveEnemyKilledCount = 0;
        waveEnemySpawnedCount = Mathf.RoundToInt(baseWaveEnemyCount * (currentWave + waveEnemyCountMultiplier));
        EnemySpawManager sM = gameObject.GetComponent<EnemySpawManager>();

        Debug.Log("Enemies to spawn: " + waveEnemySpawnedCount);

        for(int i = 0; i < waveEnemySpawnedCount; i++)
        {
            int randPoint = Random.Range(0, spawnPoints.Length);

            if (currentWave - 1 < lootPools.Length)
                spawnPoints[randPoint].SpawnEnemy(enemyPrefab, currentWave, lootPools[currentWave - 1], sM);
            else
                spawnPoints[randPoint].SpawnEnemy(enemyPrefab, currentWave, lootPools[lootPools.Length - 1], sM);
        }
    }

    public void KilledEnemy()
    {
        waveEnemyKilledCount++;

        if(waveEnemyKilledCount == waveEnemySpawnedCount)
        {
            NewWave();
        }
    }
}
