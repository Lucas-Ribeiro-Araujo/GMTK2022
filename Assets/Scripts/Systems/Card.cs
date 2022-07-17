using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    bool destroyed;

    [SerializeField]
    float dissolveRate =  1;
    float dissolveValue = 0.2f;

    [SerializeField]
    CardSound soundManager;

    MeshRenderer meshRenderer;
    Material material;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }

    private void Update()
    {
        if (destroyed)
        {
            dissolveValue += dissolveRate * Time.deltaTime;
            material.SetFloat("_Death", dissolveValue);
            if (dissolveValue > 1)
            {
                dissolveValue = 0.2f;
                destroyed = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTakeDamage(object sender, EventArgs e)
    {
        destroyed = true;
        soundManager.EmitSounds();
    }

}
