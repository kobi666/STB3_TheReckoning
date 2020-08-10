using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class TargetBank<T> : MonoBehaviour where T : Component

{
    [SerializeField]
    public List<string> debugTargetNames = new List<string>();
    public RangeDetector rangeDetector;
    public event Action<GameObject> onTryToAddTarget;
    public void OnTryToAddTarget(GameObject targetGO) {
        onTryToAddTarget?.Invoke(targetGO);
    }

    public event Action<string> onTargetRemove;
    public void OnTargetRemove(string targetName) {
        onTargetRemove?.Invoke(targetName);
    }
    Dictionary <string,T> targets = new Dictionary<string, T>();
    public Dictionary<string,T> Targets {
        get {
            //clearNulls();
            return targets;
        }
    }

    public abstract T TryToGetTargetOfType(GameObject GO);
    
    public event Action<T> onTargetAdd;
    public void OnTargetAdd(T t) {
        onTargetAdd?.Invoke(t);
    }
    

    void AddTarget(GameObject targetGO) {
        T t = TryToGetTargetOfType(targetGO);
        if (t != null) {
            Targets.Add(targetGO.name, t);
            OnTargetAdd(t);
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

    void AddNamesToDebugList(T t) {
        debugTargetNames.Add(t.name);
    }

    void removeNameFromDebugList(string s) {
        debugTargetNames.Remove(s);
    }

    
    void Awake()
    {
        //rangeDetector =  GetComponent<RangeDetector>() ?? GetComponentInChildren<RangeDetector>() ?? null;
        onTryToAddTarget += AddTarget;
        onTargetRemove += RemoveTarget;
        onTargetAdd += AddNamesToDebugList;
        onTargetRemove += removeNameFromDebugList;
        
    }

    public void InitRangeDetectorEvents() {
        if (rangeDetector != null) {
        rangeDetector.onTargetEnter += OnTryToAddTarget;
        rangeDetector.onTargetExit += OnTargetRemove;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitRangeDetectorEvents();
        DeathManager.instance.onEnemyUnitDeath += OnTargetRemove;
        DeathManager.instance.onPlayerUnitDeath += OnTargetRemove;
        GameObjectPool.Instance.onObjectDisable += OnTargetRemove;
        PostStart();
    }

    public abstract void PostStart();
}
