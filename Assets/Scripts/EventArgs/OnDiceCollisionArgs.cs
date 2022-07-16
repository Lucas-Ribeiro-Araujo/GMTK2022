using System;
using UnityEngine;

public class OnDiceCollisionArgs : EventArgs
{
    public Vector3 collisionVelocity;
    public Vector3 collisionNormal;
    public Vector3 contactPosition;
    public float impactPower;
    public CollisionType collisionType;
}

public enum CollisionType { Table, Wood, Metal, Generic }
