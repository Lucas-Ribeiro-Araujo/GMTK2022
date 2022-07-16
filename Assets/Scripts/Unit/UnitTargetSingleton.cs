using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTargetSingleton : MonoBehaviour
{
    public static UnitTargetSingleton Instance;

    private void Awake()
    {
        Instance = this;
    }
}
