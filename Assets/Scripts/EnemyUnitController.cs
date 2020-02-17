using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitController : UnitController
{
    
   public BezierSolution.UnitWalker walker;
   public StateMachine SM;
   public GameObject targetPlayerUnit;
    public int setHP;
    public int setArmor;
    public int SetSpecialArmor;
    
    public UnitLifeManager UnitLife;

    public bool InBattleWithUnit() {
        if (targetPlayerUnit != null) {
            return true;
        }
        else 
        { 
            return false;
        }
    }

    public void UnitDeath() {
        SM.SetState(states.Death);
    }

    
    
    //Display purpose only
    [SerializeField]
    public int HP;
    //Display purpose only
    [SerializeField]
    int ARMOR;

     private void Awake() {
        
        UnitLife = new UnitLifeManager(setHP, setArmor, SetSpecialArmor);
        walker = GetComponent<BezierSolution.UnitWalker>();
        SM = GetComponent<StateMachine>();
        unitType = new UnitType(this, SM);
        //SM.InitilizeStateMachine(unitType.States);
        //SM.InitilizeStateMachine(UnitTypes.NormalEnemy(this).States);
        // UnitLife._onUnitDeath += UnitDeath;
        // SM.States["Death"].OnEnterState += Die;
        // SM.States["PreBattle"].OnEnterState += StopWalkingOnPath;
        // SM.States["Default"].OnEnterState += ReturnToWalkPath;
        // SM.States["InBattle"].OnEnterState += StartBattle;
     }

    private void Update() {
        HP = UnitLife.HP;
        ARMOR = UnitLife.Armor;
    }
    
}
