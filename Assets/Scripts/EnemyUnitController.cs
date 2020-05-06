using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyUnitController : UnitController
{
    public PlayerUnitController Target { get => Data.PlayerTarget ?? null;}
    event Action initiateBattle;
    public void InitiateBattle() {
        initiateBattle?.Invoke();
    }
    // Start is called before the first frame update
    public float Proximity {
        get => Walker.ProximityToEndOfSplineFunc();
    }

    public abstract bool CannotInitiateBattleWithThisUnit();
    public abstract void LateStart();

    
    
}
