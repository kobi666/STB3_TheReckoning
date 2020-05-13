using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyUnitController : UnitController
{


    public PlayerUnitController Target { get => Data.PlayerTarget ?? null;}

    public abstract event Action<EnemyUnitController> onAttack;
    public abstract event Action onBattleInitiate;
    public abstract void OnBattleInitiate();
      
    // Start is called before the first frame update
    public float Proximity {
        get => Walker.ProximityToEndOfSplineFunc();
    }

    public abstract bool CannotInitiateBattleWithThisUnit();
    public abstract void LateStart();

    private void Start() {
        LateStart();
    }
    
    
    
}
