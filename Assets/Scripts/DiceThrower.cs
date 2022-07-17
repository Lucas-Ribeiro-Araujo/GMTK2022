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
    float selectAnimationSpeed = 0.5f;

    public Vector3 throwDirection;

    Dice selectedDice;

    public ThrowingState throwingState = ThrowingState.Idle;

    void Start()
    {
        GameManager.Instance.playerEvents.OnDiceThrowStart += ThrowStart;
        GameManager.Instance.playerEvents.OnDiceThrowTargetUpdate += UpdateThrowDirection;
        GameManager.Instance.playerEvents.OnDiceThrow += Throw;
    }

    void OnDestroy()
    {
        GameManager.Instance.playerEvents.OnDiceThrowStart -= ThrowStart;
        GameManager.Instance.playerEvents.OnDiceThrowTargetUpdate -= UpdateThrowDirection;
        GameManager.Instance.playerEvents.OnDiceThrow -= Throw;
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
        if (selectedDice.selectableState == SelectableState.Transitioning) return;

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
        if (selectedDice.selectableState == SelectableState.Transitioning) return;

        selectedDice.EnablePhysics(true);

        selectedDice.SetVelocity(throwDirection * args.power);
        float range = Mathf.PI * 2;
        selectedDice.SetAngularVelocity(new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range)));

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
        dice.selectableState = SelectableState.Transitioning;
        dice.EnablePhysics(false);

        float duration = selectAnimationSpeed;

        Vector3 origin = dice.transform.position;
        Vector3 destiny = transform.position;

        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            dice.transform.position = Vector3.Lerp(origin, destiny, time / duration);
            yield return null;
        }
        dice.selectableState = SelectableState.NotSelectable;
        throwingState = ThrowingState.ReadyToThrow;
    }

    IEnumerator UnselectDiceAnimation(Dice dice)
    {
        if (diceUnselectionOrigin == null)
        {
            dice.EnablePhysics(true);
            dice.selectableState = SelectableState.Selectable;
            yield break;
        }

        dice.selectableState = SelectableState.Transitioning;
        float duration = selectAnimationSpeed;

        Vector3 origin = transform.position;
        Vector3 destiny = diceUnselectionOrigin.position;

        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            dice.transform.position = Vector3.Lerp(origin, destiny, time / duration);
            yield return null;
        }
        dice.EnablePhysics(true);
        dice.selectableState = SelectableState.Selectable;
    }

}

public enum ThrowingState { Idle, ReadyToThrow, Charging }
