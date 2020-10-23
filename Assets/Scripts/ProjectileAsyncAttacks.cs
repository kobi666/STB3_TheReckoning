using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

[System.Serializable]
public class ProjectileAsyncAttacks
{
    
    public static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ProjectileAttackFunction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ProjectileAttackFunction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    /*public static ValueDropdownList<ProjectileAttackFunction> Attacks()
    {
        var t = typeof(ProjectileAttackFunction);
        var list = typeof(ProjectileAsyncAttacks).Assembly.GetTypes()
            .Where(x => typeof(ProjectileAttackFunction).GetTypeInfo().IsAssignableFrom(x.GetType()));
        
        ValueDropdownList<ProjectileAttackFunction> paflist = new ValueDropdownList<ProjectileAttackFunction>();
        foreach (Type at in list)
        {
            ConstructorInfo cinfo = at.GetConstructor(Type.EmptyTypes);
            ProjectileAttackFunction paf = cinfo.Invoke(null) as ProjectileAttackFunction;
            if (paf != null)
            {
                paflist.Add(at.Name, paf);
            }
        }

        return paflist;
    }*/
}

[System.Serializable]
public class ShootOneProjectile : ProjectileAttackFunction {
    [ShowInInspector] 
    public override int NumOfProjectiles { get; set; } = 1;
    public override float AssistingFloat1 { get; set; }
    public override float AssistingFloat2 { get; set; }
    
    public override int AssistingInt1 { get; set; }
    public override int AssistingInt2 { get; set; }

    public override void AttackFunction(List<PoolObjectQueue<GenericProjectile>> projectilePools, int numOfProjectiles, Quaternion direction, Vector2 SingleTargetPosition,
        Effectable singleTarget, Effectable[] multiTargets, float af1, float af2, int ai1, int ai2, List<ProjectileExitPoint> exitPoints,
        List<ProjectileFinalPoint> finalPoints)
    {
        GenericProjectile proj = projectilePools[0].GetInactive();
        proj.transform.position = exitPoints[0].transform.position;
        proj.TargetPosition = SingleTargetPosition; // can also be projectile final point position
        proj.EffectableTarget = singleTarget ?? null;
        proj.transform.rotation = exitPoints[0].transform.rotation;
        proj.Activate();
    }

    public void InvokeAttack(List<PoolObjectQueue<GenericProjectile>> projectilePools, Quaternion direction, Vector2 SingleTargetPosition, Effectable singleTarget,
        Effectable[] multiTargets)
    {
        AttackFunction(projectilePools, NumOfProjectiles, direction, SingleTargetPosition, singleTarget,
            multiTargets, AssistingFloat1, AssistingFloat2, AssistingInt1, AssistingInt2, ProjectileExitPoints,
            ProjectileFinalPoints);
    }
}
    
    
    
    [System.Serializable]
    public class ShootMultipleProjectilesOneAfterTheOther : ProjectileAttackFunction
    {
        [ShowInInspector]
        public override int NumOfProjectiles { get; set; }
        
        [ShowInInspector]
        [LabelText("Time between each Projectile")]
        public override  float AssistingFloat1 { get; set; }
        public override float AssistingFloat2 { get; set; }
        public override int AssistingInt1 { get; set; }
        public override int AssistingInt2 { get; set; }

        public override async void AttackFunction(List<PoolObjectQueue<GenericProjectile>> projectilePools, int numOfProjectiles, Quaternion direction, Vector2 SingleTargetPosition,
            Effectable singleTarget, Effectable[] multiTargets, float timebetweenProjectiles, float af2, int ai1, int ai2, List<ProjectileExitPoint> exitPoints,
            List<ProjectileFinalPoint> finalPoints)
        {
            int counter = numOfProjectiles;
            float timeCounter = 0;
            while (counter >= 0)
            {
                if (timeCounter <= 0) {
                GenericProjectile proj = projectilePools[0].GetInactive();
                proj.transform.position = exitPoints[0].transform.position;
                proj.TargetPosition = SingleTargetPosition; // can also be projectile final point position
                proj.EffectableTarget = singleTarget ?? null;
                proj.transform.rotation = exitPoints[0].transform.rotation;
                proj.Activate();
                timeCounter = timebetweenProjectiles;
                counter -= 1;
                }
                timeCounter -= StaticObjects.instance.DeltaGameTime;
            }
        }
    }

