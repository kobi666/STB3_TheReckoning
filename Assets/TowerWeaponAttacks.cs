using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack {

    public float FireCounter = 0;
    public abstract void AttackFunction(WeaponController self);
}
[System.Serializable]
public abstract class ProjectileAttack : Attack {
    [SerializeField]
    public Projectile ProjectilePrefab {get;}
    
}
[System.Serializable]
public class OnHitProjectileAttack : ProjectileAttack {
    public override void AttackFunction(WeaponController self) {
        FireCounter += (StaticObjects.instance.DeltaGameTime * self.Data.FireRate) / 10;
        if (FireCounter >= 10) {

        }
    }
    
}

public class TowerWeaponAttacks
{
    public static void TestDebugRay(WeaponController self) {
        Debug.DrawLine(self.ProjectileExitPoint, self.Target.transform.position);
    }
    
}


