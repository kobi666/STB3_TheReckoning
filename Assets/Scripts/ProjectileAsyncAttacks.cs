using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
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
    public override int NumOfProjectiles { get; set; } = 1;

    public ProjectilePoolCreationData Projectile;

    private List<PoolObjectQueue<GenericProjectile>> projectilePool;
    public override List<PoolObjectQueue<GenericProjectile>> ProjectilePools { get => projectilePool; set => projectilePool = value; }
    public override void InitializeAttack()
    {
        ProjectilePools.Add(Projectile.CreatePool());
    }

    public override void AttackFunction(Quaternion direction, Vector2 originPosition, Vector2 SingleTargetPosition,
        Effectable singleTarget)
    {
        GenericProjectile proj = ProjectilePools[0].GetInactive();
        proj.TargetPosition = SingleTargetPosition; // can also be projectile final point position
        proj.EffectableTarget = singleTarget ?? null;
        proj.Activate();
    }

    
}
    
    
    
    [System.Serializable]
    public class ShootMultipleProjectilesOneAfterTheOther : ProjectileAttackFunction
    {
        [ShowInInspector]
        public override int NumOfProjectiles { get; set; }

        public ProjectilePoolCreationData Projectile;

        public override List<PoolObjectQueue<GenericProjectile>> ProjectilePools { get; set; }

        public override void InitializeAttack()
        {
            ProjectilePools.Add(Projectile.CreatePool());
        }

        public override async void AttackFunction(Quaternion direction, Vector2 originPosition,
            Vector2 SingleTargetPosition, Effectable singleTarget)
        {
            int counter = NumOfProjectiles;
            float timeCounter = 0;
            while (counter >= 0)
            {
                if (timeCounter <= 0) {
                    GenericProjectile proj = ProjectilePools[0].GetInactive();
                    proj.transform.position = originPosition;
                    proj.TargetPosition = SingleTargetPosition; // can also be projectile final point position
                    proj.EffectableTarget = singleTarget ?? null;
                    proj.transform.rotation = direction;
                    proj.Activate();
                    timeCounter = timebetweenProjectiles;
                    counter -= 1;
                }
                timeCounter -= StaticObjects.instance.DeltaGameTime;
                await Task.Yield();
            }
        }


        public float timebetweenProjectiles = 0.5f;

    }

