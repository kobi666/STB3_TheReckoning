using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TestProjectileWeapon : WeaponController
{
    // Start is called before the first frame update
    
    



    

    void Start()
    {
        _OnTargetCheck += SetEnemyTarget;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
