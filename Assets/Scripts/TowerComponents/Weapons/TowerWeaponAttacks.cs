using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class Attack {

    bool attackInitilized = false;
    public bool AttackInitilized {get => attackInitilized; private set {attackInitilized = value;}}
    public float FireCounter = 0;
    public abstract void AttackFunction(WeaponController firingWeapon);
    public void InitializeAttackOnce() {
        if (attackInitilized == false) {
            InitilizeAttack();
            attackInitilized = true;
        }
    }
    public abstract void InitilizeAttack();
    
}



public class TowerWeaponAttacks
{
    public static void TestDebugRay(WeaponController self) {
        Debug.DrawLine(self.ProjectileExitPoint, self.Target.transform.position);
    }

    public static void TestDebugRay(TestWeaponController self) {
        Debug.DrawLine(self.ProjectileExitPoint, self.Target.transform.position);
    }


    
}


