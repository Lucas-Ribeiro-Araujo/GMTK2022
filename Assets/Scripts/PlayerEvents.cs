using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerEvents
{

    public event EventHandler<OnDiceThrowStartArgs> OnDiceThrowStart;
    public event EventHandler<OnDiceThrowArgs> OnDiceThrow;

    [HideInInspector]
    public Vector3 throwTarget;

    [Range(1f, 20f)]
    public float aimIntersectionPlaneDistance = 10f;
    [Range(1f, 10f)]
    public float throwOriginDistance = 5f;

    public Transform directionArrow;

    public void Update()
    {
        if (Input.GetButtonDown("Click")){
            GameManager.Instance.StartCoroutine(DiceThrow());
        }

        throwTarget = CalculateThrowDirection();
        directionArrow.position = throwTarget;

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

    Vector3 CalculateThrowDirection()
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        Plane intersectionPlane = new Plane(cameraForward * -1, Camera.main.transform.position + cameraForward * aimIntersectionPlaneDistance);

        Ray viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        viewRay.origin = Camera.main.transform.position;

        float dist;
        if (intersectionPlane.Raycast(viewRay, out dist))
        {
            return viewRay.origin + viewRay.direction * dist;
        }

        return Vector3.forward;
    }

}
