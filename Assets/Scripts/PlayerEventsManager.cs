using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerEventsManager : IManager
{

    public event EventHandler<OnDiceThrowStartArgs> OnDiceThrowStart;
    public event EventHandler<OnDiceThrowChargeArgs> OnDiceThrowCharge;
    public event EventHandler<OnDiceThrowTargetUpdateArgs> OnDiceThrowTargetUpdate;
    public event EventHandler<OnDiceThrowArgs> OnDiceThrow;

    [HideInInspector]
    public Vector3 throwTarget;

    [Range(1f, 20f)]
    public float aimIntersectionZPos = 10f;
    public float minTargetHeight = 0f;
    public float heightGain = 3f;
    public DiceThrower diceThrower;

    public Transform directionArrow;

    public void Start()
    {
        if (diceThrower == null)
        {
            Debug.LogError("A DiceThrower component is needed");
        }
    }

    public void Update()
    {
        if (directionArrow != null)
        {
            directionArrow.position = diceThrower.transform.position;
        }
        if (Input.GetButton("Click")){
            if (diceThrower.throwingState == ThrowingState.ReadyToThrow)
            {
                GameManager.Instance.StartCoroutine(DiceThrow());
            }
        }

        throwTarget = CalculateThrowTarget();
        if (directionArrow != null)
        {
            directionArrow.LookAt(throwTarget);
        }
        OnDiceThrowTargetUpdate?.Invoke(this, new OnDiceThrowTargetUpdateArgs
        {
            targetPosition = throwTarget,
            throwOrigin = diceThrower.transform.position
        });
    }

    IEnumerator DiceThrow() {
        OnDiceThrowStart?.Invoke(this, new OnDiceThrowStartArgs { });

        for (float heldTime = 0f; true; heldTime += Time.deltaTime)
        {
            float power = Utils.Remap(heldTime, 0.2f, 3f, 10f, 30f);
            power = Mathf.Clamp(power, 10, 30);

            OnDiceThrowCharge?.Invoke(this, new OnDiceThrowChargeArgs { power = power });

            if (!Input.GetButton("Click"))
            {
                OnDiceThrow?.Invoke(this, new OnDiceThrowArgs { power = power });
                yield break;
            }
            yield return null;
        }
    }

    Vector3 CalculateThrowTarget()
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        Plane intersectionPlane = new Plane(Vector3.back, new Vector3(0, 0, aimIntersectionZPos));

        Ray viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        viewRay.origin = Camera.main.transform.position;

        float dist;
        if (intersectionPlane.Raycast(viewRay, out dist))
        {
            Vector3 target = viewRay.origin + viewRay.direction * dist;
            target.y = Mathf.Max(minTargetHeight, target.y + heightGain);
            return target;
        }

        return Vector3.forward;
    }

    public void Reset()
    {
        
    }
}
