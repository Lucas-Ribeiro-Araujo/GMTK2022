using System;
using UnityEngine;

[System.Serializable]
public class TimeTick
{
    public event EventHandler<OnTickEventArgs> OnTick;

    public float TICK_TIMER_MAX = 2f;
    public int tick;
    public float tickTimer;
    // Start is called before the first frame update

    public void Awake()
    {
        tick = 0;
    }


    public void Start()
    {
        /*GameManager.Instance.timeTick.OnTick += delegate (object sender, OnTickEventArgs e)
        {
            Debug.Log("Tick: " + e.tick);
        };*/
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
