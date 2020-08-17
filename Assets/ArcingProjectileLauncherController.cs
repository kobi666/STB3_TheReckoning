using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcingProjectileLauncherController : WeaponController
{
    
    // Start is called before the first frame update
    public override void MainAttackFunction()
    {
        ProjectileUtils.SpawnArcingAOEProjectile(ProjectileQueuePool, ProjectileExitPoint, Target.transform.position, 6f, 1.2f, HitAndExplode);
    }

    void HitAndExplode(Effectable effectable)
    {
        int damage = Data.damageRange.RandomDamage();
        effectable?.ApplyDamage(damage);
        effectable?.ApplyExplosion(5f);
    }

    public override void PostStart()
    {
        
    }

    void Start()
    {
      base.Start();
    }

    // Update is called once per frame
    
}
