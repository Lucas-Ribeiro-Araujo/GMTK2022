using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    Transform spawnPosition;
    public GameObject[] enemy;

    private void Start()
    {
        GameManager.Instance.GetComponent<SpawnerManager>().OnTick += SpawnEnemy;
        spawnPosition = GetComponentInChildren<Transform>().transform;
    }
    
    private void SpawnEnemy(object sender, OnTickEventArgs e)
    {
        //Spawn enemy on desired Tick
        //if (e.tick == 2)
            Instantiate(enemy[0], spawnPosition);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnTick?.Invoke(this, );
            
        }
    }

}
