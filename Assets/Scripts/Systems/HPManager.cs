using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HPManager : IManager
{

    [SerializeField]
    List<Card> Cards = new List<Card>();

    public event EventHandler OnDamageTaken;

    public event EventHandler<OnDeathArgs> OnDeath;

    private int currentCard = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if(EnabledCardsCount() > 0)
        {
            OnDamageTaken += Cards[currentCard].OnTakeDamage;
            OnDamageTaken?.Invoke(this, EventArgs.Empty);
            OnDamageTaken -= Cards[currentCard].OnTakeDamage;
            currentCard++;
        }
        else
        {
            OnDeathArgs args = new OnDeathArgs();
            args.waveReached = GameManager.Instance.stateManager.waveNumber;
            args.score = GameManager.Instance.stateManager.score;
            Debug.Log("GameOVer");
            OnDeath?.Invoke(this, args);
        }
    }

    int EnabledCardsCount()
    {
        int count = 0;
        foreach (Card card in Cards)
        {
            if (card.gameObject.activeSelf) count++;
        }
        return count;
    }

    public void Start()
    {
        
    }

    public void Reset()
    {
        currentCard = 0;

        foreach (Card card in Cards)
        {
            card.gameObject.SetActive(true);
        }
    }
}
