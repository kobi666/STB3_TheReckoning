using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

[System.Serializable]
public class ProjectileAttack
{
    [TypeFilter("GetProjectileAttacks")]
    [LabelText("Attack Function")]
    [ShowInInspector]
    public ProjectileAttackFunction AttackFunction = new ShootOneProjectile();
    
    private static IEnumerable<Type> GetProjectileAttacks()
    {
        var q = typeof(ProjectileAttackFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ProjectileAttackFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }


    [LabelText("Projectile Data")]
    public List<ProjectilePoolCreationData> ProjectilesData = new List<ProjectilePoolCreationData>();
    public Dictionary<string,PoolObjectQueue<GenericProjectile>> projectilePools = new Dictionary<string,PoolObjectQueue<GenericProjectile>>();

    public void InitPools()
    {
        foreach (var projs in ProjectilesData)
        {
            if (projs != null)
            {
                (PoolObjectQueue<GenericProjectile>, string) pool = projs.CreatePool();
                projectilePools.Add(pool.Item2,pool.Item1);
            }
        }
    }
    
    
}







