using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{

    [SerializeField]
    GameObject dice;

    Vector3 throwDirection;

    void Start()
    {
        GameManager.Instance.playerEvents.OnDiceThrow += SpawnDice;
        GameManager.Instance.playerEvents.OnDiceThrowTargetUpdate += UpdateThrowDirection;
        //GameManager.Instance.playerEvents.OnDiceThrowStart
    }

    void OnDestroy()
    {
        GameManager.Instance.playerEvents.OnDiceThrow -= SpawnDice;
        GameManager.Instance.playerEvents.OnDiceThrowTargetUpdate -= UpdateThrowDirection;
        //GameManager.Instance.playerEvents.OnDiceThrowStart
    }

    void Update()
    {
        
    }

    void UpdateThrowDirection(object sender, OnDiceThrowTargetUpdateArgs args)
    {
        throwDirection = (args.targetPosition - transform.position).normalized;
        transform.position = args.throwOrigin;
    }

    void SpawnDice(object sender, OnDiceThrowArgs args)
    {
        if (dice != null)
        {
            GameObject createdDice = Instantiate(dice, transform.position, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));

            Rigidbody rgdb = createdDice.GetComponent<Rigidbody>();
            if (!rgdb) return;

            rgdb.velocity = throwDirection * Mathf.Clamp(args.power, 1f, 3f) * 10f;
            float range = 3f;
            rgdb.angularVelocity = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        }
    }

}
