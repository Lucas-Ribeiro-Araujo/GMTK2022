using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    public static DiceSpawner instance;


    [SerializeField]
    List<GameObject> diceTypes;
    [SerializeField]
    int maxDiceOnBox = 8;


    private List<Dice> dicesOnBoard = new List<Dice>();
    private List<Dice> dicesOnBox = new List<Dice>();

    [Min(0f)]
    public float randomDirectionAmount;

    private void Awake()
    {
        instance = this;
    }

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
        if(dicesOnBox.Count >= maxDiceOnBox)
        {
            return;
        }

        GameObject diceType = diceTypes[Random.Range(0, diceTypes.Count)];
        GameObject dice = Instantiate(diceType, transform.position, Quaternion.identity);

        Rigidbody rb = dice.GetComponent<Rigidbody>();

        float range;

        range = randomDirectionAmount;
        rb.velocity = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));

        range = Mathf.PI * 2;
        rb.angularVelocity = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
    }

    public void AddDiceToList(Dice dice)
    {
        dicesOnBox.Add(dice);
    }
    public void AddDiceToBoard(Dice dice)
    {
        dicesOnBox.Remove(dice);
    }
}
