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
public class ProjectileAsyncAttacks
{
    
    public static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ProjectileAttackFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ProjectileAttackFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
}

[System.Serializable]
public class ShootOneProjectile : ProjectileAttackFunction {
    [ShowInInspector] 
    public override int ProjectileMultiplier { get; set; } = 1;

    [OdinSerialize] public List<ProjectilePoolCreationData> Projectile = new List<ProjectilePoolCreationData>();
    //public List<ProjectilePoolCreationData> pools = new List<ProjectilePoolCreationData>();
    public ProjectileExitPoint ExitPoint;
    public ProjectileFinalPoint FinalPoint;

    private PoolObjectQueue<GenericProjectile> projectilePool;
    
    public override void InitializeAttack()
    {
        projectilePool = Projectile[0].CreatePool();
    }
    
    
    
    public override async void AttackFunction(Effectable singleTarget,Vector2 SingleTargetPosition)
    {
        if (AsyncAttackInProgress == false)
        { 
            AsyncAttackInProgress = true;
            GenericProjectile proj = projectilePool.GetInactive();
            proj.transform.rotation = ExitPoint.transform.rotation;
            proj.transform.position = ExitPoint.transform.position;
            proj.TargetPosition = FinalPoint?.Position ?? SingleTargetPosition; // can also be projectile final point position
            proj.EffectableTarget = singleTarget ?? null;
            proj.Activate();
            AsyncAttackInProgress = false;
        }
    }

    
}
    
    
    
    [System.Serializable]
    public class ShootMultipleProjectilesOneAfterTheOther : ProjectileAttackFunction
    {
        [ShowInInspector]
        public override int ProjectileMultiplier { get; set; }

        public int NumOfProjectiles = 3;
        public float timebetweenProjectiles = 0.5f;

        
        [OdinSerialize] public List<ProjectilePoolCreationData> Projectile = new List<ProjectilePoolCreationData>();
        public ProjectileExitPoint ExitPoint;
        public ProjectileFinalPoint FinalPoint;
        
        private PoolObjectQueue<GenericProjectile> projectilePool;

        public override void InitializeAttack()
        {
            projectilePool = Projectile[0].CreatePool();
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
                    timeCounter -= StaticObjects.instance.DeltaGameTime;
                    await Task.Yield();
                }

                AsyncAttackInProgress = false;
            }
        }


       

    }

