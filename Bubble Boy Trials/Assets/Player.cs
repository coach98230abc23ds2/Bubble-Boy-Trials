﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int Health = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Destroy(this);
        }
    }
}
