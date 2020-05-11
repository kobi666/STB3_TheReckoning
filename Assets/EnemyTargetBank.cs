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
    EnemyUnitController tempEc;
    SortedList<string, EnemyUnitController> targets = new SortedList<string, EnemyUnitController>();
    public event Action<EnemyUnitController> targetEnteredRange;
    public event Action<string> targetLeftRange;
    public void AddObjectToTargets(EnemyUnitController ec) {
        if (ec.CompareTag("Enemy")) {
            if (ec.IsTargetable()) {
                try {
                targets.Add(ec.name, ec);
                Debug.LogWarning("TargetAdded");
                }
                catch(Exception e) {
                    Debug.LogWarning(e.Message);
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

    private void OnTriggerEnter2D(Collider2D other) {
        tempEc = other.gameObject.GetComponent<EnemyUnitController>() ?? null;
        AddObjectToTargets(tempEc);
        targetEnteredRange?.Invoke(tempEc);
    }

    private void OnTriggerExit2D(Collider2D other) {
        RemoveObjectFromTargets(other.gameObject);
        targetLeftRange?.Invoke(other.name);
    }

    

    public void RemoveObjectFromTargets(GameObject go) {
        if (go.CompareTag("Enemy")) {
            try {
                targets.Remove(go.name);
                Debug.LogWarning("TargetRemoved");
            }
            catch(Exception e) {
                Debug.LogWarning(e.Message);
            }
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

    

    
}
