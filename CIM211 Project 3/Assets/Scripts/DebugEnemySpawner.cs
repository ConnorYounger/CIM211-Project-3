using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;

public class DebugEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public InvDeadBody inventory;

    private GameObject spawnedEnemy;

    public GameModeManager gameModeManager;

    public bool debugMode;
    public bool firstEnemy;

    [Header("Outline")]
    public bool outline;
    public Color outlineColour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.G))
            SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        SetEnemyStats();
    }

    void SetEnemyStats()
    {
        InvDeadBody enemyInv = spawnedEnemy.GetComponent<Enemy>().inventory;

        if(inventory.head != null)
            enemyInv.head = inventory.head;
        if (inventory.brain != null)
            enemyInv.brain = inventory.brain;
        if (inventory.eyes != null)
            enemyInv.eyes = inventory.eyes;
        if (inventory.leftArm != null)
            enemyInv.leftArm = inventory.leftArm;
        if (inventory.rightArm != null)
            enemyInv.rightArm = inventory.rightArm;
        if (inventory.heart != null)
            enemyInv.heart = inventory.heart;
        if (inventory.lungs != null)
            enemyInv.lungs = inventory.lungs;
        if (inventory.body != null)
            enemyInv.body = inventory.body;
        if (inventory.leftLeg != null)
            enemyInv.leftLeg = inventory.leftLeg;
        if (inventory.rightLeg != null)
            enemyInv.rightLeg = inventory.rightLeg;

        spawnedEnemy.GetComponent<Enemy>().SetStats();
        spawnedEnemy.GetComponent<EnemyHealth>().UpdateHealthUI();

        if (gameModeManager)
        {
            spawnedEnemy.GetComponent<EnemyHealth>().gameModeManager = gameModeManager;
            gameModeManager.firstEnemy = spawnedEnemy;
        }

        if (firstEnemy)
            spawnedEnemy.GetComponent<EnemyHealth>().firstEnemy = true;

        if (outline)
        {
            spawnedEnemy.GetComponent<Outline>().OutlineColor = outlineColour;
            spawnedEnemy.GetComponent<Outline>().enabled = true;
        }
    }
}
