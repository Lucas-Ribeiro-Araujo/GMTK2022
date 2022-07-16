using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves;
    public Transform spawnPosition;

    private void Start()
    {
        
    }


    public void SpawnWave(object sender, OnTickEventArgs e)
    {
        foreach (Wave item in waves)
        {
            if (item.spawnOnTick == e.tick)
            {
                StartCoroutine(item.SpawnEnemyCoroutine(spawnPosition));
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnTick?.Invoke(this, );
            
        }
    }

}


