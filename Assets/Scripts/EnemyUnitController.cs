using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitController : MonoBehaviour
{
   public UnitType unitType;
   public BezierSolution.UnitWalker walker;
   public StateMachine SM;
   public GameObject TargetPlayerUnit = null;
    public int setHP;
    public int setArmor;
    public int SetSpecialArmor;
    
    public UnitLifeManager UnitLife;

    public void UnitDeath() {
        SM.SetState("Death");
    }

    public virtual IEnumerator Die() {
        Destroy(gameObject);
        yield break;
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
