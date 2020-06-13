using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestYieldWeaponController : WeaponControllerYield
{
    int TestCounter = 0;
    public override void MainAttackFunction() {
        Debug.DrawLine(transform.position, Target.transform.position);
        Debug.LogWarning(TestCounter);
    }

    void incCounter() {
        TestCounter += 1;
    }

    public override bool CanAttack() {
        if (ExternalAttackLock == false) {
            if (Target != null) {
                return true;
            }
        }
        return false;
    }


    public override void PostStart() {
        onAttackInitiate += incCounter;
    }
}
