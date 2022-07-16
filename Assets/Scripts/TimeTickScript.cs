using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeTickScript
{
    public static event EventHandler<OnTickEventArgs> OnTick;

    private const float TICK_TIMER_MAX = 2f;
    private int tick;
    private float tickTimer;
    // Start is called before the first frame update

    public void Awake()
    {
        tick = 0;
    }


    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;
            OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });
        }
    }
}
