using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawManager : MonoBehaviour
{
    [Header("Refrences")]
    public LevelStats levelStats;

    public GameObject enemyPrefab;

    public GameObject spawnPoints;

    public LootPool[] lootPools;

    public bool spawnAtStart;

    [Header("UI")]
    public TMP_Text waveCounter;
    public TMP_Text enemyCounter;

    [Header("Wave Stats")]
    public int currentWave;
    public int baseWaveEnemyCount = 5;
    public float waveEnemyCountMultiplier = 0.5f;
    public int waveEnemySpawnedCount;
    public int waveEnemyKilledCount;

    public List<GameObject> aliveEnemies;
    public List<GameObject> spawnedEnemies;
    public List<GameObject> previousWaveEnemies;
    private int vision = 1;

    // Start is called before the first frame update
    void Start()
    {
        aliveEnemies = new List<GameObject>();
        previousWaveEnemies = new List<GameObject>();
        spawnedEnemies = new List<GameObject>();
        currentWave = levelStats.currentWave;

        if(spawnAtStart)
            StartCoroutine("SpawnNewWave");
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnAtStart)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine("SpawnNewWave");
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentWave++;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && currentWave > 0)
            {
                currentWave--;
            }
        }
    }

    void NewWave()
    {
        currentWave++;
        waveCounter.text = "Wave: " + currentWave;
        StartCoroutine("SpawnNewWave");
    }

    IEnumerator SpawnNewWave()
    {
        if(previousWaveEnemies.Count > 0)
        {
            foreach (GameObject enemy in previousWaveEnemies)
            {
                Destroy(enemy);
            }

            previousWaveEnemies.Clear();
        }

        if (spawnedEnemies.Count > 0)
        {
            foreach (GameObject enemy in spawnedEnemies)
            {
                previousWaveEnemies.Add(enemy);
            }

            spawnedEnemies.Clear();
        }

        waveEnemyKilledCount = 0;
        waveEnemySpawnedCount = Mathf.RoundToInt(baseWaveEnemyCount * (currentWave + waveEnemyCountMultiplier));
        EnemySpawManager sM = gameObject.GetComponent<EnemySpawManager>();

        UpdateEnemyCounterUI();

        Debug.Log("Enemies to spawn: " + waveEnemySpawnedCount);


        for(int i = 0; i < waveEnemySpawnedCount; i++)
        {
            int randPoint = Random.Range(0, spawnPoints.transform.childCount);

            if (currentWave - 1 < lootPools.Length)
                spawnPoints.transform.GetChild(randPoint).GetComponent<EnemySpawner>().SpawnEnemy(enemyPrefab, currentWave, lootPools[currentWave - 1], sM);
            else
                spawnPoints.transform.GetChild(randPoint).GetComponent<EnemySpawner>().SpawnEnemy(enemyPrefab, currentWave, lootPools[lootPools.Length - 1], sM);

            yield return new WaitForSeconds(0.1f);
        }

        VisionUpdate(vision);
    }

    IEnumerator UpdateAliveEnemies()
    {
        yield return new WaitForSeconds(0.2f);

        if (aliveEnemies.Count > 0)
        {
            foreach (GameObject enemy in aliveEnemies)
            {
                spawnedEnemies.Add(enemy);
            }

            //aliveEnemies.Clear();
        }
    }

    void UpdateEnemyCounterUI()
    {
        enemyCounter.text = "Enemies to kill: " + waveEnemyKilledCount + " / " + waveEnemySpawnedCount;
    }

    public void AddEnemy(GameObject enemy)
    {
        aliveEnemies.Add(enemy);

        if(aliveEnemies.Count >= waveEnemySpawnedCount)
        {
            StartCoroutine("UpdateAliveEnemies");
        }
    }

    public void KilledEnemy(GameObject enemy)
    {
        if(vision > 1)
        {
            enemy.GetComponent<Outline>().enabled = false;
        }

        aliveEnemies.Remove(enemy);

        waveEnemyKilledCount++;
        UpdateEnemyCounterUI();

        if(waveEnemyKilledCount == waveEnemySpawnedCount)
        {
            NewWave();
        }
    }

    public void VisionUpdate(int v)
    {
        Debug.Log("Vision Update: Current Vision " + v);

        vision = v;

        foreach(GameObject enemy in aliveEnemies)
        {
            if (enemy.GetComponent<Outline>())
            {
                Outline o = enemy.GetComponent<Outline>();

                if (v > 1)
                    enemy.GetComponent<EnemyHealth>().ShowHealthUI();
                else
                    enemy.GetComponent<EnemyHealth>().HideHealthUI();

                switch (v)
                {
                    case 3:
                        o.enabled = true;
                        o.OutlineMode = Outline.Mode.OutlineVisible;
                        break;
                    case 4:
                        o.enabled = true;
                        o.OutlineMode = Outline.Mode.OutlineAndSilhouette;
                        break;
                    default:
                        o.enabled = false;
                        break;
                }
            }
        }
    }
}
