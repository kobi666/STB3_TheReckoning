using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

[System.Serializable]
public class ProjectileFireFunction
{
    [LabelText("Movement Function")]
    public ProjectileMovementFunction ProjectileMovementFunction;
    
    private static IEnumerable<Type> GetProjectileMovements()
    {
        var q = typeof(ProjectileAttackFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ProjectileAttackFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    
    
    
    
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

[System.Serializable]
[HideLabel]
public class ProjectilePoolCreationData
{
    public ProjectileEffect projectileEffect = new ProjectileEffect();
    public GenericProjectile ProjectileBase;
    Vector3 nowhere = new Vector3(9999,9999,9999);

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




[System.Serializable]
public abstract class ProjectileAttackFunction 
{
    public abstract int NumOfProjectiles { get; set; }

    public abstract void AttackFunction(List<PoolObjectQueue<GenericProjectile>> projectilePools, int numOfProjectiles,
        Quaternion direction, Vector2 originPosition, Vector2 SingleTargetPosition, Effectable singleTarget);

    public event Action onAttack; 
    
    public event Action<List<PoolObjectQueue<GenericProjectile>>, int,
        Quaternion, Vector2, Effectable, Effectable[],
    float, float, int, List<ProjectileExitPoint> ,List<ProjectileFinalPoint>> attack;
    public void Attack(List<PoolObjectQueue<GenericProjectile>> projectilePools, Quaternion direction, Vector2 originPosition,
        Vector2 SingleTargetPosition, Effectable singleTarget)
    {
        onAttack?.Invoke();
        AttackFunction(projectilePools, NumOfProjectiles, direction, originPosition,SingleTargetPosition, singleTarget);
    }
}
