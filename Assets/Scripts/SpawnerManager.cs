using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] spawner;
    public event EventHandler<OnTickEventArgs> OnTimerConclusion;

    private void Start()
    {
        
        TimeTickScript.OnTick += delegate (object sender, OnTickEventArgs e)
        {
            Debug.Log("Tick: " + e.tick);
        };
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnTimerConclusion?.Invoke(this, );
        }
    }
}
