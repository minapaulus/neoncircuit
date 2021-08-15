using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : MonoBehaviour
{

    public List<GameObject> enemyTypes = new List<GameObject>();
    public int spawnCount;
    private System.Random random;
    public int spawnSeed;

    void Start()
    {
        spawnSeed = Environment.TickCount;
        random = new System.Random(spawnSeed);

        for(int i = 0; i < spawnCount; i++)
        {
            int randIndex = random.Next(enemyTypes.Count);
            GameObject enemy = Instantiate(enemyTypes[randIndex], transform.position, Quaternion.identity);
            var name = enemy.name;
            int randColor = (int) UnityEngine.Random.Range(0, 2);
            enemy.GetComponentInChildren<Enemy>().ChangeAssignedColor((Enemy.AssignedColors)randColor);
        }
    }
}
