using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;


public enum EffectNames
{
    None = 0,
    Damage = 1,
    Freeze = 2,
    Slow = 3,
    SplitMultipleProjectiles = 4,
}

[System.Serializable]
public class Effect
{
    public virtual void UpdateEffectValues(Effect ef)
    {
        
    }
    public virtual EffectNames EffectName()
    {
        return EffectNames.None;
    }
    public GenericWeaponController ParentWeapon;

    public virtual void Apply(Effectable ef, Vector2 targetPos)
    {
        
    }
    public bool IsAOE;

    public virtual void InitializeEffectForWeapon(GenericWeaponController parentWeapon)
    {
        
    }
}


[System.Serializable]
[HideLabel]
public class Damage : Effect
{
    public override void UpdateEffectValues(Effect ef)
    {
        Damage d = ef as Damage;
        DamageRange.min += d.DamageRange.min;
        DamageRange.max += d.DamageRange.max;
    }

    public override EffectNames EffectName()
    {
        return EffectNames.Damage;
    }

    [ShowInInspector]
    public DamageRange DamageRange = new DamageRange();
    
    public override void Apply(Effectable ef, Vector2 targetPos)
    {
        ef?.ApplyDamage(DamageRange.RandomDamage());
    }

    public override void InitializeEffectForWeapon(GenericWeaponController parentWeapon)
    {
        
    }
    
    
}


public class DebugEffect : Effect
{
    public override void Apply(Effectable ef,Vector2 targetPos)
    {
        Debug.LogWarning(ef.name + " was affected");
    }

    public override void InitializeEffectForWeapon(GenericWeaponController parentWeapon)
    {
        
    }
}

public class SplitToMultipleProjectiles : Effect
{
    public int AmountOfProjectiles = 3;
    public List<ProjectilePoolCreationData> ProjectileData = new List<ProjectilePoolCreationData>();
    private PoolObjectQueue<GenericProjectile> pool = null;
    public float travelDistance;

    public PoolObjectQueue<GenericProjectile> Pool
    {
        get
        {
            if (pool == null)
            {
                pool = ProjectileData[0].CreatePool();
            }
            return pool;
        }
        set => pool = value;
    }
    public override void Apply(Effectable ef,Vector2 tpos)
    {
        for (int i = 0; i < AmountOfProjectiles; i++)
        {
            GenericProjectile proj = Pool.GetInactive();
            //proj.transform.rotation = ExitPoint.transform.rotation;
            proj.transform.position = tpos;
            proj.TargetExclusionList.Add(ef.name);
            Vector2 randomDirection = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
            proj.TargetPosition = tpos + randomDirection * travelDistance;
            proj.EffectableTarget = ef ?? null;
            proj.Activate();
        }
    }

    public override void InitializeEffectForWeapon(GenericWeaponController parentWeapon)
    {
        ParentWeapon = parentWeapon;
        Pool = ProjectileData[0].CreatePool(ParentWeapon.name);
    }
}




[System.Serializable]
public class DoSomethingWithPrefab : Effect
{
    [ShowInInspector] public GameObject somePrefab;
    
    public override void Apply(Effectable ef,Vector2 targetPos)
    {
        EffectableUnit efu = ef as EffectableUnit;
        if (efu.unitController != null)
        {
            if (somePrefab != null)
            {
                GameObject someGo = GameObject.Instantiate(somePrefab);
            }
        }
    }

    public override void InitializeEffectForWeapon(GenericWeaponController parentWeapon)
    {
        
    }
}


