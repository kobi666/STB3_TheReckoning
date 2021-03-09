using System;
using System.Collections.Generic;
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
    
    public void InitializeAttackProperties(GenericWeaponController parentWeapon)
    {
        ParentWeapon = parentWeapon;
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
        foreach (var fp in GetFinalPoints())
        {
            var position = fp.transform.position;
            Vector2 pos = (Vector2)position +
                          new Vector2(ParentWeapon.Data.componentRadius, position.y);
            fp.transform.position = pos;
        }
    }


    public  List<ProjectileExitPoint> GetExitPoints()
    {
        return ProjectileExitPoints;
    }

    public void SetInitialExitPointPosition()
    {
        throw new NotImplementedException();
    }
}
