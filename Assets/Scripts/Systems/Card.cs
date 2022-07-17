using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
  public void OnTakeDamage(object sender, EventArgs e)
    {
        Debug.Log("i die: " + gameObject.name);
        gameObject.SetActive(false);
    }

}
