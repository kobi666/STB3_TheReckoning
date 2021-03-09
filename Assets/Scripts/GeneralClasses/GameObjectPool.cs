using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;
using MyBox;


[DefaultExecutionOrder(-1)]
public class GameObjectPool : MonoBehaviour
{
    public Dictionary<string, PoolObjectQueue<RangeDetector>> RangeDetectorObjectPoolQueue = new Dictionary<string, PoolObjectQueue<RangeDetector>>();
    public Dictionary<string, PoolObjectQueue<EnemyUnitController>> EnemyUnitObjectPoolQueue = new Dictionary<string, PoolObjectQueue<EnemyUnitController>>();
    public Dictionary<string, PoolObjectQueue<PathDiscoveryPoint>> PathDiscoveryPointObjectPoolQueue = new Dictionary<string, PoolObjectQueue<PathDiscoveryPoint>>();

    public Dictionary<string, PoolObjectQueue<GenericProjectile>> GenericProjectilesObjectPoolQueue = new Dictionary<string, PoolObjectQueue<GenericProjectile>>();
    
    public Dictionary<string, PoolObjectQueue<AreaOfEffectController>> AOEObjectPoolQueue = new Dictionary<string, PoolObjectQueue<AreaOfEffectController>>();
    public Dictionary<string,PoolObjectQueue<SingleAnimationObject>> SingleAnimationPoolQueue = new Dictionary<string, PoolObjectQueue<SingleAnimationObject>>();
    public Dictionary<string,PoolObjectQueue<AreaEffect>> AreaEffectPoolQueue = new Dictionary<string, PoolObjectQueue<AreaEffect>>();
    public Dictionary<string,PoolObjectQueue<EffectAnimationController>> EffectAnimationPoolQueue = new Dictionary<string, PoolObjectQueue<EffectAnimationController>>();

    public event Action<string> onUnitDisable;
    public void OnUnitDisable(string unitName)
    {
        if (ActiveUnits.ContainsKey(unitName))
        {
            ActiveUnits.Remove(unitName);
            onUnitDisable?.Invoke(unitName);
        }
    }
    public event Action<string> onUnitEnable;

    public void OnUnitEnable(GenericUnitController unit)
    {
        /*try
        {*/
            ActiveUnits.Add(unit.name,unit);
        /*}
        catch (Exception e)
        {
            Debug.LogWarning(e.Message  + " : " + unit.name);
        }*/
        
        onUnitEnable?.Invoke(unit.name);
    }
    
    public Dictionary<string,GenericUnitController> ActiveUnits = new Dictionary<string, GenericUnitController>();
    
    public List<string> Targetables = new List<string>();

    public event Action<string,bool> onTargetableUpdate;

    public void AddTargetable(string targetableName)
    {
        if (!Targetables.Contains(targetableName))
        {
            Targetables.Add(targetableName);
            onTargetableUpdate?.Invoke(targetableName,true);
        }
    }
    
    public void RemoveTargetable(string targetableName)
    {
        if (Targetables.Contains(targetableName))
        {
            Targetables.Remove(targetableName);
            onTargetableUpdate?.Invoke(targetableName,false);
        }
    }
    

    public event Action<string,string> onObjectDisable;
    public void OnObjectDisable(string objectName) {
        onObjectDisable?.Invoke(objectName,name);
    }
    
    public Dictionary<string,GameObject> PlaceHoldersDict = new Dictionary<string, GameObject>();

    void CreateNewObjectQueue<T> (Dictionary<string,PoolObjectQueue<T>> dict, T prefab) where T : Component,IQueueable<T> {
        GameObject placeholder = Instantiate(new GameObject());
        placeholder.transform.parent = this.transform;
        placeholder.name = "_PlaceHolder_" + prefab.gameObject.name;
        PlaceHoldersDict.Add(placeholder.name, placeholder);
        dict.Add(prefab.name, new PoolObjectQueue<T>(prefab, 20,placeholder));
    }

    public PoolObjectQueue<T> GetOrCreateObjectQueue<T>(T t) where T : Component,IQueueable<T> {
        return null;
    }
    
    public PoolObjectQueue<EffectAnimationController> GetEffectAnimationQueue()
    {
        
        if (EffectAnimationPoolQueue.IsNullOrEmpty())
        {
            GameObject eac = new GameObject();
            eac.name = EffectAnimationController.DefaultName;
            eac.AddComponent<SpriteRenderer>();
            eac.AddComponent<AnimancerComponent>();
            eac.GetOrAddComponent<AnimationController>();
            EffectAnimationController effectAnimationController = eac.GetOrAddComponent<EffectAnimationController>();
            CreateNewObjectQueue<EffectAnimationController>(EffectAnimationPoolQueue,effectAnimationController);
            return EffectAnimationPoolQueue[eac.name];
        }
        return EffectAnimationPoolQueue[EffectAnimationController.DefaultName];
    }
    
    
    
