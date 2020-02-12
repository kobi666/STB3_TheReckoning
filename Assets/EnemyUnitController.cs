using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitController : MonoBehaviour
{
    UnitState[] UnitStates;
    BezierSolution.UnitWalker walker;
   public string[] states = new string[5];
   public StateMachine SM;
   public UnitState CreateState(string _stateName, bool _isfinal) {
       return new UnitState(_isfinal, _stateName, this);
   }

   

   
    public int setHP;
    
    public UnitLifeManager UnitLife;
    
    //Display purpose only
    [SerializeField]
     int HP;
    //Display purpose only
    [SerializeField]
     int ARMOR;

     private void Awake() {
         UnitLife = new UnitLifeManager(setHP, 1,0);
     }

    public IEnumerator Die() {
        Destroy(gameObject);
        yield break;
    }

    public IEnumerator test() {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Success!");
    }

    public virtual void UnitDeath() {
        SM.SetState("Death");
    }

    public virtual void GoIntoBattleState() {
        SM.SetState("InBattle");
    }

    public virtual void goInto_Pre_BattleState() {
        SM.SetState("PreBattle");
    }

    IEnumerator ReturnToWalkPath() {
        walker.IsWalking = true;
        yield break;
    }

    IEnumerator StopWalkingOnPath() {
        walker.IsWalking = false;
        yield break;
    }

    IEnumerator Battle() {
        Debug.Log("I'm in battle!");
        yield break;
    }

    

    
    
    

    private void Start() {
        walker = GetComponent<BezierSolution.UnitWalker>();
        SM = GetComponent<StateMachine>();
        UnitStates = UnitTypes.normal(this);
        SM.InitilizeStateMachine(UnitTypes.normal(this));
        UnitLife._onUnitDeath += UnitDeath;
        SM.States["Death"].OnEnterState += Die;
        SM.States["PreBattle"].OnEnterState += StopWalkingOnPath;
        SM.States["Default"].OnEnterState += ReturnToWalkPath;
        SM.States["InBattle"].OnEnterState += Battle;
    }

    private void Update() {
        HP = UnitLife.HP;
        ARMOR = UnitLife.Armor;
    }
    
}
