using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTable : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        Debug.Log("Clicked: " + gameObject.name);
    }

    public void OnRelease()
    {
        Debug.Log("Released: " + gameObject.name);
    }
}
