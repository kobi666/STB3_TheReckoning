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
    public List<GenericWeaponController> Weapons = new List<GenericWeaponController>();
    public AnimationClip OnAttackAnimation;
    public AnimationClip OnSpecialAttackAnimation;

    void updateTargetState(string tname, bool state)
    {
        if (tname == TargetUnit.name)
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

    private void Start()
    {
        if (!Weapons.IsNullOrEmpty())
        {
            Weapons = GetComponentsInChildren<GenericWeaponController>().ToList();
        }

        GameObjectPool.Instance.onTargetableUpdate += updateTargetState;
    }
}
