using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{

    [SerializeField]
    List<GameObject> diceTypes;

    [Min(0f)]
    public float randomDirectionAmount;

    void Start()
    {
        GameManager.Instance.timeTickManager.OnTick += SpawnDice;
    }

    void OnDestroy()
    {
        GameManager.Instance.timeTickManager.OnTick -= SpawnDice;
    }

    void SpawnDice(object sender, OnTickEventArgs args)
    {
        GameObject diceType = diceTypes[Random.Range(0, diceTypes.Count)];
        GameObject dice = Instantiate(diceType, transform.position, Quaternion.identity);

        Rigidbody rb = dice.GetComponent<Rigidbody>();

        float range;

        range = randomDirectionAmount;
        rb.velocity = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));

        range = Mathf.PI * 2;
        rb.angularVelocity = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
    }
}
