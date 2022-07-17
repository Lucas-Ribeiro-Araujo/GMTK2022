using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback
{

    public static Knockback Empty
    {
        get
        {
            return new Knockback();
        }
    }

    public Vector3 origin;
    public Vector3 force;

}
