using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Sats", menuName = "Manager/LevelStats")]
public class LevelStats : ScriptableObject
{
    public int currentWave;
    public int spawnedEnemies;
    public int killedEnemies;
}
