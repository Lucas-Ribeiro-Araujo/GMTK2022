using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/Wave")]
public class Wave : ScriptableObject
{
    public GameObject[] Enemy;
    public int spawnOnTick;
    public float intervalBeetweenSpawn;

   
    public IEnumerator SpawnEnemyCoroutine(Transform spawnPosition)
    {
       for (int i = 0; i < Enemy.Length; i++)
        {
            Instantiate(Enemy[i], new Vector3(spawnPosition.position.x, spawnPosition.position.y, spawnPosition.position.z), spawnPosition.rotation);
            yield return new WaitForSeconds(intervalBeetweenSpawn);
        }

        GameManager.Instance.spawnerManager.hasSpawned = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
