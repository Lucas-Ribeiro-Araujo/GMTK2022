using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerManager
{
    public GameObject[] spawner;
    public event EventHandler<OnTickEventArgs> OnTick;

    public void Start()
    {
        
        TimeTickScript.OnTick += delegate (object sender, OnTickEventArgs e)
        {
            Debug.Log("Tick: " + e.tick);
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
