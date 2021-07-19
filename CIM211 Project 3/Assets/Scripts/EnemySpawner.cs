using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region lootPools
    [Header("Loot Pools")]
    public InvItem[] lootPool1;
    public InvItem[] lootPool2;
    public InvItem[] lootPool3;
    public InvItem[] lootPool4;
    public InvItem[] lootPool5;
    public InvItem[] lootPoolFinal;

    [System.Serializable]
    public struct lootPoolsTest
    {
        public InvItem[] head;
        public InvItem[] eyes;
        public InvItem[] brain;
        public InvItem[] lungs;
        public InvItem[] heart;
        public InvItem[] body;
        public InvItem[] leftArm;
        public InvItem[] rightArm;
        public InvItem[] leftLeg;
        public InvItem[] rightLeg;
    }

    public LootPool[] loootPool;

    public lootPoolsTest[] lootPoolss;
    #endregion

    private InvDeadBody enemyInv;
    private int minLootSpawn = 2;
    private int maxLootSpawn = 3;

    public void SpawnEnemy(GameObject enemyPrefab, int wave)
    {
        Debug.Log("Spawn Enemy");
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemyInv = enemy.GetComponent<InvDeadBody>();

        //switch (wave)
        //{
        //    case 1:
        //        RandomizeItems(lootPool1);
        //        minLootSpawn = 2;
        //        maxLootSpawn = 3;
        //        break;
        //    case 2:
        //        RandomizeItems(lootPool2);
        //        minLootSpawn = 3;
        //        maxLootSpawn = 4;
        //        break;
        //    case 3:
        //        RandomizeItems(lootPool3);
        //        minLootSpawn = 4;
        //        maxLootSpawn = 5;
        //        break;
        //    case 4:
        //        RandomizeItems(lootPool4);
        //        minLootSpawn = 5;
        //        maxLootSpawn = 6;
        //        break;
        //    case 5:
        //        RandomizeItems(lootPool5);
        //        minLootSpawn = 6;
        //        maxLootSpawn = 7;
        //        break;
        //    default:
        //        RandomizeItems(lootPoolFinal);
        //        minLootSpawn = lootPoolFinal.Length;
        //        maxLootSpawn = minLootSpawn;
        //        break;
        //}

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

        RandomizeItemsTest(loootPool[wave - 1], wave);
    }

    void RandomizeItems(InvItem[] lootPool)
    {
        List<InvItem> spawnedLoot = new List<InvItem>();

        while (spawnedLoot.Count < minLootSpawn)
        {
            int rand = Random.Range(0, lootPool.Length);

            if(lootPool[rand] != null)
            {
                if(spawnedLoot.Count > 0)
                {

                }
                else
                {
                    //lootPool[rand]
                }
            }
        }
    }

    void RandomizeItemsTest(LootPool lootPool, int wave)
    {
        int spawnedLoot = 0;
        int lootToSpawn = wave + Random.Range(minLootSpawn, maxLootSpawn + 1);

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
    }
}
