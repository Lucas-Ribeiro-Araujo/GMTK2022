using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<Wave> waves;
    public Transform spawnPoint;

    public void SpawnWave(Wave wave)
    {
        GameManager.Instance.StartCoroutine(SpawnEnemyCoroutine(wave));
    }

    IEnumerator SpawnEnemyCoroutine(Wave wave)
    {
        for (int i = 0; i < wave.enemies.Count; i++)
        {
            Instantiate(wave.enemies[i], new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), spawnPoint.rotation);
            yield return new WaitForSeconds(wave.intervalBeetweenSpawn);
        }
    }

}


