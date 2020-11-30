using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;

public class SplineAttack : WeaponAttack
{
    [TypeFilter("GetSplineAttacks")] [LabelText("Attack Function")] [OdinSerialize]
    public SplineAttackProperties SplineAttacks;
    
    private static IEnumerable<Type> GetSplineAttacks()
    {
        var q = typeof(SplineAttackProperties).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(SplineAttackProperties).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    public List<SplineBehavior> Splines {get; set;}
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        
    }

    public override void InitlizeAttack()
    {
        
    }
}
