using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix;
using Sirenix.OdinInspector;

public class ProjectileFactory : SerializedMonoBehaviour
{
    public static GenericProjectile CreateProjectileBase(GenericProjectile projectileBase, ProjectileEffect projectileEffect, string weaponName )
    {
        
        GenericProjectile proj = Instantiate(projectileBase,
            new Vector3(999.0f, 999.0f, 999.0f), Quaternion.identity);
        proj.gameObject.SetActive(false);
        proj.name = projectileBase.name + '_' + weaponName;
        proj.BaseProjectileEffect = projectileEffect;
        return proj;
    }

    public static PoolObjectQueue<GenericProjectile> GetOrCreateGenericProjectilePool(GenericProjectile projectileBase,
        ProjectileEffect projectileEffect, string weaponName)
    {
        GenericProjectile proj = CreateProjectileBase(projectileBase,
            projectileEffect, weaponName);
        return GameObjectPool.Instance.GetOrCreateGenericProjectileQueue(proj);
    } 
        

    

    
}
