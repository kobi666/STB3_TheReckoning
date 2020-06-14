using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestYieldWeaponController : WeaponController
{
    int TestCounter = 0;
    public override void MainAttackFunction() {
        TowerWeaponAttacks.TestDebugRay(this);
    }

    void incCounter() {
        TestCounter += 1;
    }


    public override void PostStart() {
        onAttackInitiate += incCounter;
    }
}
