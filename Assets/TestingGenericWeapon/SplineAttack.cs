using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

[System.Serializable]
public class SplineAttack : WeaponAttack
{
    [TypeFilter("GetSplineAttacks")] [LabelText("Attack Function")][SerializeReference]
    public SplineAttackProperties SplineAttackType = new SplineAttackProperties();


    private static IEnumerable<Type> GetSplineAttacks()
    {
        var q = typeof(SplineAttackProperties).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(SplineAttackProperties)));
        
        return q;
    }
    
    public override void Attack(Effectable singleTarget, Vector2 SingleTargetPosition)
    {
        SplineAttackType.StartAsyncSplineAttack(singleTarget, SingleTargetPosition);
    }

    public override void StopAttack()
    {
        StopSplineAttack();
    }

    public override List<Effect> GetEffects()
    {
        List<Effect> listeffect = new List<Effect>();
        foreach (var sb in SplineAttackType.SplineBehaviors )
        {
            foreach (var se in sb.SplineEffect)
            {
                foreach (var mtef in se.MainTargetEffects)
                {
                    listeffect.Add(mtef);
                }

                foreach (var opef in se.OnPathTargetsEffects)
                {
                    listeffect.Add(opef);
                }
            }
        }

        return listeffect;
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
