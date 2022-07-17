using System;
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

    [SerializeField]
    private float hp;

    private Quaternion previousRotation;
    private Vector3 previousPosition;

    [Header("Options")]
    [SerializeField]
    private float recoveryTimer = 1;
    [SerializeField]
    private float ragdolTimer;
    [SerializeField]
    private float destroyDelay = 1;

    [SerializeField]
    private float TimeBetweemJumps;
    private float TimeBetweemJumpsCounter = 2;
    [SerializeField]
    private float jumpTimer;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float forceTowardsDestination;


    [Header("References")]
    [SerializeField]
    private NavMeshAgent unitNavAgent;

    [SerializeField]
    private Rigidbody unitRigidbody;

    [SerializeField]
    private Transform unitMeshTransform;
    [SerializeField]
    private Collider unitCollider;


    private Vector3 recoveryPoint;



    private void Start()
    {
        UnitRagdollState();

        hp = maxHp;
        currentPath = new NavMeshPath();

        unitNavAgent.updatePosition = false;
        unitNavAgent.updateRotation = false;
        unitNavAgent.updateUpAxis = false;

    }

    public EventHandler<EventArgs> OnWalk;
    NavMeshPath currentPath;
    int currentPathIndex = 1;

    private void FixedUpdate()
    {
        switch (state)
        {
            case UnitStates.Ragdolling:
                {
                    if (unitRigidbody.velocity.magnitude < .01f && unitRigidbody.angularVelocity.magnitude < .01f)
                    {
                        ragdolTimer += Time.fixedDeltaTime;
                        if (ragdolTimer >= recoveryTimer)
                        {
                            ragdolTimer = 0;
                            UnitMovingState();
                        }
                    }
                    break;
                }
            case UnitStates.Moving:
                {
                    if (currentPath != null)
                    {
                        jumpTimer += Time.fixedDeltaTime;
                        if (jumpTimer > TimeBetweemJumpsCounter)
                        {
                            TimeBetweemJumpsCounter += TimeBetweemJumps;


                            //unitNavAgent.nextPosition = currentPath.corners[currentPathIndex];
                            if (Vector3.Distance(transform.position, unitNavAgent.nextPosition) < 2 && )
                            {

                                currentPathIndex++;
                                unitNavAgent.nextPosition = currentPath.corners[currentPathIndex];
                            }

                            unitRigidbody.AddForce(Vector3.up * jumpForce);
                            unitRigidbody.AddForce((currentPath.corners[currentPathIndex] - transform.position).normalized * forceTowardsDestination);
                        }
                    }
                    break;
                }
            case UnitStates.Attacking:
                {
                    UnitDeadState();
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
        unitNavAgent.CalculatePath(UnitTargetSingleton.Instance.transform.position, currentPath);
        state = UnitStates.Ragdolling;
    }

    private void UnitMovingState()
    {
        state = UnitStates.Moving;
        unitNavAgent.CalculatePath(UnitTargetSingleton.Instance.transform.position, currentPath);
        unitNavAgent.nextPosition = currentPath.corners[1];
    }

    private void UnitAttackingState()
    {
        state = UnitStates.Attacking;
    }

    private void UnitDeadState()
    {
        state = UnitStates.Dead;
        StartCoroutine(DieAfterTime(destroyDelay));
    }

    public void TakeDamage(float damageToTake, Knockback knockback)
    {
        hp -= damageToTake;

        if (hp <= 0)
        {
            UnitDeadState();
        }
        else
        {
            UnitRagdollState();
            unitRigidbody.angularVelocity = Vector3.zero;
            unitRigidbody.velocity = Vector3.zero;
            unitRigidbody.AddForceAtPosition(knockback.force, knockback.origin);
        }
    }

    private IEnumerator DieAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        HPManager.Instance.TakeDamage();
        Destroy(this.gameObject);
        yield break;
    }

    public void EmitWalkSound()
    {
        this.OnWalk?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        if (currentPath != null)
        {

            for (int i = 0; i < currentPath.corners.Length; i++)
            {
                Gizmos.color = Color.blue;
                if (i > 0)
                    Gizmos.DrawLine(currentPath.corners[i - 1], currentPath.corners[i]);
                if (currentPathIndex == i)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(currentPath.corners[currentPathIndex], 1f);
                }
               
            }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, unitNavAgent.nextPosition);
        }
    }
}



public enum UnitStates
{
    Moving,
    Ragdolling,
    Attacking,
    Dead
}

