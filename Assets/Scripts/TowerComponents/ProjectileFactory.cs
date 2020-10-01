using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix;
using Sirenix.OdinInspector;

public class ProjectileFactory : SerializedMonoBehaviour
{
    public static GenericProjectile CreateProjectileVariant(ProjectileCreationData projectileCreationData )
    {
        GenericProjectile proj = Instantiate(projectileCreationData.projectileBase,
            new Vector3(999.0f, 999.0f, 999.0f), Quaternion.identity);
        proj.gameObject.SetActive(false);
        foreach (var VARIABLE in projectileCreationData.singleTargetEffects)
        {
            if (VARIABLE == null)
            {
                continue;
            }
            proj.onHitSingleTarget += VARIABLE;
        }

        foreach (var VARIABLE in projectileCreationData.multiTargerEffects)
        {
            if (VARIABLE == null)
            {
                continue;
            }
            proj.onHitMultipleTargets += VARIABLE;
        }
        proj.onMainMovementAction += projectileCreationData.MovementFunction;
        proj.DirectHitProjectile = projectileCreationData.isDirectHitProjectile;
        proj.AOEProjectile = projectileCreationData.isAOEProjectile;
        proj.SingleTargetProjectile = projectileCreationData.isSingletargetProjectile;
        proj.HomingProjectile = projectileCreationData.isHomingProjectile;
        
        return proj;
    }
        

    

    
}
