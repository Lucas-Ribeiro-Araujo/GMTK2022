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
    public int currentDialog = 0;
    [SerializeField]
    AudioClip[] dialogClips;

    [SerializeField]
    AudioClip startClip;
    [SerializeField]
    AudioClip[] endClip;
    [SerializeField]
    AudioClip winClip;

    EventHandler playEvent;
    EventHandler resetEvent;

    public void Start()
    {
        GameManager.Instance.hpManager.OnDeath += DeadState;
        GameManager.Instance.timeTickManager.OnTick += OnTick;
        StartState();
    }

    private void OnTick(object sender, OnTickEventArgs args)
    {
        if (gameState == GameState.Playing)
        {
            if (args.tick % 5 == 0 && args.tick >= 5)
            {
                GameManager.Instance.audioManager.PlayDialogClip(dialogClips[currentDialog]);
                currentDialog++;
            }
            if (args.tick > 55)
            {
                WinState();
            }
        }
    }

    void StartState()
    {
        gameState = GameState.Start;
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
        resetEvent = GameManager.Instance.audioManager.PlayDialogClip(endClip[UnityEngine.Random.Range(0, 2)]);
        resetEvent += ResetScene;
    }

    void WinState()
    {

        gameState = GameState.Win;
        resetEvent = GameManager.Instance.audioManager.PlayDialogClip(winClip);
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
    Start, Playing, Dead, Win
}
