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

    public EnemyUnitController CurrentLowestProximityTarget;

    public event Action EnemyWithLowerProximityDetected;
    
    EnemyUnitController tempEc;
    public SortedList<string, EnemyUnitController> targets = new SortedList<string, EnemyUnitController>();
    public event Action<EnemyUnitController> targetEnteredRange;
    public event Action<string> targetLeftRange;
    public void AddObjectToTargets(EnemyUnitController ec) {
        clearNullsFromList();
        if (ec.CompareTag("Enemy")) {
            if (ec.IsTargetable()) {
                try {
                targets.Add(ec.name, ec);
                Debug.LogWarning(ec.name);
                CurrentLowestProximityTarget = FindSingleTargetNearestToEndOfSpline();
                }
                catch(Exception e) {
                    Debug.LogWarning(e.Message);
                }
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (targets.Count > 1) {
            foreach (EnemyUnitController ec in targets.Values)
            {
                if (ec.Proximity < CurrentLowestProximityTarget.Proximity) {
                    Debug.LogWarning("New target has lower Proximity:: " + ec.name);
                    //targetEnteredRange(ec);
                }
            }
        }
    }

    private void clearNullsFromList() {
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
        return ec;
    }

    void Start() {
        DeathManager.instance.onEnemyUnitDeath += RemoveFromTargetsString;
    }

    

    
}
