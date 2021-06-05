using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Concurrent;
using MyBox;
using Sirenix.OdinInspector;
using UnityEditor;

[System.Serializable]
public abstract class TargetBank<T> : MonoBehaviour where T : ITargetable,IhasGameObjectID

{
#if UNITY_EDITOR
    [TagSelector]
# endif
    [SerializeField]
    public List<string> DiscoverableTags;
    
    
    
    [Required][SerializeField] public MyGameObject parentMyGameObject;
    public int MyGameObjectID { get => parentMyGameObject.MyGameObjectID; }
    [ShowInInspector]
    public bool HasTargets;
    private void OnEnable()
    {
        
    }

    protected void OnDisable()
    {
        //DisableRangedetectorEvents();
        Targets.Clear();
    }

    [ShowInInspector]
    public bool HasTargetableTargets = false;

    public event Action<string,bool> onTargetableChange;

    void TargetablesCheck(int gameObjectID,bool targetableState,string callerName)
    {
        if (Targets.IsNullOrEmpty())
        {
            HasTargets = false;
            HasTargetableTargets = false;
            return;
        }

        foreach (var target in Targets)
        {
            if (target.Value.Item1.IsTargetable())
            {
                HasTargets = true;
                HasTargetableTargets = true;
                return;
            }
        }
        
    }
    

    public event Action onTargetsUpdate;
    
    [Required]
    public CollisionDetector Detector;
    public event Action<int> onTryToAddTarget;
    public void OnTryToAddTarget(int targetCollisionID)
    {
        int GID = GameObjectPool.CollisionIDToGameObjectID[targetCollisionID].Item1;
        onTryToAddTarget?.Invoke(GID);
    }

    public event Action<int> onTargetRemove;
    public void OnTargetRemoveFromCollision(int targetCollisionID,string callerName) {
        //Debug.LogWarning(name + " : " + "collision ID " + targetCollisionID + "  removed by " + callerName + " Object Name : " + GameObjectPool.CollisionIDToGameObjectID[targetCollisionID].Item2);
        int GID = GameObjectPool.CollisionIDToGameObjectID[targetCollisionID].Item1;
        onTargetRemove?.Invoke(GID);
    }

    public void OnTargetRemoveGID(int targetGameObjectID, string callerName)
    {
        if (Targets.ContainsKey(targetGameObjectID)) {
        //Debug.LogWarning(name + " : " + "GID " + targetGameObjectID + " removed by " + callerName);
        onTargetRemove?.Invoke(targetGameObjectID);
        }
    }
    
    
    /// <summary>
    /// Add marked by targeter boolean field, which something can do an OnTargetGet mark,
    /// which signifies to other people sharing this target bank that this target has been marked,
    /// you can then do so that attacks that have multiple weapons \ splines \ etc, can only fire or aquire a target
    /// in case it has not been marked as taken, up to you.
    /// </summary>
    ///
    [SerializeReference] 
    public ConcurrentDictionary<int,(T,bool)> Targets = new ConcurrentDictionary<int,(T,bool)>();

    public abstract T TryToGetTargetOfType(int gameObjectID);
    
    public event Action<T> onTargetAdd;
    public void OnTargetAdd(T t) {
        onTargetAdd?.Invoke(t);
        onTargetsUpdate?.Invoke();
    }

    private List<string> excludedNames = new List<string>();

    public void AddNameExclusion(string objectName)
    {
        excludedNames.Add(objectName);
    }
    

    void AddTarget(int _gameobjectID) {
        if (_gameobjectID == MyGameObjectID)
        {
            return;
        }
        clearNulls();
        T t = TryToGetTargetOfType(_gameobjectID);
        if (t != null) {
            if (!Targets.ContainsKey(_gameobjectID))
            {
                Targets.TryAdd(_gameobjectID, (t,t.IsTargetable()));
                OnTargetAdd(t);
                if (t.IsTargetable())
                {
                    HasTargetableTargets = true;
                }
                HasTargets = true;
            }
        }
    }


    public bool debugMe;
    void RemoveTarget(int gameObjectID) {
        clearNulls();
        if (Targets.ContainsKey(gameObjectID)) {
            Targets.TryRemove(gameObjectID,out outValue);
            if (Targets.IsNullOrEmpty())
            {
                HasTargets = false;
                HasTargetableTargets = false;
            }
        }
    }

    private (T, bool) outValue;
    void clearNulls() {
        if (Targets.Count > 0) {
            foreach (var item in Targets)
            {
                if (item.Value.Item1 == null) {
                    Targets.TryRemove(item.Key,out outValue);
                    onTargetsUpdate?.Invoke();
                }
            }
            
        }
    }

    

    
    

    public void InitRangeDetectorEvents() {
        Detector.onTargetEnter += OnTryToAddTarget;
        Detector.onTargetExit += OnTargetRemoveFromCollision;
        Detector.ResetCollisions();
    }

    void DisableRangedetectorEvents()
    {
        if (Detector != null)
        {
            Detector.onTargetEnter -= OnTryToAddTarget;
            Detector.onTargetExit -= OnTargetRemoveFromCollision;
        }
    }
    // Start is called before the first frame update
    protected void Start()
    {
        InitRangeDetectorEvents();
        excludedNames.Add(name);
        onTryToAddTarget += AddTarget;
        onTargetRemove += RemoveTarget;
        GameObjectPool.Instance.onTargetableUpdate += TargetablesCheck;
        GameObjectPool.Instance.onTargetableUpdate += OnTargetableRemove;
        GameObjectPool.Instance.onObjectDisable += OnTargetRemoveGID;
        PostStart();
    }

    public void OnTargetableRemove(int targetGameObjectID, bool tstate, string callerName)
    {
        if (!tstate)
        {
            OnTargetRemoveGID(targetGameObjectID,callerName);
        }
    }

    public abstract void PostStart();
}
