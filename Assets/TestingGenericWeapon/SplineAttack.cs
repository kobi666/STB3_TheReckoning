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
    public SplineAttackProperties SplineAttackType;
    
    private static IEnumerable<Type> GetSplineAttacks()
    {
        var q = typeof(SplineAttackProperties).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(SplineAttackProperties).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        SplineAttackType.StartAsyncSplineAttack(singleTarget, SingleTargetPosition);
    }

    public void StopSplineAttack()
    {
        SplineAttackType.StopAttack();
    }

    public override void InitlizeAttack(GenericWeaponController parentWeapon)
    {
        SplineAttackType.InitlizeProperties();
        SplineAttackType.SpecificPropertiesInit();
    }
}
