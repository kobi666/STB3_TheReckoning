using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

[System.Serializable]
[HideLabel]
public class ProjectilePoolCreationData
{
    
    [TypeFilter("GetMovementFunctions")]
    public ProjectileMovementFunction projectileMovement;
    public ProjectileEffect projectileEffect = new ProjectileEffect();
    public GenericProjectile ProjectileBase;
    Vector3 nowhere = new Vector3(9999,9999,9999);
    
     
    private static IEnumerable<Type> GetMovementFunctions()
    {
        var q = typeof(ProjectileMovementFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ProjectileMovementFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    public (PoolObjectQueue<GenericProjectile>,string) CreatePool()
    {
        GenericProjectile proj = GameObject.Instantiate(ProjectileBase, nowhere, Quaternion.identity);
        proj.gameObject.SetActive(false);
        proj.projectileEffect = projectileEffect;
        proj.name = ProjectileBase + Random.Range(0, 999999).ToString();
        GameObject placeholder = GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity,
            GameObjectPool.Instance.transform);
        placeholder.name = proj.name + "_placeHolder";
        PoolObjectQueue<GenericProjectile> pool = new PoolObjectQueue<GenericProjectile>(proj, 5, placeholder);
        pool.ObjectQueue.Enqueue(proj);
        return (pool, proj.name);
    }
}