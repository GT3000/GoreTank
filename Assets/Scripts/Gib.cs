using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gib : MonoBehaviour
{
    protected int fuelValue;

    public int FuelValue
    {
        get => fuelValue;
        set => fuelValue = value;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //TODO update fuel count
            GameEvents.AddFuel(fuelValue);
            Destroy(gameObject);
        }
    }
}
