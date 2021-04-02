using System.Collections.Generic;
using UnityEngine;
using System;
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
        InitRangeDetectorEvents();
    }

    protected void OnDisable()
    {
        DisableRangedetectorEvents();
    }

    [ShowInInspector]
    public bool HasTargetableTargets = false;

    public event Action<string,bool> onTargetableChange;

    void TargetablesCheck(int gameObjectID,bool targetableState)
    {
        if (!Targets.IsNullOrEmpty())
        {
            if (Targets.ContainsKey(gameObjectID))
            {
                if (targetableState)
                {
                    HasTargetableTargets = true;
                    Targets[gameObjectID] = (Targets[gameObjectID].Item1, true);
                    return;
                }

                if (!targetableState)
                {
                    Targets[gameObjectID] = (Targets[gameObjectID].Item1, false);
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
    
    [Required]
    public CollisionDetector Detector;
    public event Action<int> onTryToAddTarget;
    public void OnTryToAddTarget(int targetCollisionID)
    {
        int GID = GameObjectPool.CollisionIDToGameObjectID[targetCollisionID];
        onTryToAddTarget?.Invoke(GID);
    }

    public event Action<int> onTargetRemove;
    public void OnTargetRemove(int targetCollisionID) {
        int GID = GameObjectPool.CollisionIDToGameObjectID[targetCollisionID];
        onTargetRemove?.Invoke(GID);
    }
    
    
    /// <summary>
    /// Add marked by targeter boolean field, which something can do an OnTargetGet mark,
    /// which signifies to other people sharing this target bank that this target has been marked,
    /// you can then do so that attacks that have multiple weapons \ splines \ etc, can only fire or aquire a target
    /// in case it has not been marked as taken, up to you.
    /// </summary>
    ///
    [SerializeReference] 
    public Dictionary<int,(T,bool)> Targets = new Dictionary<int,(T,bool)>();

    public abstract T TryToGetTargetOfType(int gameObjectID);
    
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
                Targets.Add(_gameobjectID, (t,t.IsTargetable()));
                OnTargetAdd(t);
                if (t.IsTargetable())
                {
                    HasTargetableTargets = true;
                }
                HasTargets = true;
            }
        }
    }

    void RemoveTarget(int gameObjectID) {
        clearNulls();
        if (Targets.ContainsKey(gameObjectID)) {
            Targets.Remove(gameObjectID);
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
        onTryToAddTarget += AddTarget;
        onTargetRemove += RemoveTarget;
        GameObjectPool.Instance.onTargetableUpdate += TargetablesCheck;
    }

    public void InitRangeDetectorEvents() {
        Detector.onTargetEnter += OnTryToAddTarget;
        Detector.onTargetExit += OnTargetRemove;
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
        GameObjectPool.Instance.onTargetableUpdate += OnTargetableRemove;
        GameObjectPool.Instance.onObjectDisable += OnTargetRemove;
        PostStart();
    }

    public void OnTargetableRemove(int targetGameObjectID, bool tstate)
    {
        if (!tstate)
        {
            OnTargetRemove(targetGameObjectID);
        }
    }

    public abstract void PostStart();
}
