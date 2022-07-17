using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateManager : IManager
{

    public GameState gameState;

    public int waveNumber = 0;
    public int score = 0;

    AudioClip startClip;
    AudioClip endClip;

    EventHandler playEvent;
    EventHandler resetEvent;

    public void Start()
    {
        GameManager.Instance.hpManager.OnDeath += DeadState;
        gameState = GameState.Start;
        StartState();
    }

    void StartState()
    {
        playEvent = GameManager.Instance.audioManager.PlayDialogClip(startClip);
        playEvent += PlayingState;
    }
    
    void PlayingState(object sender, EventArgs args)
    {
        gameState = GameState.Playing;
        playEvent -= PlayingState;
    }

    void DeadState(object sender, OnDeathArgs args)
    {
        gameState = GameState.Dead;
        resetEvent = GameManager.Instance.audioManager.PlayDialogClip(endClip);
        resetEvent += ResetScene;
    }

    void ResetScene(object sender, EventArgs args)
    {
        resetEvent -= ResetScene;
        GameManager.Instance.ResetScene();
    }

    void UpdateWaveNumber(object sender, EventArgs args)
    {
        waveNumber++;
    }

    void UpdateScore(object sender, OnScoreAddArgs args)
    {
        score += args.scoreAmount;
    }

    public void Update()
    {
        
    }

    public void Reset()
    {
        gameState = GameState.Start;
        waveNumber = 0;
        score = 0;
    }
}

public enum GameState
{
    Start, Playing, Dead
}
