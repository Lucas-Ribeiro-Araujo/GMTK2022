using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{

    public static UnitTarget UnitTarget;

    [Header("Stats")]
    [SerializeField]
    private UnitStates state;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float maxHp = 100;


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
    private Collider unitCollider;

    public event EventHandler<EventArgs> OnWalk;
    public event EventHandler<OnCollisionArgs> OnUnitCollision;


    private void Start()
    {
        UnitRagdollState();

        hp = maxHp;

        unitNavAgent.updatePosition = false;
        unitNavAgent.updateRotation = false;
        unitNavAgent.updateUpAxis = false;

    }


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
                    if (unitNavAgent.path != null)
                    {
                        unitNavAgent.nextPosition = unitRigidbody.position;

                        jumpTimer += Time.fixedDeltaTime;
                        if (jumpTimer > TimeBetweemJumps)
                        {
                            jumpTimer -= TimeBetweemJumps;

                            unitRigidbody.AddForce(Vector3.up * jumpForce);
                            unitRigidbody.AddForce(unitNavAgent.desiredVelocity.normalized * forceTowardsDestination);
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

    private void OnCollisionEnter(Collision collision)
    {
        List<ContactPoint> contacts = new List<ContactPoint>(collision.contacts);
        foreach (ContactPoint contact in contacts)
        {
            OnCollisionArgs args = new OnCollisionArgs();

            args.collisionVelocity = collision.relativeVelocity;
            args.collisionNormal = contact.normal;
            args.contactPosition = contact.point;

            args.impactPower = Vector3.Dot(args.collisionVelocity.normalized, args.collisionNormal);

            switch (collision.collider.tag)
            {
                case "Table":
                    args.collisionType = CollisionType.Table;
                    break;
                case "Wood":
                    args.collisionType = CollisionType.Wood;
                    break;
                case "Metal":
                    args.collisionType = CollisionType.Metal;
                    break;
                default:
                    args.collisionType = CollisionType.Generic;
                    break;
            }

            OnUnitCollision?.Invoke(this, args);
        }
    }

    private void UnitRagdollState()
    {
        state = UnitStates.Ragdolling;
    }

    private void UnitMovingState()
    {
        state = UnitStates.Moving;
        unitNavAgent.SetDestination(UnitTarget.transform.position);
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

        GameManager.Instance.hpManager.TakeDamage();
        Destroy(this.gameObject);
        yield break;
    }

    public void EmitWalkSound()
    {
        this.OnWalk?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        if (unitNavAgent.path != null)
        {
            for (int i = 0; i < unitNavAgent.path.corners.Length; i++)
            {
                Gizmos.color = Color.blue;
                if (i > 0)
                    Gizmos.DrawLine(unitNavAgent.path.corners[i - 1], unitNavAgent.path.corners[i]);
            }

            Gizmos.DrawRay(unitNavAgent.nextPosition, unitNavAgent.desiredVelocity);
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