    public PoolObjectQueue<GenericProjectile> GetOrCreateGenericProjectileQueue(GenericProjectile prefab) {
        if (GenericProjectilesObjectPoolQueue.ContainsKey(prefab.name)) {
            return GenericProjectilesObjectPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<GenericProjectile>(GenericProjectilesObjectPoolQueue, prefab);
            return GenericProjectilesObjectPoolQueue[prefab.name];
        }
    }
    
    public PoolObjectQueue<PathDiscoveryPoint> GetPathDiscoveryPool(PathDiscoveryPoint prefab) {
        if (PathDiscoveryPointObjectPoolQueue.ContainsKey(prefab.name)) {
            return PathDiscoveryPointObjectPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<PathDiscoveryPoint>(PathDiscoveryPointObjectPoolQueue, prefab);
            return PathDiscoveryPointObjectPoolQueue[prefab.name];
        }
    }

    public PoolObjectQueue<RangeDetector> GetRangeDetectorQueue(RangeDetector prefab) {
        if (RangeDetectorObjectPoolQueue.ContainsKey(prefab.name)) {
            return RangeDetectorObjectPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<RangeDetector>(RangeDetectorObjectPoolQueue, prefab);
            return RangeDetectorObjectPoolQueue[prefab.name];
        }
    }

    public PoolObjectQueue<SingleAnimationObject> GetSingleAnimationObjectQueue(SingleAnimationObject prefab) {
        if (SingleAnimationPoolQueue.ContainsKey(prefab.name)) {
            return SingleAnimationPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<SingleAnimationObject>(SingleAnimationPoolQueue, prefab);
            return SingleAnimationPoolQueue[prefab.name];
        }
    }

    public PoolObjectQueue<AreaOfEffectController> GetAOEQueue(AreaOfEffectController prefab) {
        if (AOEObjectPoolQueue.ContainsKey(prefab.name)) {
            return AOEObjectPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<AreaOfEffectController>(AOEObjectPoolQueue, prefab);
            return AOEObjectPoolQueue[prefab.name];
        }
    }

    public PoolObjectQueue<EnemyUnitController> GetUnitQueue(EnemyUnitController prefab) {
        if (EnemyUnitObjectPoolQueue.ContainsKey(prefab.name)) {
            return EnemyUnitObjectPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<EnemyUnitController>(EnemyUnitObjectPoolQueue, prefab);
            return EnemyUnitObjectPoolQueue[prefab.name];
        }
    }
    public TargetUnit GetTargetUnit(string targetName) {
        TargetUnit tu = new TargetUnit(targetName);
        return tu;
    }

    public ActiveObjectPool<UnitController> ActiveUnitPool = new ActiveObjectPool<UnitController>();
    
    public ActiveObjectPool<Effectable> ActiveEffectables = new ActiveObjectPool<Effectable>();
    public ActiveObjectPool<GenericProjectile> ActiveGenericProjectiles = new ActiveObjectPool<GenericProjectile>();

    
    public ActiveObjectPool<PathDiscoveryPoint> ActivePathDiscoveryPoints = new ActiveObjectPool<PathDiscoveryPoint>();
    public ActiveObjectPool<SplinePathController> ActiveSplines = new ActiveObjectPool<SplinePathController>();
    
    public void RemoveObjectFromAllPools(string objectName, string callerName) {
        ActiveEffectables.RemoveObjectFromPool(objectName);
        ActiveUnitPool.RemoveObjectFromPool(objectName);
        ActiveGenericProjectiles.RemoveObjectFromPool(objectName);
        ActiveGenericProjectiles.RemoveObjectFromPool(objectName);
        OnObjectDisable(objectName);
    }






    private static GameObjectPool instance;
    private static bool instantiated = false;

    public static GameObjectPool Instance
    {
        get
        {
            if (instantiated == false)
            {
                GameObject g = Instantiate(new GameObject());
                g.name = "GameObjectPool";
                g.AddComponent<GameObjectPool>();
                Instance = g.GetComponent<GameObjectPool>();
            }

            return instance;
        }
        set
        {
            instance = value;
            instantiated = true;
        }
    }

    private void Awake() {
        Instance = this;
    }

    protected void Start()
    {
        DeathManager.Instance.onUnitDeath += OnUnitDisable;
    }
}
