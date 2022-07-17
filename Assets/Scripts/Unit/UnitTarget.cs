using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTarget : MonoBehaviour
{

    private void Awake()
    {
        Unit.UnitTarget = this;
    }
}
