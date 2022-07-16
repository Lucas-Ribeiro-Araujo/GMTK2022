using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject[] enemy;
    
    private void Start()
    {
        
    }
    
    public void SpawnEnemy(object sender, OnTickEventArgs e)
    {
        //Spawn enemy on desired Tick
        if (e.tick % 2 == 0)
        {
            Instantiate(enemy[0], spawnPosition.transform.position, spawnPosition.rotation);
            GameManager.Instance.timeTick.OnTick -= SpawnEnemy;
            GameManager.Instance.spawnerManager.hasSpawned = false;
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
