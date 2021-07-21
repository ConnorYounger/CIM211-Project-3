using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private InvDeadBody enemyInv;
    private int minLootSpawn = 0;
    private int maxLootSpawn = 2;
    private int currentWave;
    private LootPool lootPool;

    private int spawnedLoot;
    private int lootToSpawn;

    private EnemySpawManager enemySpawManager;

    public void SpawnEnemy(GameObject enemyPrefab, int wave, LootPool lp, EnemySpawManager spawnManager)
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemyInv = enemy.GetComponent<InvDeadBody>();
        enemy.GetComponent<EnemyHealth>().spawManager = spawnManager;

        //switch (wave)
        //{
        //    case 1:
        //        minLootSpawn = 2;
        //        maxLootSpawn = 3;
        //        break;
        //    case 2:
        //        minLootSpawn = 3;
        //        maxLootSpawn = 4;
        //        break;
        //    case 3:
        //        minLootSpawn = 4;
        //        maxLootSpawn = 5;
        //        break;
        //    case 4:
        //        minLootSpawn = 5;
        //        maxLootSpawn = 6;
        //        break;
        //    case 5:
        //        minLootSpawn = 6;
        //        maxLootSpawn = 7;
        //        break;
        //    default:
        //        minLootSpawn = lootPoolFinal.Length;
        //        maxLootSpawn = minLootSpawn;
        //        break;
        //}

        //RandomizeItemsTest(loootPool[wave - 1], wave);
        currentWave = wave;
        lootPool = lp;
        spawnedLoot = 0;
        lootToSpawn = currentWave + Random.Range(minLootSpawn, maxLootSpawn + 1);

        StartCoroutine("RandomizeItemsTest");

        if(enemySpawManager == null)
        {
            enemySpawManager = spawnManager;
        }
        
        if(enemySpawManager != null)
        {
            enemySpawManager.AddEnemy(enemy);
        }
    }

    IEnumerator RandomizeItemsTest()
    {
        int rand = Random.Range(0, 10);

        switch (rand)
        {
            case 0:
                if (enemyInv.head == null && lootPool.head.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.head.Length);

                    enemyInv.head = lootPool.head[newRand];

                    spawnedLoot++;
                }
                break;
            case 1:
                if (enemyInv.eyes == null && lootPool.eyes.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.eyes.Length);

                    enemyInv.eyes = lootPool.eyes[newRand];

                    spawnedLoot++;
                }
                break;
            case 2:
                if (enemyInv.brain == null && lootPool.brain.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.brain.Length);

                    enemyInv.brain = lootPool.brain[newRand];

                    spawnedLoot++;
                }
                break;
            case 3:
                if (enemyInv.lungs == null && lootPool.lungs.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.lungs.Length);

                    enemyInv.lungs = lootPool.lungs[newRand];

                    spawnedLoot++;
                }
                break;
            case 4:
                if (enemyInv.heart == null && lootPool.heart.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.heart.Length);

                    enemyInv.heart = lootPool.heart[newRand];

                    spawnedLoot++;
                }
                break;
            case 5:
                if (enemyInv.body == null && lootPool.body.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.body.Length);

                    enemyInv.body = lootPool.body[newRand];

                    spawnedLoot++;
                }
                break;
            case 6:
                if (enemyInv.leftArm == null && lootPool.leftArm.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.leftArm.Length);

                    enemyInv.leftArm = lootPool.leftArm[newRand];

                    spawnedLoot++;
                }
                break;
            case 7:
                if (enemyInv.rightArm == null && lootPool.rightArm.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.rightArm.Length);

                    enemyInv.rightArm = lootPool.rightArm[newRand];

                    spawnedLoot++;
                }
                break;
            case 8:
                if (enemyInv.leftLeg == null && lootPool.leftLeg.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.leftLeg.Length);

                    enemyInv.leftLeg = lootPool.leftLeg[newRand];

                    spawnedLoot++;
                }
                break;
            case 9:
                if (enemyInv.rightLeg == null && lootPool.rightLeg.Length > 0)
                {
                    int newRand = Random.Range(0, lootPool.rightLeg.Length);

                    enemyInv.rightLeg = lootPool.rightLeg[newRand];

                    spawnedLoot++;
                }
                break;
        }

        yield return new WaitForSeconds(0);

        if (spawnedLoot < lootToSpawn)
            StartCoroutine("RandomizeItemsTest");
    }
}
