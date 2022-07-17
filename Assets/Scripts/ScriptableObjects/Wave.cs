using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/Wave")]
public class Wave : ScriptableObject
{
    public List<GameObject> enemies;
    public int spawnOnTick;
    public float intervalBeetweenSpawn;
}
