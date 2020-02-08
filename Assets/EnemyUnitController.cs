using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitController : MonoBehaviour
{
    public int setHP;
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

    private void Start() {
        _UnitStats._onUnitDeath += Die;
    }

    private void Update() {
        HP = _UnitStats.HP;
        ARMOR = _UnitStats.Armor;
    }
    
}
