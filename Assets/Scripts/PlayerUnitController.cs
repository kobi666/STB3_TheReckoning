using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnitController : MonoBehaviour
{
    public UnitType unitType;
    public NormalUnitStates states {get => unitType.States;}
    public int AttackRate;
    public DamageRange DamageRange;
    public StateMachine SM;
    public int setHP;
    public int setArmor;
    public int SetSpecialArmor;
    UnitLifeManager UnitLife;
    public int speed;
    
    
    public Collider2D [] collisions;

    // public void SetEnemyTarget() {
    //     EnemyTarget = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
    //     //Debug.Log("Player Unit Found Enemy " + EnemyTarget.name);
    // }
    // Start is called before the first frame update
    public event Action _onTargetCheck;
    public void OnTargetCheck() {
        if (_onTargetCheck != null) {
            _onTargetCheck.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        OnTargetCheck();
    }

    private void OnTriggerExit2D(Collider2D other) {
        OnTargetCheck();
    }

    private void Awake() {
    SM = GetComponent<StateMachine>();
    UnitLife = new UnitLifeManager(setHP, setArmor, SetSpecialArmor);
    }


}
