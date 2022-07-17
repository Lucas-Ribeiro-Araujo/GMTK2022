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
    private float ragdollRecoveryDelay = 1;
    [SerializeField]
    private float ragdollToleranceDelay = 0.2f;

    private float ragdollRecoveryTimer = 0f;
    private float ragdollToleranceTimer = 0f;

    [SerializeField]
    private float destroyDelay = 1;
    [SerializeField]
    private float damageBlinkDecrease = 0.85f;
    private float damageBlinkRatio = 1;

    bool isDamageBlinking = false;


    [SerializeField]
    private float TimeBetweemJumps;

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

    private MeshRenderer meshRenderer;
    private Material unitMaterial;

    public event EventHandler<EventArgs> OnWalk;
    public event EventHandler<OnCollisionArgs> OnUnitCollision;

    Coroutine ragdollRecoverCoroutine;


    private void Start()
    {
        UnitRagdollState();

        hp = maxHp;

        unitNavAgent.updatePosition = false;
        unitNavAgent.updateRotation = false;
        unitNavAgent.updateUpAxis = false;


        meshRenderer = GetComponent<MeshRenderer>();
        unitMaterial = meshRenderer.material;
        unitMaterial.SetFloat("_WhitePiece", UnityEngine.Random.Range(0, 10) > 5f ? 1 : 0);
    }


    private void FixedUpdate()
    {
        bool isStanding = Utils.DirectionEquals(Vector3.up, transform.up, .97f);

        if (isDamageBlinking)
        {
            damageBlinkRatio = damageBlinkRatio * damageBlinkDecrease;

            if (damageBlinkRatio < 0.01f)
            {
                isDamageBlinking = false;
                damageBlinkRatio = 0;
            }

            unitMaterial.SetFloat("_Damage", damageBlinkRatio);
        }

        switch (state)
        {
            case UnitStates.Ragdolling:
                if (unitRigidbody.velocity.magnitude < .01f && unitRigidbody.angularVelocity.magnitude < .01f)
                {
                    ragdollRecoveryTimer += Time.fixedDeltaTime;
                    if (ragdollRecoveryTimer >= ragdollRecoveryDelay)
                    {
                        ragdollRecoveryTimer = 0;
                        if (isStanding)
                        {
                            UnitMovingState();
                        }
                        else
                        {
                            ragdollRecoverCoroutine = StartCoroutine(RagdollRecover());
                        }
                    }
                }
                else
                {
                    ragdollRecoveryTimer = 0;
                }
                break;

            case UnitStates.Moving:
                if (!isStanding)
                {
                    ragdollToleranceTimer += Time.fixedDeltaTime;
                    if (ragdollToleranceTimer > ragdollToleranceDelay)
                    {
                        ragdollToleranceTimer = 0f;
                        UnitRagdollState();
                        break;
                    }
                }
                else
                {
                    ragdollToleranceTimer = 0;
                }

                if (unitNavAgent.path != null)
                {
                    unitNavAgent.nextPosition = unitRigidbody.position;

                    jumpTimer += Time.fixedDeltaTime;
                    if (jumpTimer > TimeBetweemJumps && isStanding)
                    {
                        jumpTimer = 0f;

                        unitRigidbody.AddForce(Vector3.up * jumpForce);
                        unitRigidbody.AddForce(unitNavAgent.desiredVelocity.normalized * forceTowardsDestination);
                    }
                }
                break;

            case UnitStates.Attacking:
                UnitDeadState();
                break;

            default:
                break;
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
        jumpTimer = 0f;
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

    public void TakeDamage(float damageToTake, Knockback knockback = null)
    {
        if (knockback == null)
        {
            knockback = Knockback.Empty;
        }

        hp -= damageToTake;

        damageBlinkRatio = 1;
        unitMaterial.SetFloat("_Damage", 1);
        isDamageBlinking = true;

        if (ragdollRecoverCoroutine != null)
        {
            StopCoroutine(ragdollRecoverCoroutine);
            ragdollRecoverCoroutine = null;
        }

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

    private IEnumerator RagdollRecover()
    {
        state = UnitStates.Recovering;
        unitRigidbody.useGravity = true;

        float duration = 2f;

        Vector3 originUp = transform.up;
        Vector3 targetUp = Vector3.up;

        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            transform.up = Vector3.Slerp(originUp, targetUp, time / duration);
            yield return null;
        }

        unitRigidbody.useGravity = true;
        unitRigidbody.velocity = Vector3.zero;
        unitRigidbody.angularVelocity = Vector3.zero;
        UnitMovingState();
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
    Recovering,
    Attacking,
    Dead
}

