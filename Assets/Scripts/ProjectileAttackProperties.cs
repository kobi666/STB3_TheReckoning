using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public abstract class ProjectileAttackProperties : AttackProperties,IhasExitAndFinalPoint
{
    public abstract int ProjectileMultiplier { get; set; }
    
    [Required]
    public List<ProjectileFinalPoint> ProjectileFinalPoints = new List<ProjectileFinalPoint>();
    [Required]
    public List<ProjectileExitPoint> ProjectileExitPoints = new List<ProjectileExitPoint>();
    
    [Required]
    public List<ExitFinalPointPair> ExitFinalPointPairs = new List<ExitFinalPointPair>();


    private bool finalPointMovementInProgress = false;
    async void  SetFinalPointPositionToTarget()
    {
        if (!finalPointMovementInProgress)
        {
            finalPointMovementInProgress = true;
            while (finalPointMovementInProgress)
            {
                foreach (var exitFinalPointPair in ExitFinalPointPairs)
                {
                    exitFinalPointPair.cachedFinalPointPosition = ParentWeapon.Target?.transform.position ?? exitFinalPointPair.cachedFinalPointPosition;
                    exitFinalPointPair.FinalPoint.transform.position = exitFinalPointPair.cachedFinalPointPosition;
                }

                await Task.Yield();
            }

            finalPointMovementInProgress = false;
        }
    }

    void stopFinalPointMovement()
    {
        finalPointMovementInProgress = false;
    }
    
    public void InitializeAttackProperties(GenericWeaponController parentWeapon)
    {
        ParentWeapon = parentWeapon;
        if (!ParentWeapon.RotatingComponent)
        {
            ParentWeapon.onAttackInitiate += SetFinalPointPositionToTarget;
            ParentWeapon.onAttackCease += stopFinalPointMovement;
        }
        InitializeAttack(ParentWeapon);
    }
    public abstract void InitializeAttack(GenericWeaponController parentWeapon);

    public abstract void AttackFunction(Effectable singleTarget,
         Vector2 SingleTargetPosition);
    
    public event Action onAttack;
    
    public bool AsyncAttackInProgress;
    
    
    [SerializeField] public List<ProjectilePoolCreationData> Projectiles = new List<ProjectilePoolCreationData>();

    public async void Attack(Effectable singleTarget,
        Vector2 singleTargetPosition)
    {
        onAttack?.Invoke();
        AttackFunction(singleTarget, singleTargetPosition);
    }

    public List<ProjectileFinalPoint> GetFinalPoints()
    {
        return ProjectileFinalPoints;
    }

    public void SetInitialFinalPointPosition()
    {
        foreach (var exitFinal in ExitFinalPointPairs)
        {
            var position1 = exitFinal.ExitPoint.transform.position;
            var distanceForFinalPoint = (ParentWeapon.Data.componentRadius / 1.5f) -
                                        Vector2.Distance(ParentWeapon.transform.position, position1);
            var position = new Vector2(position1.x + distanceForFinalPoint,
                position1.y);
            exitFinal.FinalPoint.transform.position = position;
        }
    }


    public  List<ProjectileExitPoint> GetExitPoints()
    {
        return ProjectileExitPoints;
    }

    public void SetInitialExitPointPosition()
    {
        
    }
}


[System.Serializable]
public class ExitFinalPointPair
{
    public ProjectileExitPoint ExitPoint;
    public ProjectileFinalPoint FinalPoint;
    public Vector2 cachedFinalPointPosition;
}
