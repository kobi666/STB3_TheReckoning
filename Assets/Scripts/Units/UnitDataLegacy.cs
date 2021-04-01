using System.Collections.Generic;
using UnityEngine;
using System;
using BansheeGz.BGSpline.Components;
using Sirenix.OdinInspector;


[System.Serializable]
public class UnitData
{
    public UnitMetaData MetaData = new UnitMetaData();
    public UnitDynamicData DynamicData = new UnitDynamicData();
}

[Serializable]
public class UnitMetaData
{
    public int HP;
    public float MovementSpeed = 1;
    public float AttackSpeed;
    public float DiscoveryRadius = 1;
}

[Serializable]
public class UnitDynamicData
{
    public Vector2 TargetPosition;
    [ShowInInspector]
    public Vector2? BasePosition;
    public BGCcMath Spline;
    public float DistanceOnPath;
}


[System.Serializable]
public class UnitDataLegacy
{
    public bool CanBeStoppedByOtherUnit;
    public Dictionary<string, GameObject> TargetsInRange;
    public int AttackRate;
    public DamageRange DamageRange;
    public int HP;
    public int Armor;
    public int SpecialArmor;
    public int speed;
    public Vector2 SetPosition;

    public Effectable EffectableTarget;
    public GameObject Target;
    
    
    
    public UnitType unitType;
    public Damage_Type damageType = new Damage_Type("normal");
    

    

    

    

    

    

    

    
}
