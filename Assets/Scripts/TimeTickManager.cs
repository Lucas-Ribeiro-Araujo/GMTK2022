using System;
using UnityEngine;

[System.Serializable]
public class TimeTickManager : IManager
{
    public event EventHandler<OnTickEventArgs> OnTick;

    public float tickInterval = 2f;
    public int tick = 0;

    float timer = 0f;

    public void Start()
    {
        
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tickInterval)
        {
            timer -= tickInterval;
            tick++;
            OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });
        }
    }

    public void Reset()
    {
        tick = 0;
        timer = 0f;
    }
}
