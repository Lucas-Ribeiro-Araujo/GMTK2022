using UnityEngine;

[System.Serializable]
public class SpawnerManager
{
    public GameObject[] spawner;
    int maxSpawnerQuantity;
    public bool hasSpawned;
    

    public void Start()
    {
        maxSpawnerQuantity = spawner.Length;
        hasSpawned = false;
    }

    private void SpawnEnemyOnSelectedSpawner()
    {
        if (spawner.Length != 0)
        {
            int spawnerRange = Random.Range(0, maxSpawnerQuantity);
            GameManager.Instance.timeTick.OnTick += spawner[spawnerRange].GetComponent<EnemySpawner>().SpawnEnemy;
            hasSpawned = true;
        }

    }

    // Update is called once per frame
    public void Update()
    {
        if (!hasSpawned)
        {
            SpawnEnemyOnSelectedSpawner();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnTick?.Invoke(this, );
        }
    }
}
