using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{

    [SerializeField]
    List<GameObject> dices;

    [SerializeField]
    Transform diceUnselectionOrigin;

    [SerializeField]
    [Min(0f)]
    float selectAnimationDuration = 0.5f;

    public Vector3 throwDirection;

    Dice selectedDice;

    public ThrowingState throwingState = ThrowingState.Idle;

    void Start()
    {
        GameManager.Instance.playerEventsManager.OnDiceThrowStart += ThrowStart;
        GameManager.Instance.playerEventsManager.OnDiceThrowTargetUpdate += UpdateThrowDirection;
        GameManager.Instance.playerEventsManager.OnDiceThrow += Throw;
    }

    void OnDestroy()
    {
        GameManager.Instance.playerEventsManager.OnDiceThrowStart -= ThrowStart;
        GameManager.Instance.playerEventsManager.OnDiceThrowTargetUpdate -= UpdateThrowDirection;
        GameManager.Instance.playerEventsManager.OnDiceThrow -= Throw;
    }

    void Update()
    {
        
    }

    public void SelectDice(Dice dice)
    {
        if (this.selectedDice != null)
        {
            StartCoroutine(UnselectDiceAnimation(this.selectedDice));
        }
        if (dice != null)
        {
            StartCoroutine(SelectDiceAnimation(dice));
        } else
        {
            throwingState = ThrowingState.Idle;
        }
        this.selectedDice = dice;
    }

    void UpdateThrowDirection(object sender, OnDiceThrowTargetUpdateArgs args)
    {
        throwDirection = (args.targetPosition - transform.position).normalized;
        transform.position = args.throwOrigin;
    }

    void ThrowStart(object sender, OnDiceThrowStartArgs args)
    {
        if (selectedDice == null) return;
        if (selectedDice.state == DiceState.Transitioning) return;

        throwingState = ThrowingState.Charging;

        DiceTrajectory diceTrajectory = GetComponent<DiceTrajectory>();
        if (diceTrajectory != null)
        {
            diceTrajectory.ShowTrajectory(true);
        }
    }

    void Throw(object sender, OnDiceThrowArgs args)
    {
        if (selectedDice == null) return;
        if (selectedDice.state == DiceState.Transitioning) return;

        selectedDice.state = DiceState.Thrown;

        selectedDice.EnablePhysics(true);

        selectedDice.SetVelocity(throwDirection * args.power);
        float range = Mathf.PI * 2;
        selectedDice.SetAngularVelocity(new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range)));

        DiceSpawner.instance.AddDiceToBoard(selectedDice);

        selectedDice = null;

        DiceTrajectory diceTrajectory = GetComponent<DiceTrajectory>();
        if (diceTrajectory != null)
        {
            diceTrajectory.ShowTrajectory(false);
        }

        throwingState = ThrowingState.Idle;
    }

    IEnumerator SelectDiceAnimation(Dice dice)
    {
        dice.state = DiceState.Transitioning;
        dice.EnablePhysics(false);

        float duration = selectAnimationDuration;

        Vector3 origin = dice.transform.position;
        Vector3 destiny = transform.position;

        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            dice.transform.position = Vector3.Lerp(origin, destiny, time / duration);
            yield return null;
        }
        dice.state = DiceState.Inactive;
        throwingState = ThrowingState.ReadyToThrow;
    }

    IEnumerator UnselectDiceAnimation(Dice dice)
    {
        if (diceUnselectionOrigin == null)
        {
            dice.EnablePhysics(true);
            dice.state = DiceState.Selectable;
            yield break;
        }

        dice.state = DiceState.Transitioning;
        float duration = selectAnimationDuration;

        Vector3 origin = transform.position;
        Vector3 destiny = diceUnselectionOrigin.position;

        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            dice.transform.position = Vector3.Lerp(origin, destiny, time / duration);
            yield return null;
        }
        dice.EnablePhysics(true);
        dice.state = DiceState.Selectable;
    }

}

public enum ThrowingState { Idle, ReadyToThrow, Charging }
