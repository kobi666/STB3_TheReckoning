using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

[HideLabel]
[System.Serializable]
public class ProjectilePoolCreationData
{
    
    [TypeFilter("GetMovementFunctions")][SerializeReference]
    public ProjectileMovementFunction projectileMovement;

    [HideLabel] public ProjectileEffect projectileEffect;
    public GenericProjectile ProjectileBase;
    static Vector3 nowhere = new Vector3(9999,9999,9999);
    
     
    private static IEnumerable<Type> GetMovementFunctions()
    {
        var q = typeof(ProjectileMovementFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(ProjectileMovementFunction))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    public PoolObjectQueue<GenericProjectile> CreatePool()
    {
        GenericProjectile proj = GameObject.Instantiate(ProjectileBase, nowhere, Quaternion.identity);
        proj.BaseProjectileEffect = projectileEffect;
        proj.MovementFunction = projectileMovement;
        proj.InitProjectile();
        proj.gameObject.SetActive(true);
        proj.name = ProjectileBase + "_" + Random.Range(0, 999999);
        GameObject placeholder = GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity,
            GameObjectPool.Instance.transform);
        placeholder.name = proj.name + "_placeHolder";
        proj.gameObject.SetActive(false);
        PoolObjectQueue<GenericProjectile> pool = new PoolObjectQueue<GenericProjectile>(proj, 10, placeholder);
        //pool.ObjectQueue.Enqueue(proj);
        return pool;
    }
    public PoolObjectQueue<GenericProjectile> CreatePool(string ParentName)
    {
        GenericProjectile proj = GameObject.Instantiate(ProjectileBase, nowhere, Quaternion.identity);
        proj.BaseProjectileEffect = projectileEffect;
        proj.MovementFunction = projectileMovement;
        proj.gameObject.SetActive(true);
        proj.name = ParentName + "_" + ProjectileBase.name + "_" + Random.Range(0, 999999);
        GameObject placeholder = GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity,
            GameObjectPool.Instance.transform);
        placeholder.name = proj.name + "_placeHolder";
        proj.gameObject.SetActive(false);
        PoolObjectQueue<GenericProjectile> pool = new PoolObjectQueue<GenericProjectile>(proj, 10, placeholder);
        //pool.ObjectQueue.Enqueue(proj);
        return pool;
    }
}