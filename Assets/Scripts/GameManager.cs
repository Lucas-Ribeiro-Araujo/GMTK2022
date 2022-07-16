using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    public PlayerEvents playerEvents;
    [SerializeField]
    public TimeTickScript timeTick;
    [SerializeField]
    public SpawnerManager spawnerManager;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        //timeTick.Awake();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        spawnerManager.Start();
    }
    void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerEvents.Update();
        timeTick.Update();
        spawnerManager.Update();
    }
}
