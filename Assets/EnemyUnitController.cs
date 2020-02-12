using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitController : MonoBehaviour
{
    
    public UnitState US;
    public int setHP;
    StateMachine SM;
    public UnitStats _UnitStats;
    
    //Display purpose only
    [SerializeField]
     int HP;
    //Display purpose only
    [SerializeField]
     int ARMOR;

     private void Awake() {
         _UnitStats = new UnitStats(setHP, 1,0);
     }

    public void Die() {
        Destroy(gameObject);
    }

    public IEnumerator test() {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Success!");
    }



    private void Start() {
        _UnitStats._onUnitDeath += Die;
        
        SM = gameObject.GetComponent<StateMachine>();
        US = new UnitState(false, "US", this);
        US.OnEnterState += test;
        SM.SetState(US);
        
    }

    private void Update() {
        HP = _UnitStats.HP;
        ARMOR = _UnitStats.Armor;
    }
    
}
