using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{

    public event EventHandler<OnDiceCollisionArgs> OnDiceCollision;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        List<ContactPoint> contacts = new List<ContactPoint>(collision.contacts);
        foreach (ContactPoint contact in contacts)
        {
            OnDiceCollisionArgs args = new OnDiceCollisionArgs();

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
            Unit unit = collision.collider.GetComponentInParent<Unit>();

            if (impactPower > 5)
            {
                Knockback knockback = new Knockback();
                knockback.origin = collision.contacts[0].point;
                knockback.force = collision.contacts[0].normal * -impactPower * 2f;
                Debug.Log(impactPower);
                unit.TakeDamage(Mathf.Max(impactPower, 0), knockback);
            }
        }
    }
}
