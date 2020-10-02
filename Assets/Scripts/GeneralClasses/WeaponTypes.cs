using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System;

[InlineProperty]
[LabelWidth(75)]
public class WeaponTypes
{
    [ValueDropdown("valueList", DropdownWidth = 100)]
    [HideLabel]
    [BoxGroup]
    [SerializeField]
    protected int WeaponType;
    
    private static ValueDropdownList<int> valueList = new ValueDropdownList<int>()
    {
        {"IsProjectileWeapon", 0},
        {"IsAOEWeapon", 1},
        {"IsLaZoRWeapon",2}
    };
    [ShowIf("WeaponType", 0)] public GenericProjectile ProjectileBase;
    
    
    [ShowIf("WeaponType", 0)] 
    [SerializeField]
    [BoxGroup]
    public ProjectileBehaviorData ProjectileBehavior = new ProjectileBehaviorData();

    [ShowIf("WeaponType", 1)]
    [SerializeField]
    protected Action<AOEProjectile>[] AoeEvent = new Action<AOEProjectile>[0];

    [ShowIf("WeaponType", 2)]
    [SerializeField]
    protected Func<bool>[] LaZorEvent = new Func<bool>[0];
}
