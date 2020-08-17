using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTargetBank : MonoBehaviour
{
    public bool TargetExists() {
        if (targets.Count > 0) {
            return true;
        }
        return false;
    }

    public CircleCollider2D RangeCollider;

    public EnemyUnitController CurrentLowestProximityTarget;
    public string CurrentLowestProximtyTargetName;

    public event Action EnemyWithLowerProximityDetected;
    
    EnemyUnitController tempEc;
    SortedList<string, EnemyUnitController> targets = new SortedList<string, EnemyUnitController>();
    public SortedList<string, EnemyUnitController> Targets {
        get {
            clearNullsFromList();
            return targets;
        }
    }
    public event Action<EnemyUnitController> targetEnteredRange;
    public event Action<string> targetLeftRange;
    public void AddObjectToTargets(EnemyUnitController ec) {
        clearNullsFromList();
        if (ec.CompareTag("Enemy")) {
            if (ec.IsTargetable()) {
                try {
                targets.Add(ec.name, ec);
//                Debug.LogWarning(ec.name);
                CurrentLowestProximityTarget = FindSingleTargetNearestToEndOfSpline();
                }
                catch(Exception e) {
                    Debug.LogWarning(e.Message);
                }
            }

        }
    }

    public void clearNullsFromList() {
        foreach (var item in targets)
        {
            if (item.Value == null) {
                targets.Remove(item.Key);
            }
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
        tempEc = GameObjectPool.Instance.ActiveUnitPool.Pool[other.name] as EnemyUnitController ;
        AddObjectToTargets(tempEc);
        targetEnteredRange?.Invoke(tempEc);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
        RemoveFromTargetsString(other.name,name);
        targetLeftRange?.Invoke(other.name);
        }
    }

    public void RemoveFromTargetsString(String name,string callerName) {
        clearNullsFromList();
        try {
            targets.Remove(name);
            CurrentLowestProximityTarget = FindSingleTargetNearestToEndOfSpline();
        }
        catch (Exception e) {
            Debug.LogWarning(e.Message);
        }
    }

    public void RemoveFromTargetsGO(GameObject go) {
            try {
                targets.Remove(go.name);
//                Debug.LogWarning("TargetRemoved");
            }
            catch(Exception e) {
                Debug.LogWarning(e.Message);
            }
    }

    public EnemyUnitController FindSingleTargetNearestToEndOfSpline() {
        clearNullsFromList();
        EnemyUnitController ec = null;
        float p = 999999.0f;
        foreach (var item in targets)
        {
            float tp = item.Value.Proximity;
            if (tp < p) {
                p = tp;
                ec = item.Value;
            }
        }
        CurrentLowestProximtyTargetName = ec?.name ?? "NULL";
        return ec;
    }

    private void Awake() {
        RangeCollider = GetComponent<CircleCollider2D>();
    }

    void Start() {
        DeathManager.instance.onEnemyUnitDeath += RemoveFromTargetsString;
        
    }

    private void Update() {
        // if (onConcurrentProximityCheck != null) {
        // ConcurrentProximityCheckInUpdate();
        // }
    }

    

    
}
