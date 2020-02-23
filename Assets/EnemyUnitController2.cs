using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController2 : MonoBehaviour
{
    public UnitType unitType;
    public UnitData Data;
    public UnitLifeManager LifeManager;
    public StateMachine SM;
    public NormalUnitStates states {get => unitType.states;}

    public void ChangeDisplayStats() {
        Data.HP = LifeManager.HP;
        Data.Armor = LifeManager.Armor;
        Data.SpecialArmor = LifeManager.SpecialArmor;
    }

    string currentStateName {
        get => SM.CurrentState.stateName;
    }
    public IEnumerator EmptyCoroutine() {
        Debug.Log("Currnet State : " + currentStateName);
        yield break;
    }

    public StateMachine TargetStateMachine { get => Data.Target.GetComponent<StateMachine>();}
    // Start is called before the first frame update
    void Start()
    {
        SM = GetComponent<StateMachine>();
        unitType = new UnitType(this, SM);
        LifeManager = new UnitLifeManager(Data.HP, Data.Armor, Data.SpecialArmor);
        LifeManager.hp_changed += ChangeDisplayStats;

        states.Default.OnEnterState += EmptyCoroutine;
        states.Default.OnExitState += EmptyCoroutine;

        states.PreBattle.OnEnterState += EmptyCoroutine;
        states.PreBattle.OnExitState += EmptyCoroutine;

        states.InBattle.OnEnterState += EmptyCoroutine;
        states.InBattle.OnExitState += EmptyCoroutine;

        states.Death.OnEnterState += EmptyCoroutine;
        states.Death.OnExitState += EmptyCoroutine;

        SM.SetState(states.Default);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
