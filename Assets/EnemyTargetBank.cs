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

    public event Action<EnemyUnitController> onConcurrentProximityCheck;
    public void OnConcurrentProximityCheck(EnemyUnitController ec) {
        onConcurrentProximityCheck?.Invoke(ec);
    }

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

    private void ConcurrentProximtyCheck() {
        if (targets.Count >= 1) {
            foreach (var item in targets)
            {
                
                if (CurrentLowestProximtyTargetName == item.Value.name) {
                    continue;
                }
                if (CurrentLowestProximtyTargetName == "NULL") {
                    Debug.LogWarning("There's a target in the targets list while the current lowest proximity target is NULL");
                }
                else if (targets.ContainsKey(CurrentLowestProximtyTargetName)) {
                    if (item.Value.Proximity < targets[CurrentLowestProximtyTargetName].Proximity) {
                        Debug.LogWarning("Target " + item.Value.name + " has a lower proximity than last found target");
                    }
                }
            }
        }
    }

    private void ConcurrentProximityCheckInUpdate() {
        if (targets.Count >= 1) {
            clearNullsFromList();
            if (CurrentLowestProximityTarget == null) {
                CurrentLowestProximityTarget = targets.Values[0];
            }
            foreach (var item in targets)
            {
                if (item.Value.name == CurrentLowestProximityTarget.name) {
                    OnConcurrentProximityCheck(CurrentLowestProximityTarget);
                    continue;
                }
                if (item.Value.Proximity < CurrentLowestProximityTarget.Proximity) {
                    CurrentLowestProximityTarget = item.Value;
                    OnConcurrentProximityCheck(CurrentLowestProximityTarget);
                    Debug.LogWarning("New Target found and replaced old target.");
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (targets.Count >= 1) {
            ConcurrentProximtyCheck();
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
    IEnumerator proxyCheckPH;

    public void initProxyCheck() {
        StopCoroutine(proxyCheckPH);
        StartCoroutine(proxyCheckPH);
    }
    // public IEnumerator LiveProximtyCheck() {
    //     if (targets.Count > 1) {
    //         float currentLowestProximty = 999.0f; 
    //         while (targets.Count > 1) {
    //             foreach (var item in targets)
    //             {
    //                 if (item.Value.Proximity < currentLowestProximty) {
    //                     Debug.LogWarning(item.Value.name + " Has lower Proximity");
    //                 }
    //             }
    //             yield return new WaitForFixedUpdate();
    //         }
    //     }
    //     yield break;
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
        tempEc = other.gameObject.GetComponent<EnemyUnitController>() ?? null;
        AddObjectToTargets(tempEc);
        targetEnteredRange?.Invoke(tempEc);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
        RemoveFromTargetsString(other.name);
        targetLeftRange?.Invoke(other.name);
        }
    }

    public void RemoveFromTargetsString(String name) {
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
