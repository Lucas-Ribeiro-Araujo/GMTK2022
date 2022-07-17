using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnerManager : IManager
{
    public List<WaveSpawner> spawners;

    public EventHandler OnWaveStart;

    public void Start()
    {
        GameManager.Instance.timeTickManager.OnTick += SpawnWaves;
    }

    public void SpawnWaves(object sender, OnTickEventArgs args)
    {

        bool isWaveSpawn = false;

        foreach (WaveSpawner spawner in spawners)
        {
            foreach (Wave wave in spawner.waves)
            {
                if (wave.spawnOnTick == args.tick)
                {
                    spawner.SpawnWave(wave);
                    isWaveSpawn = true;
                }
            }
        }

        if (isWaveSpawn) OnWaveStart?.Invoke(this, EventArgs.Empty);

    }

    public void Update()
    {
        
    }

    public void Reset()
    {
        
    }
}
