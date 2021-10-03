using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentFilters;


public abstract class ComponentFilter
{
    public abstract bool ValidateComponent(TowerComponent towerComponent);
}

namespace ComponentFilters
{
    public class AllComponents : ComponentFilter
    {
        public override bool ValidateComponent(TowerComponent towerComponent)
        {
            return true;
        }
    }
    
    

    public class IsAOE : ComponentFilter
    {
        public override bool ValidateComponent(TowerComponent towerComponent)
        {
            if (towerComponent is GenericWeaponController s)
            {
                if (s.WeaponAttack is AOEAttack)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class IsProjectileAOE : ComponentFilter
    {
        public override bool ValidateComponent(TowerComponent towerComponent)
        {
            if (towerComponent is GenericWeaponController s)
            {
                if (s.WeaponAttack is ProjectileAttack pa)
                {
                    foreach (var effect in pa.GetEffects())
                    {
                        if (effect.IsAOE)
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
        }
    }

    public class IsProjectile : ComponentFilter
    {
        public override bool ValidateComponent(TowerComponent towerComponent)
        {
            if (towerComponent is GenericWeaponController s)
            {
                if (s.WeaponAttack is ProjectileAttack)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class IsProjectileFamily1 : ComponentFilter
    {
        public List<ProjectileFamily> ProjectileFamilies = new List<ProjectileFamily>();

        public override bool ValidateComponent(TowerComponent towerComponent)
        {
            if (towerComponent is GenericWeaponController s)
            {
                if (s.WeaponAttack is ProjectileAttack pa)
                {
                    foreach (var p in pa.ProjectileAttackProperties.Projectiles)
                    {
                        if (ProjectileFamilies.Contains(p.ProjectileFamily))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
    
    public class IsBeam : ComponentFilter
    {
        public override bool ValidateComponent(TowerComponent towerComponent)
        {
            if (towerComponent is GenericWeaponController s)
            {
                if (s.WeaponAttack is SplineAttack)
                {
                    return true;
                }
            }

            return false;
        }
    }
}