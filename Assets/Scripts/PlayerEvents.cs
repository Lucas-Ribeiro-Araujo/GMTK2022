using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerEvents
{

    public event EventHandler<OnDiceThrowStartArgs> OnDiceThrowStart;
    public event EventHandler<OnDiceThrowArgs> OnDiceThrow;
    public event EventHandler<OnDiceThrowTargetUpdateArgs> OnDiceThrowTargetUpdate;

    [HideInInspector]
    public Vector3 throwTarget;

    [Range(1f, 20f)]
    public float aimIntersectionZPos = 10f;
    public float heightGain = 3f;
    public Vector3 throwOrigin = Vector3.zero;

    public Transform directionArrow;

    public void Start()
    {
        if (directionArrow != null)
        {
            directionArrow.position = throwOrigin;
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Click")){
            GameManager.Instance.StartCoroutine(DiceThrow());
        }

        throwTarget = CalculateThrowTarget();
        //directionArrow.LookAt(throwTarget);
        OnDiceThrowTargetUpdate?.Invoke(this, new OnDiceThrowTargetUpdateArgs
        {
            targetPosition = throwTarget,
            throwOrigin = throwOrigin
        });

    }

    IEnumerator DiceThrow() {
        OnDiceThrowStart?.Invoke(this, new OnDiceThrowStartArgs { });

        for (float heldTime = 0f; true; heldTime += Time.deltaTime)
        {
            if (!Input.GetButton("Click"))
            {
                float power = Utils.Remap(heldTime, 0.2f, 3f, 10f, 30f);
                power = Mathf.Clamp(power, 10, 30);

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
            target.y = Mathf.Max(0, target.y + heightGain);
            return target;
        }

        return Vector3.forward;
    }

}
