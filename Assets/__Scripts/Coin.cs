﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour, IChangeScore
{

    
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if ( ! collider.isTrigger)
        {
            Debug.LogError("Forcing Collider to be a trigger", this);
        }
    }

    public void ChangeScore(PigController pig)
    {
        pig.ChangeCoinCount( 1 );
        Destroy(gameObject);
    }
}
