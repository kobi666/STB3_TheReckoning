using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTargetBank : MonoBehaviour
{
    
    SortedList<string, EnemyUnitController> targets = new SortedList<string, EnemyUnitController>();
    public event Action<string> targetEnteredRange;
    public event Action<string> targetLeftRange;
    public void AddObjectToTargets(GameObject go) {
        if (go.CompareTag("Enemy")) {
            try {
            targets.Add(go.name, go.GetComponent<EnemyUnitController>());
            Debug.LogWarning("TargetAdded");
            }
            catch(Exception e) {
                Debug.LogWarning(e.Message);
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
        AddObjectToTargets(other.gameObject);
        targetEnteredRange?.Invoke(other.name);
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

    private void Awake() {
        
    }

    
}
