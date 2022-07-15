using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    PlayerEvents playerEvents;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        playerEvents = new PlayerEvents();    }

    void Start()
    {
        
    }
    void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerEvents.Update();
    }
}
