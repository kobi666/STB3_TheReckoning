using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyUnitController : UnitController
{



    public PlayerUnitController Target { get => Data.PlayerTarget ?? null;}

    public event Action<EnemyUnitController> onAttack;
    public void OnAttack() {
        onAttack?.Invoke(this);
    }
    public abstract event Action onBattleInitiate;
    public abstract void OnBattleInitiate();
      
    // Start is called before the first frame update
    public float Proximity {
        get => Walker.ProximityToEndOfSplineFunc();
    }

    private void PlayAttackAnimation(EnemyUnitController ec) {
        animationController.OnDirectBattleAttack();
    }

    public abstract bool CannotInitiateBattleWithThisUnit();
    public abstract void LateStart();

    private void Start() {
        DeathManager.instance.onPlayerUnitDeath += Data.RemovePlayerUnitTarget;
        if (tag == "Untagged") {
            tag = "Enemy";
        }
        onAttack += PlayAttackAnimation;
        //States.Death.OnEnterState += Unit
        LateStart();
    }
    
    
    
}
