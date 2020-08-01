using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(RangeDetector))]
public abstract class TargetBank<T> : MonoBehaviour where T : Component
{
    RangeDetector rangeDetector;
    public event Action<GameObject> onTargetAdd;
    public void OnTargetAdd(GameObject targetGO) {
        onTargetAdd?.Invoke(targetGO);
    }

    public event Action<string> onTargetRemove;
    public void OnTargetRemove(string targetName) {
        onTargetRemove?.Invoke(targetName);
    }
    Dictionary <string,T> targets;
    public Dictionary<string,T> Targets {
        get {
            clearNulls();
            return targets;
        }
    }

    T TryToGetTargetOfType(GameObject GO) {
        T t = null;
        try {
            t = GO.GetComponent<T>() ?? null;
        }
        catch (Exception e) {
            Debug.LogWarning(e.Message);
        }
        return t;
    }

    void AddTarget(GameObject targetGO) {
        T t = TryToGetTargetOfType(targetGO);
        if (t != null) {
            Targets.Add(t.name, t);
        }
    }

    void RemoveTarget(String targetName) {
        if (Targets.ContainsKey(targetName)) {
            Targets.Remove(targetName);
        }
    }

    void clearNulls() {
        foreach (var item in targets)
        {
            if (item.Value == null || item.Value.gameObject.activeSelf == false) {
                targets.Remove(item.Key);
            }
        }
    }

    
    void Awake()
    {
        rangeDetector = GetComponent<RangeDetector>() ?? null;
        onTargetAdd += AddTarget;
        onTargetRemove += RemoveTarget;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        rangeDetector.onTargetEnter += OnTargetAdd;
        rangeDetector.onTargetExit += OnTargetRemove;

        PostStart();
    }

    public abstract void PostStart();
}
