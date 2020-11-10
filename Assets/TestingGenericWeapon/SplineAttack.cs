using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;

public class SplineAttack : WeaponAttack
{

    [TypeFilter("GetBeamAttacks")] [LabelText("Attack Function")] [OdinSerialize]
    public SplineAttackFunction AttackFunction;
    
    private static IEnumerable<Type> GetBeamAttacks()
    {
        var q = typeof(SplineAttackFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(SplineAttackFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    
    
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        
    }

    public override void InitlizeAttack()
    {
        
    }
}
