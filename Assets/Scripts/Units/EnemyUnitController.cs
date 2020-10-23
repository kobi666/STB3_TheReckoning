﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyUnitController : UnitController,ITypeTag
{
    public bool ShouldGiveMoneyzOnDeath = true;
    public int MoneyzAmount;
    [NonSerialized]
    public static string Tag = "Enemy";
    public string TypeTag {get => Tag;}
    
    public PlayerUnitController Target { get => Data.PlayerTarget ?? null; set { Data.PlayerTarget = value;}}

    public event Action<EnemyUnitController> onAttack;
    public void OnAttack() {
        onAttack?.Invoke(this);
    }
    public event Action onBattleInitiate;
    public void OnBattleInitiate() {
        onBattleInitiate?.Invoke();
    }

    void SetXDirectionInBattle() {
        if (Target?.transform.position.x > transform.position.x) {
            SetXdirection(false);
        }
        if (Target?.transform.position.x < transform.position.x) {
            SetXdirection(true);
        }
    }
      
    // Start is called before the first frame update
    

    private void PlayAttackAnimation(EnemyUnitController ec) {
        animationController.OnDirectBattleAttack();
    }

    public abstract bool CannotInitiateBattleWithThisUnit();
    public abstract void LateStart();

    void UpdateMoneyzOnDeath()
    {
        if (ShouldGiveMoneyzOnDeath)
        {
            PlayerResources.instance.UpdateMoneyz(MoneyzAmount);
        }
    }
    
    private void Start()
    {
        LifeManager.onUnitDeath += UpdateMoneyzOnDeath;
        gameObject.tag = TypeTag;
        DeathManager.instance.onPlayerUnitDeath += Data.RemovePlayerUnitTarget;
        if (tag == "Untagged") {
            tag = Tag;
        }
        onAttack += PlayAttackAnimation;
        onBattleInitiate += SetXDirectionInBattle;
        //States.Death.OnEnterState += Unit
        LateStart();
    }
    
    
    
}