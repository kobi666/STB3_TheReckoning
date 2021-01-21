using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;

[System.Serializable]
public class ShootOneProjectile : ProjectileAttackProperties 
{
    [ShowInInspector] 
    public override int ProjectileMultiplier { get; set; } = 1;
    
    public ProjectileExitPoint ExitPoint;
    public ProjectileFinalPoint FinalPoint;

    private PoolObjectQueue<GenericProjectile> projectilePool;
    
    public override void InitializeAttack(GenericWeaponController parentWeapon)
    {
        projectilePool = Projectiles[0].CreatePool();
    }
    
    
    
    public override async void AttackFunction(Effectable singleTarget,Vector2 SingleTargetPosition)
    {
        if (AsyncAttackInProgress == false)
        { 
            AsyncAttackInProgress = true;
            GenericProjectile proj = projectilePool.Get();
            proj.transform.rotation = ExitPoint.transform.rotation;
            proj.transform.position = ExitPoint.transform.position;
            proj.TargetPosition = FinalPoint?.Position ?? SingleTargetPosition; // can also be projectile final point position
            proj.EffectableTarget = singleTarget ?? null;
            proj.Activate();
            AsyncAttackInProgress = false;
        }
        
    }

    public override List<ProjectileFinalPoint> GetFinalPoints()
    {
        List<ProjectileFinalPoint> lpfp = new List<ProjectileFinalPoint>();
        lpfp.Add(FinalPoint);
        return lpfp;
    }

    public override List<ProjectileExitPoint> GetExitPoints()
    {
        List<ProjectileExitPoint> pepl = new List<ProjectileExitPoint>();
        pepl.Add(ExitPoint);
        return pepl;
    }
}


    
    
    
    [System.Serializable]
    public class ShootMultipleProjectilesOneAfterTheOther : ProjectileAttackProperties
    {
        [ShowInInspector]
        public override int ProjectileMultiplier { get; set; }

        public int NumOfProjectiles = 3;
        public float timebetweenProjectiles = 0.5f;
        
        public ProjectileExitPoint ExitPoint;
        public ProjectileFinalPoint FinalPoint;
        
        private PoolObjectQueue<GenericProjectile> projectilePool;

        public override void InitializeAttack(GenericWeaponController parentWeapon)
        {
            projectilePool = Projectiles[0].CreatePool();
        }
        
        public override List<ProjectileFinalPoint> GetFinalPoints()
        {
            List<ProjectileFinalPoint> lpfp = new List<ProjectileFinalPoint>();
            lpfp.Add(FinalPoint);
            return lpfp;
        }

        public override List<ProjectileExitPoint> GetExitPoints()
        {
            List<ProjectileExitPoint> pepl = new List<ProjectileExitPoint>();
            pepl.Add(ExitPoint);
            return pepl;
        }

        public override async void AttackFunction(Effectable singleTarget,
            Vector2 SingleTargetPosition)
        {
            int counter = NumOfProjectiles;
            float timeCounter = 0;
            if (AsyncAttackInProgress == false)
            {
                AsyncAttackInProgress = true;
                while (counter >= 0)
                {
                    if (timeCounter <= 0) {
                        GenericProjectile proj = projectilePool.GetInactive();
                        proj.transform.position = ExitPoint.transform.position;
                        proj.transform.rotation = ExitPoint.transform.rotation;
                        proj.TargetPosition = FinalPoint?.Position ?? SingleTargetPosition; //can also be projectile final point position
                        proj.EffectableTarget = singleTarget ?? null;
                        
                        proj.Activate();
                        timeCounter = timebetweenProjectiles;
                        counter -= 1;
                    }
                    timeCounter -= StaticObjects.Instance.DeltaGameTime;
                    await Task.Yield();
                }

                AsyncAttackInProgress = false;
            }
        }
    }

