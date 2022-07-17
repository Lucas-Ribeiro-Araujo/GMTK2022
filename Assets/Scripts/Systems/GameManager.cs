using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public PlayerEventsManager playerEventsManager;
    public TimeTickManager timeTickManager;
    public SpawnerManager spawnerManager;
    public HPManager hpManager;
    public StateManager stateManager;
    public AudioManager audioManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        Cursor.lockState = CursorLockMode.Confined;
        playerEventsManager.Start();
        spawnerManager.Start();
        stateManager.Start();
    }

    void OnDestroy()
    {
        
    }

    void Update()
    {
        playerEventsManager.Update();
        timeTickManager.Update();
        hpManager.Update();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        playerEventsManager.Reset();
        timeTickManager.Reset();
        spawnerManager.Reset();
        hpManager.Reset();
        stateManager.Reset();
        audioManager.Reset();
    }
}
