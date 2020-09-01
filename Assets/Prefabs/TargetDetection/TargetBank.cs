using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class TargetBank<T> : MonoBehaviour where T : Component

{
    private void OnEnable()
    {
        InitRangeDetectorEvents();
    }

    protected void OnDisable()
    {
        DisableRangedetectorEvents();
    }


    [SerializeField]
    public List<string> debugTargetNames = new List<string>();
    public RangeDetector RangeDetector;
    public event Action<GameObject> onTryToAddTarget;
    public void OnTryToAddTarget(GameObject targetGO) {
        onTryToAddTarget?.Invoke(targetGO);
    }

    public event Action<string,string> onTargetRemove;
    public void OnTargetRemove(string targetName, string callerName) {
        onTargetRemove?.Invoke(targetName, callerName);
    }
    
    public Dictionary<string,T> Targets = new Dictionary<string,T>();

    public abstract T TryToGetTargetOfType(GameObject GO);
    
    public event Action<T> onTargetAdd;
    public void OnTargetAdd(T t) {
        onTargetAdd?.Invoke(t);
    }
    

    void AddTarget(GameObject targetGO) {
        clearNulls();
        T t = TryToGetTargetOfType(targetGO);
        if (t != null) {
            
            if (!Targets.ContainsKey(targetGO.name))
            {
                Targets.Add(targetGO.name, t);
                OnTargetAdd(t);
            }
        }
    }

    void RemoveTarget(String targetName, string callerName) {
        clearNulls();
        if (Targets.ContainsKey(targetName)) {
            Targets.Remove(targetName);
            //Debug.LogWarning(name + " : " + "Target " + targetName + " was removed by" + callerName  );
        }
    }

    void clearNulls() {
        if (Targets.Count > 0) {
            foreach (var item in Targets)
            {
                if (item.Value == null) {
                    Targets.Remove(item.Key);
                }
            }
        }
    }

    void AddNamesToDebugList(T t) {
        debugTargetNames.Add(t.name);
    }

    void removeNameFromDebugList(string s, string callerName) {
        debugTargetNames.Remove(s);
    }

    
    void Awake()
    {
        RangeDetector =  RangeDetector ?? GetComponentInChildren<RangeDetector>() ?? null;
        onTryToAddTarget += AddTarget;
        onTargetRemove += RemoveTarget;
        onTargetAdd += AddNamesToDebugList;
        onTargetRemove += removeNameFromDebugList;
    }

    public void InitRangeDetectorEvents() {
        if (RangeDetector == null)
        {
            RangeDetector = GetComponentInChildren<RangeDetector>();
        }
        
        if (RangeDetector != null) {
        RangeDetector.onTargetEnter += OnTryToAddTarget;
        RangeDetector.onTargetExit += OnTargetRemove;
        }
    }

    void DisableRangedetectorEvents()
    {
        if (RangeDetector != null)
        {
            RangeDetector.onTargetEnter -= OnTryToAddTarget;
            RangeDetector.onTargetExit -= OnTargetRemove;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //InitRangeDetectorEvents();
        DeathManager.instance.onEnemyUnitDeath += OnTargetRemove;
        DeathManager.instance.onPlayerUnitDeath += OnTargetRemove;
        GameObjectPool.Instance.onObjectDisable += OnTargetRemove;
        PostStart();
    }

    public abstract void PostStart();
}
