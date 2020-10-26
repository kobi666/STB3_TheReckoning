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

    /*public static ValueDropdownList<ProjectileAttackFunction> Attacks()
    {
        var t = typeof(ProjectileAttackFunction);
        var list = typeof(ProjectileAsyncAttacks).Assembly.GetTypes()
            .Where(x => typeof(ProjectileAttackFunction).GetTypeInfo().IsAssignableFrom(x.GetType()));
        
        ValueDropdownList<ProjectileAttackFunction> paflist = new ValueDropdownList<ProjectileAttackFunction>();
        foreach (Type at in list)
        {
            ConstructorInfo cinfo = at.GetConstructor(Type.EmptyTypes);
            ProjectileAttackFunction paf = cinfo.Invoke(null) as ProjectileAttackFunction;
            if (paf != null)
            {
                paflist.Add(at.Name, paf);
            }
        }

        return paflist;
    }*/
}

[System.Serializable]
public class ShootOneProjectile : ProjectileAttackFunction {
    [ShowInInspector] 
    public override int NumOfProjectiles { get; set; } = 1;

    public override void AttackFunction(List<PoolObjectQueue<GenericProjectile>> projectilePools, int numOfProjectiles, Quaternion direction, Vector2 originPosition,
        Vector2 SingleTargetPosition, Effectable singleTarget)
    {
        GenericProjectile proj = projectilePools[0].GetInactive();
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

        public override async void AttackFunction(List<PoolObjectQueue<GenericProjectile>> projectilePools, int numOfProjectiles, Quaternion direction, Vector2 originPosition,
            Vector2 SingleTargetPosition, Effectable singleTarget)
        {
            int counter = numOfProjectiles;
            float timeCounter = 0;
            while (counter >= 0)
            {
                if (timeCounter <= 0) {
                    GenericProjectile proj = projectilePools[0].GetInactive();
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

