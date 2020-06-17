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
[System.Serializable]
public abstract class ProjectileAttack<T> : Attack where T: Projectile,IQueueable<T> {
    [SerializeField]
    T projectilePrefab;
    public T ProjectilePrefab {get => projectilePrefab ;set { projectilePrefab = value;}}
    public PoolObjectQueue<T> Pool;
    public abstract void GetPool();
}



public class TowerWeaponAttacks
{
    public static void TestDebugRay(WeaponController self) {
        Debug.DrawLine(self.ProjectileExitPoint, self.Target.transform.position);
    }


    
}


