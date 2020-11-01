using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.Serialization;
using Random = UnityEngine.Random;

public class ProjectileAttack
{
    [TypeFilter("GetProjectileAttacks")]
    [LabelText("Attack Function")]
    [OdinSerialize]
    public ProjectileAttackFunction AttackFunction = new ShootOneProjectile();
    
    private static IEnumerable<Type> GetProjectileAttacks()
    {
        var q = typeof(ProjectileAttackFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ProjectileAttackFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }


}







