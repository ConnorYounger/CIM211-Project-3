using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawManager : MonoBehaviour
{
    public LevelStats levelStats;

    public int currentWave;
    public int baseWaveEnemyCount = 5;
    public int waveEnemyCount;

    public GameObject enemyPrefab;

    public EnemySpawner[] spawnPoints;

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
    }

    void SpawnNewWave()
    {
        foreach(EnemySpawner point in spawnPoints)
        {
            point.SpawnEnemy(enemyPrefab, currentWave);
        }
    }
}
