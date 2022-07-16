using UnityEngine;

[System.Serializable]
public class SpawnerManager
{
    public GameObject[] spawner;

    public void Start()
    {
        SpawnEnemyOnSpawner();
    }

    private void SpawnEnemyOnSpawner()
    {
       
        for (int i = 0; i < spawner.Length; i++)
        {
            GameManager.Instance.timeTick.OnTick += spawner[i].GetComponent<EnemySpawner>().SpawnWave;
            
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
