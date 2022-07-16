using UnityEngine;

[System.Serializable]
public class SpawnerManager
{
    public GameObject[] spawner;
    public bool hasSpawned;
    

    public void Start()
    {
        hasSpawned = false;
    }

    private void SpawnEnemyOnSpawner()
    {
       
        for (int i = 0; i < spawner.Length; i++)
        {
            GameManager.Instance.timeTick.OnTick += spawner[i].GetComponent<EnemySpawner>().SpawnWave;
            
        }
        hasSpawned = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!hasSpawned)
        {
            SpawnEnemyOnSpawner();
        }
    }
}
