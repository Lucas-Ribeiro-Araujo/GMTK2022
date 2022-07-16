using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public static HPManager Instance;

    [SerializeField]
    List<Card> Cards = new List<Card>();

    public event EventHandler damageEvent;

    public event EventHandler deathEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void TakeDamage()
    {
        if(Cards.Count > 0)
        {
        damageEvent += Cards[Cards.Count - 1].OnTakeDamage;
        damageEvent?.Invoke(this, EventArgs.Empty);
        damageEvent -= Cards[Cards.Count - 1].OnTakeDamage;
        Cards.Remove(Cards[Cards.Count - 1]);
        }
        else
        {
            deathEvent?.Invoke(this, EventArgs.Empty);
            Debug.Log("I is die!");
        }
    }
}
