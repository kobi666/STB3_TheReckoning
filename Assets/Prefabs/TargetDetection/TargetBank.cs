using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[System.Serializable]
public abstract class TargetBank<T> : MonoBehaviour where T : ITargetable

{

    public bool HasTargets;
    private void OnEnable()
    {
        InitRangeDetectorEvents();
    }

    protected void OnDisable()
    {
        DisableRangedetectorEvents();
    }


    public bool HasTargetableTargets = false;

    public event Action<string,bool> onTargetableChange;

    void TargetablesCheck(string targetableName,bool targetableState)
    {
        if (!Targets.IsNullOrEmpty())
        {
            if (Targets.ContainsKey(targetableName))
            {
                if (targetableState)
                {
                    HasTargetableTargets = true;
                    Targets[targetableName] = (Targets[targetableName].Item1, true);
                    return;
                }

                if (!targetableState)
                {
                    Targets[targetableName] = (Targets[targetableName].Item1, false);
                }

                foreach (var target in Targets)
                {
                    if (target.Value.Item2)
                    {
                        HasTargetableTargets = true;
                        return;
                    }
                }
            }
        }
        HasTargetableTargets = false;
        
    }
    

    public event Action onTargetsUpdate;
    
    [FormerlySerializedAs("RangeDetector")] public TagDetector Detector;
    public event Action<GameObject> onTryToAddTarget;
    public void OnTryToAddTarget(GameObject targetGO) {
        onTryToAddTarget?.Invoke(targetGO);
    }

    public event Action<string,string> onTargetRemove;
    public void OnTargetRemove(string targetName, string callerName) {
        onTargetRemove?.Invoke(targetName, callerName);
    }
    
    
    /// <summary>
    /// Add marked by targeter boolean field, which something can do an OnTargetGet mark,
    /// which signifies to other people sharing this target bank that this target has been marked,
    /// you can then do so that attacks that have multiple weapons \ splines \ etc, can only fire or aquire a target
    /// in case it has not been marked as taken, up to you.
    /// </summary>
    ///
    [SerializeReference] 
    public Dictionary<string,(T,bool)> Targets = new Dictionary<string,(T,bool)>();

    public abstract T TryToGetTargetOfType(GameObject GO);
    
    public event Action<T> onTargetAdd;
    public void OnTargetAdd(T t) {
        onTargetAdd?.Invoke(t);
        onTargetsUpdate?.Invoke();
        Collision c;
    }

    private List<string> excludedNames = new List<string>();

    public void AddNameExclusion(string objectName)
    {
        excludedNames.Add(objectName);
    }
    

    void AddTarget(GameObject targetGO) {
        if (targetGO.name == name)
        {
            return;
        }
        clearNulls();
        T t = TryToGetTargetOfType(targetGO);
        if (t != null) {
            if (!Targets.ContainsKey(targetGO.name))
            {
                Targets.Add(targetGO.name, (t,t.IsTargetable()));
                OnTargetAdd(t);
                if (t.IsTargetable())
                {
                    HasTargetableTargets = true;
                }
                HasTargets = true;
            }
        }
    }

    void RemoveTarget(String targetName, string callerName) {
        clearNulls();
        if (Targets.ContainsKey(targetName)) {
            Targets.Remove(targetName);
            if (Targets.IsNullOrEmpty())
            {
                HasTargets = false;
                HasTargetableTargets = false;
            }
        }
    }

    void clearNulls() {
        if (Targets.Count > 0) {
            foreach (var item in Targets)
            {
                if (item.Value.Item1 == null) {
                    Targets.Remove(item.Key);
                    onTargetsUpdate?.Invoke();
                }
            }
            
        }
    }

    

    
    void Awake()
    {
        excludedNames.Add(name);
        Detector =  Detector ?? GetComponentInChildren<RangeDetector>() ?? null;
        onTryToAddTarget += AddTarget;
        onTargetRemove += RemoveTarget;
        GameObjectPool.Instance.onTargetableUpdate += TargetablesCheck;
    }

    public void InitRangeDetectorEvents() {
        if (Detector == null)
        {
            Detector = GetComponentInChildren<RangeDetector>();
        }
        
        if (Detector != null) {
        Detector.onTargetEnter += OnTryToAddTarget;
        Detector.onTargetExit += OnTargetRemove;
        }
    }

    void DisableRangedetectorEvents()
    {
        if (Detector != null)
        {
            Detector.onTargetEnter -= OnTryToAddTarget;
            Detector.onTargetExit -= OnTargetRemove;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //InitRangeDetectorEvents();
        DeathManager.Instance.onEnemyUnitDeath += OnTargetRemove;
        DeathManager.Instance.onPlayerUnitDeath += OnTargetRemove;
        GameObjectPool.Instance.onObjectDisable += OnTargetRemove;
        PostStart();
    }

    public abstract void PostStart();
}
