using UnityEngine;

[System.Serializable]
public class SpawnerManager
{
    public GameObject[] spawner;
    

    public void Start()
    {
        
      
    }

    // Update is called once per frame
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnTick?.Invoke(this, );
        }
    }
}
