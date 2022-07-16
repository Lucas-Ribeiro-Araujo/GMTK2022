using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject[] enemy;

    private void Start()
    {
        GameManager.Instance.timeTick.OnTick += SpawnEnemy;
    }
    
    private void SpawnEnemy(object sender, OnTickEventArgs e)
    {
        //Spawn enemy on desired Tick
        if (e.tick % 2 == 0)
            Instantiate(enemy[0], spawnPosition.transform.position, spawnPosition.rotation);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnTick?.Invoke(this, );
            
        }
    }

}
