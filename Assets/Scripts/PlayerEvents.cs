using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{

    public event EventHandler<OnDiceThrowStartArgs> OnDiceThrowStart;
    public event EventHandler<OnDiceThrowArgs> OnDiceThrow;

    Vector3 throwDirection;

    public void Update()
    {
        if (Input.GetButtonDown("Click")){
            GameManager.Instance.StartCoroutine(DiceThrow());
        }

    }

    IEnumerator DiceThrow()
    {
        float heldTime = 0f;
        OnDiceThrowStart?.Invoke(this, new OnDiceThrowStartArgs { });

        for (float time = 0f; true; time += Time.deltaTime)
        {

            heldTime = time;

            if (!Input.GetButton("Click"))
            {
                OnDiceThrow?.Invoke(this, new OnDiceThrowArgs { power = heldTime });
                yield break;
            }
            yield return null;
        }
    }

    //Vector3 CalculateThrowDirection()
    //{
    //    Plane intersectionPlane = new Plane(Vector3.back, 0);
    //    intersectionPlane.Raycast()
    //}

}
