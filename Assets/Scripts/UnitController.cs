using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public UnitLifeManager LifeManager;
    public NormalUnitStates States;
    public BezierSolution.UnitWalker Walker;
    public EnemyTargetBank TargetBank;
    public StateMachine SM;

    [SerializeField]
    public UnitData Data;

    public UnitState CurrentState {
        get => SM?.CurrentState ?? null;
    }

    public event Action<string> onTargetIdentified;
    public void OnTargetIdentified(string targetName) {
        onTargetIdentified?.Invoke(targetName);
    }
    
    public event Action onSelfDeath;
    public void OnSelfDeath() {
        onSelfDeath?.Invoke();
    }

    void AnnounceDeath() {
        DeathManager.instance.OnUnitDeath(tag, name);
    }

    
    
    private void Awake() {
        LifeManager.onUnitDeath += AnnounceDeath;
        TargetBank = GetComponentInChildren<EnemyTargetBank>();
        SM = GetComponent<StateMachine>() ?? null;
        States = new NormalUnitStates(this);
        Walker = GetComponent<BezierSolution.UnitWalker>() ?? null;
        Data = new UnitData();
    }

    

    
}
