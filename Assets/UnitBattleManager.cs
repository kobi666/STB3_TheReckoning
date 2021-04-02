using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;

public class UnitBattleManager : MonoBehaviour
{
    
    [Required]
    public BoxCollider2D AttackArea;

    [Required] 
    public GenericAOEController AoeController;

    private float AttackAreaOffset
    {
        get => AttackArea.bounds.extents.x;
    }

    private Vector2 AttackAreaOffsetV2
    {
        get => new Vector2(AttackAreaOffset,0);
    }

    /// <summary>
    /// true means flip to left, false means flip to right
    /// </summary>
    /// <param name="direction"></param>
    public void FlipAttackArea(bool direction)
    {
        var transform1 = MeleeWeapon.transform;
        if (direction)
        {
            transform1.position = (Vector2)transform.position - (AttackAreaOffsetV2 * 1.5f);
        }
        else
        {
            transform1.position = (Vector2)transform.position + (AttackAreaOffsetV2 * 1.5f);
        }
    }
    
    public bool targetExists;
    private GenericUnitController targetUnit;
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

    public event Action onFightStart;

    public void OnFightStart()
    {
        onFightStart?.Invoke();
    }
    public void StartFight()
    {
        MeleeWeapon.Target = GameObjectPool.Instance.GetTargetUnit(TargetUnit.MyGameObjectID);
        MeleeWeapon.InAttackState = true;
    }

    public event Action onFightEnd;

    public void OnFightEnd() {
        onFightEnd?.Invoke();
    }

    public void EndFight()
    {
        MeleeWeapon.Target = null;
        MeleeWeapon.InAttackState = false;
    }
    
    

    public GenericWeaponController MeleeWeapon;
    public List<GenericWeaponController> AllWeapons = new List<GenericWeaponController>();
    public AnimationClip OnAttackAnimation;
    public AnimationClip OnSpecialAttackAnimation;

    void updateTargetState(int gameObjectID, bool state)
    {
        if (gameObjectID == TargetUnit?.MyGameObjectID)
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

    void removeTarget(int gameObjectID)
    {
        if (TargetUnit?.MyGameObjectID == gameObjectID)
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
        
        FlipAttackArea(true);
        }
        onFightStart += delegate { AoeController.OnSingleTargetSet(TargetUnit.MyGameObjectID); };
        onFightStart += StartFight;
        
        onFightEnd += EndFight;
        onFightEnd += delegate { AoeController.OnSingleTargetClear(); };

    }
}
