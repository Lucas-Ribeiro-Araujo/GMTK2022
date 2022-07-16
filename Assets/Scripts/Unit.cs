using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private float moveSpeed = 1;
    [SerializeField]
    private float acceleration = 1;
    [SerializeField]
    private float maxHp = 100;

    [SerializeField]
    private UnitStates state;

    private float hp;

    private Quaternion previousRotation;
    private Vector3 previousPosition;

    [Header("Options")]
    [SerializeField]
    private float slerpTime = .5f;
    [SerializeField]
    private float recoveryTimer = 1;

    [Header("References")]
    [SerializeField]
    private NavMeshAgent unitNavAgent;

    [SerializeField]
    private Animator unitAnimator;

    [SerializeField]
    private Rigidbody unitRigidbody;

    [SerializeField]
    private Transform unitMeshTransform;
    [SerializeField]
    private Collider unitCollider;

    [SerializeField]
    private float ragdolTimer;
    private Vector3 recoveryPoint;

    private void Start()
    {
        UnitRagdollState();

        hp = maxHp;
        unitNavAgent.speed = moveSpeed;
        unitNavAgent.acceleration = acceleration;

    }

    private void Update()
    {
        switch (state)
        {
            case UnitStates.Ragdolling:
                {
                    if (unitRigidbody.velocity.magnitude < .01f && unitRigidbody.angularVelocity.magnitude < .01f )
                    {
                    ragdolTimer += Time.deltaTime;
                        if(ragdolTimer >= recoveryTimer)
                        {
                        ragdolTimer = 0;
                        UnitGettingUpState();
                        }
                    }
                    break;
                }
            case UnitStates.GettingUp:
                {
                    unitMeshTransform.localPosition = Vector3.Slerp(unitMeshTransform.localPosition,
                        new Vector3(unitMeshTransform.localPosition.x, recoveryPoint.y + .2f, unitMeshTransform.localPosition.z), slerpTime);
                    unitMeshTransform.localRotation = Quaternion.Slerp(unitMeshTransform.localRotation, Quaternion.identity, slerpTime);


                    if (RotationEquals(unitMeshTransform.localRotation, Quaternion.identity))
                        UnitMovingState();

                    break;
                }
            case UnitStates.Moving:
                {
                    break;
                }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnitRagdollState();
        }
    }

    bool RotationEquals(Quaternion r1, Quaternion r2)
    {
        float abs = Mathf.Abs(Quaternion.Dot(r1, r2));
        if (abs >= 0.9999f)
            return true;
        return false;
    }


    private void UnitRagdollState()
    {
        state = UnitStates.Ragdolling;
        unitRigidbody.isKinematic = false;
        unitNavAgent.enabled = false;
    }

    private void UnitGettingUpState()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        recoveryPoint = hit.point + unitCollider.bounds.extents;

        state = UnitStates.GettingUp;
        unitNavAgent.enabled = false;
        unitRigidbody.isKinematic = true;
    }

    private void UnitMovingState()
    {
        state = UnitStates.Moving;
        unitNavAgent.enabled = true;
        unitRigidbody.isKinematic = false;
        unitNavAgent.SetDestination(UnitTargetSingleton.Instance.transform.position);
        unitAnimator.SetBool("Moving", true);
    }
}



public enum UnitStates
{
    Moving,
    GettingUp,
    Ragdolling,
    Attacking,
    Dead
}

