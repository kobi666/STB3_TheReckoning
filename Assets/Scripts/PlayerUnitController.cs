using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnitController : UnitController
{
    public event Action reachedBattlePosition;
    public void ReachedBattlePosition() {
        if (reachedBattlePosition != null) {
            reachedBattlePosition.Invoke();
        }
    }
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
    public event Action onTargetCheck;
    public void OnTargetCheck() {
        if (onTargetCheck != null) {
            onTargetCheck.Invoke();
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
