using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System;

[InlineProperty]
[LabelWidth(75)]
public class ConditionalWeaponList
{
    [ValueDropdown("valueList")]
    [HideLabel]
    [SerializeField]
    protected int WeaponType;
    
    private static ValueDropdownList<int> valueList = new ValueDropdownList<int>()
    {
        {"IsProjectileWeapon", 0},
        {"IsAOEWeapon", 1},
        {"IsLaZoRWeapon",2}
    };


    [ShowIf("WeaponType", 0)]
    [SerializeField]
    protected Func<Projectile>[] ProjeEvent = new Func<Projectile>[0];

    [ShowIf("WeaponType", 1)]
    [SerializeField]
    protected Action<AOEProjectile>[] AoeEvent = new Action<AOEProjectile>[0];

    [ShowIf("WeaponType", 2)]
    [SerializeField]
    protected Func<bool>[] LaZorEvent = new Func<bool>[0];
    
}
