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
    Dictionary <string,T> targets = new Dictionary<string, T>();
    public Dictionary<string,T> Targets {
        get {
            clearNulls();
            return targets;
        }
    }

    T TryToGetTargetOfType(GameObject GO) {
        T t = null;
        try {
            t = GO.GetComponent<T>();
        }
        catch (Exception e) {
            Debug.LogWarning(e.Message);
        }
        if (t != null) {
            return t;
        }
        return t;
    }

    void AddTarget(GameObject targetGO) {
        T t = TryToGetTargetOfType(targetGO);
        if (t != null) {
            Targets.Add(targetGO.name, t);
        }
    }

    void RemoveTarget(String targetName) {
        if (Targets.ContainsKey(targetName)) {
            Targets.Remove(targetName);
        }
    }

    void clearNulls() {
        if (targets.Count > 0) {
            foreach (var item in targets)
            {
                if (item.Value == null) {
                    targets.Remove(item.Key);
                }
            }
        }
    }

    
    void Awake()
    {
        rangeDetector = transform.parent.GetComponent<RangeDetector>() ?? GetComponent<RangeDetector>() ?? null;
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
