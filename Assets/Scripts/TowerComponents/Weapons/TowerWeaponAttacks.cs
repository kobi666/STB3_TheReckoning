﻿using UnityEngine;


public class TowerWeaponAttacks
{
    public static void TestDebugRay(WeaponController self) {
        Debug.DrawLine(self.ProjectileExitPoint, self.Target.transform.position);
    }

    public static void TestDebugRay(TestWeaponController self) {
        //Debug.DrawLine(self.ProjectileExitPoint, self.Target.transform.position);
    }


    
}


