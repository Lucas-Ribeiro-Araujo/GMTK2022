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
    }
}
