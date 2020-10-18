using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

public class ProjectileAsyncAttacks 
{
    public class ShootOneProjectile : ProjectileAttackFunction
    {
        [ShowInInspector]
        public override int NumOfProjectiles { get; set; }
        
        public override float AssistingFloat1 { get; set; }
        public override float AssistingFloat2 { get; set; }
        
        public override int AssistingInt1 { get; set; }
        public override int AssistingInt2 { get; set; }

        public override async void AttackFunction(List<PoolObjectQueue<GenericProjectile>> projectilePools, int numOfProjectiles, Quaternion direction, Vector2 SingleTargetPosition,
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
    }
    
    
    public class ShootProjectileInArc
    {
        
    }
}
