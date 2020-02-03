using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TestWeapon : WeaponController
{
    // Start is called before the first frame update
    public GameObject EnemyTarget;
    public void IdentifySingleEnemy(Collider2D col, Collider2D othercol) {
        EnemyTarget = Utils.IdentifyCollidingUnitNearestToPathEndWithTag(col, "Enemy", othercol);
        Debug.Log("I did this");
    }



    

    void Start()
    {
        _OnTargetCheck += IdentifySingleEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
