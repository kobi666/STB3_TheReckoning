using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcingProjectileLauncherController : WeaponController
{
    
    // Start is called before the first frame update
    public override void MainAttackFunction()
    {
        //ProjectileUtils.SpawnArcingAOEProjectile(ProjectileQueuePool, ProjectileExitPoint, Target.transform.position, 6f, Data.projectileData.projectileSpeed, Data.projectileData.aoeProjectileRadius, HitAndExplode);
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

    public override void InitComponent()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
      base.Start();
    }

    public override List<Effect> GetEffectList()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        throw new System.NotImplementedException();
    }

    public override List<TagDetector> GetRangeDetectors()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        throw new System.NotImplementedException();
    }

    

    // Update is called once per frame
    
}
