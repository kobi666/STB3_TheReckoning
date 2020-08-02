using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class GenericTargetBank<T> : MonoBehaviour, ITargetBank<T> where T : Component
{

public abstract string DiscoveryTag {get;}
    public bool TargetExists() {
        if (targets.Count > 0) {
            return true;
        }
        return false;
    }

    public CircleCollider2D RangeCollider;
    T placeholderT;
    SortedList<string, T> targets = new SortedList<string, T>();
    public SortedList<string, T> Targets {
        get {
            clearNullsFromList();
            return targets;
        }
    }
    public event Action<T> targetEnteredRange;
    public event Action<string> targetLeftRange;
    
    public event Action<T> onTargetAdd;
    public void OnTargetAdd(T t) {
        onTargetAdd?.Invoke(t);
    }
    

    public abstract bool TargetableCondition(T t);
    public void AddObjectToTargets(T t) {
        clearNullsFromList();
        if (t.CompareTag(DiscoveryTag)) {
            if (TargetableCondition(t)) {
                try {
                targets.Add(t.name, t);
//                Debug.LogWarning(ec.name);
                OnTargetAdd(t);
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
            if (item.Value?.gameObject.activeSelf == false) {
                targets.Remove(item.Key);
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag(DiscoveryTag)) {
        placeholderT = other.gameObject.GetComponent<T>() ?? null;
        AddObjectToTargets(placeholderT);
        targetEnteredRange?.Invoke(placeholderT);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag(DiscoveryTag)) {
        RemoveFromTargetsString(other.name);
        targetLeftRange?.Invoke(other.name);
        }
    }

    public void RemoveFromTargetsString(String name) {
        clearNullsFromList();
        try {
            targets.Remove(name);
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

    public abstract T GetFirstPriorityTarget(SortedList<string,T> targetsList);

    private void Awake() {
        RangeCollider = GetComponent<CircleCollider2D>();
    }

    

    


    
}
