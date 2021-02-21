using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MyBox;

public class UnitBattleManager : MonoBehaviour
{
    private GenericUnitController targetUnit;
    public bool targetExists;
    public GenericUnitController TargetUnit
    {
        get => targetUnit;
        set
        {
            targetUnit = value;
            if (value == null)
            {
                targetExists = false;
            }
            else
            {
                targetExists = true;
            }
        }
    }

    public GenericWeaponController MeleeWeapon;
    public List<GenericWeaponController> AllWeapons = new List<GenericWeaponController>();
    public AnimationClip OnAttackAnimation;
    public AnimationClip OnSpecialAttackAnimation;

    void updateTargetState(string tname, bool state)
    {
        if (tname == TargetUnit?.name)
        {
            if (state == false)
            {
                TargetUnit = null;
            }
        }
    }

    public event Action onAttackInitiate;
    public event Action onAttackCease;
    public event Action onAttack;

    void removeTarget(string targetName, string caller)
    {
        if (TargetUnit?.name == targetName)
        {
            TargetUnit = null;
        }
    }

    private void Start()
    {
        if (AllWeapons.IsNullOrEmpty())
        {
            AllWeapons = GetComponentsInChildren<GenericWeaponController>().ToList();
        }

        foreach (var weapon in AllWeapons)
        {
            weapon.TargetBank.AddNameExclusion(name);
        }
        GameObjectPool.Instance.onTargetableUpdate += updateTargetState;
        if (MeleeWeapon) {
        MeleeWeapon.TargetBank.onTargetRemove += removeTarget;
        }
        
    }
}
