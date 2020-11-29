using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[System.Serializable]
public abstract class TargetBank<T> : SerializedMonoBehaviour where T : Component

{
    private void OnEnable()
    {
        InitRangeDetectorEvents();
    }

    protected void OnDisable()
    {
        DisableRangedetectorEvents();
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
        onTargetsUpdate?.Invoke();
    }
    
    
    /// <summary>
    /// Add marked by targeter boolean field, which something can do an OnTargetGet mark,
    /// which signifies to other people sharing this target bank that this target has been marked,
    /// you can then do so that attacks that have multiple weapons \ splines \ etc, can only fire or aquire a target
    /// in case it has not been marked as taken, up to you.
    /// </summary>
    public Dictionary<string,T> Targets = new Dictionary<string,T>();

    public abstract T TryToGetTargetOfType(GameObject GO);
    
    public event Action<T> onTargetAdd;
    public void OnTargetAdd(T t) {
        onTargetAdd?.Invoke(t);
        onTargetsUpdate?.Invoke();
        Collision c;
        
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
                    onTargetsUpdate?.Invoke();
                }
            }
        }
    }

    

    
    void Awake()
    {
        Detector =  Detector ?? GetComponentInChildren<RangeDetector>() ?? null;
        onTryToAddTarget += AddTarget;
        onTargetRemove += RemoveTarget;
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
