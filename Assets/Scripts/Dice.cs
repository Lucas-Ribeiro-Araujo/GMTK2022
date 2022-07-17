using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour, IClickable
{
    
    public event EventHandler<OnCollisionArgs> OnDiceCollision;
    public event EventHandler<OnDiceSkillArgs> OnDiceMaxRoll;

    public DiceState state = DiceState.Selectable;
    public DiceType diceType;

    public float diceStationaryDelay = 1f;
    float diceStationaryTimer = 0f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnDiceMaxRoll += MaxRoll;
        DiceSpawner.instance.AddDiceToList(this);
    }

    void OnDestroy()
    {
        OnDiceMaxRoll -= MaxRoll;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50) Destroy(gameObject);

        if (state == DiceState.Thrown)
        {
            bool isUp = Utils.DirectionEquals(Vector3.up, transform.up, .98f);

            if (rb.velocity.magnitude < .01f && rb.angularVelocity.magnitude < .01f && isUp)
            {
                diceStationaryTimer += Time.deltaTime;

                if (diceStationaryTimer >= diceStationaryDelay)
                {
                    OnDiceSkillArgs args = new OnDiceSkillArgs();
                    args.type = diceType;
                    OnDiceMaxRoll?.Invoke(this, args);
                    diceStationaryTimer = 0f;
                }
            }
            else
            {
                diceStationaryTimer = 0f;
            }
        }

    }

    void MaxRoll(object sender, OnDiceSkillArgs args)
    {
        state = DiceState.Inactive;
        Debug.Log("Max Roll");

        // Implement max roll effect/power
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

            switch (collision.collider.tag) {
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

            OnDiceCollision?.Invoke(this, args);
        }

        if (collision.collider.tag == "Unit")
        {
            float impactPower = Vector3.Dot(collision.relativeVelocity, collision.contacts[0].normal);
            Unit unit = collision.collider.GetComponent<Unit>();

            if (impactPower > 5 && unit != null)
            {
                Knockback knockback = Knockback.Empty;
                unit.TakeDamage(Mathf.Max(impactPower, 0), knockback);
            }
        }
    }

    public void DestroyDice()
    {
        Destroy(gameObject);
    }

    public void EnablePhysics(bool value)
    {
        rb.isKinematic = !value;
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void SetAngularVelocity(Vector3 angularVelocity)
    {
        rb.angularVelocity = angularVelocity;
    }

    public void OnClick()
    {
        
    }

    public void OnRelease()
    {
        if (state == DiceState.Selectable)
        {
            GameManager.Instance.playerEventsManager.diceThrower.SelectDice(this);
        }
    }

}

public enum DiceState { Selectable, Thrown, Transitioning, Inactive }
public enum DiceType { D4, D6, D20}
