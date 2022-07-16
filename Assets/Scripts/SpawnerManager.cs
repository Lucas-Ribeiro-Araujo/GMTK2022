using System;
using UnityEngine;

[System.Serializable]
public class SpawnerManager
{
    public GameObject[] spawner;
    

    public void Start()
    {
        
        GameManager.Instance.timeTick.OnTick += delegate (object sender, OnTickEventArgs e)
        {
            //Debug.Log("Tick: " + e.tick);
        };
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
