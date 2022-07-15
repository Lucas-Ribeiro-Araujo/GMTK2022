using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    SpawnerManager spawnerManager;

    private void Start()
    {
        SpawnerManager spawnerManager = GetComponent<SpawnerManager>();
        spawnerManager.OnTimerConclusion += SpawnEnemy;
    }

    private void OnDisable()
    {
        spawnerManager.OnTimerConclusion -= SpawnEnemy;
    }

    private void SpawnEnemy(object sender, EventArgs e)
    {
        Debug.Log("Space");
    }

}
