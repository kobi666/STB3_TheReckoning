using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;
using MyBox;
using Sirenix.OdinInspector;


[DefaultExecutionOrder(-1)]
public class GameObjectPool : MonoBehaviour
{
    [ShowInInspector]
    public Dictionary<int,int> CollisionIDToGameObjectID = new Dictionary<int, int>();
    
    
    
    public Dictionary<string, PoolObjectQueue<RangeDetector>> RangeDetectorObjectPoolQueue = new Dictionary<string, PoolObjectQueue<RangeDetector>>();
    public Dictionary<string, PoolObjectQueue<PathDiscoveryPoint>> PathDiscoveryPointObjectPoolQueue = new Dictionary<string, PoolObjectQueue<PathDiscoveryPoint>>();

    public Dictionary<string, PoolObjectQueue<GenericProjectile>> GenericProjectilesObjectPoolQueue = new Dictionary<string, PoolObjectQueue<GenericProjectile>>();
    
    public Dictionary<string, PoolObjectQueue<AreaOfEffectController>> AOEObjectPoolQueue = new Dictionary<string, PoolObjectQueue<AreaOfEffectController>>();
    public Dictionary<string,PoolObjectQueue<SingleAnimationObject>> SingleAnimationPoolQueue = new Dictionary<string, PoolObjectQueue<SingleAnimationObject>>();
    public Dictionary<string,PoolObjectQueue<AreaEffect>> AreaEffectPoolQueue = new Dictionary<string, PoolObjectQueue<AreaEffect>>();
    public Dictionary<string,PoolObjectQueue<EffectAnimationController>> EffectAnimationPoolQueue = new Dictionary<string, PoolObjectQueue<EffectAnimationController>>();

    public event Action<int> onUnitDisable;
    public void OnUnitDisable(int gameObjectID)
    {
        if (ActiveUnits.ContainsKey(gameObjectID))
        {
            ActiveUnits.Remove(gameObjectID);
            onUnitDisable?.Invoke(gameObjectID);
        }
    }
    public event Action<string> onUnitEnable;

    public void OnUnitEnable(GenericUnitController unit)
    {
        /*try
        {*/
            ActiveUnits.Add(unit.GameObjectID,unit);
        /*}
        catch (Exception e)
        {
            Debug.LogWarning(e.Message  + " : " + unit.name);
        }*/
        
        onUnitEnable?.Invoke(unit.name);
    }
    
    public Dictionary<int,GenericUnitController> ActiveUnits = new Dictionary<int, GenericUnitController>();
    
    public List<int> Targetables = new List<int>();

    public event Action<int,bool> onTargetableUpdate;

    public void AddTargetable(int targetabelGameobjectID)
    {
        if (!Targetables.Contains(targetabelGameobjectID))
        {
            Targetables.Add(targetabelGameobjectID);
            onTargetableUpdate?.Invoke(targetabelGameobjectID,true);
        }
    }
    
    public void RemoveTargetable(int gameObjectID)
    {
        if (Targetables.Contains(gameObjectID))
        {
            Targetables.Remove(gameObjectID);
            onTargetableUpdate?.Invoke(gameObjectID,false);
        }
    }
    

    public event Action<int> onObjectDisable;
    public void OnObjectDisable(int gameObjectID) {
        onObjectDisable?.Invoke(gameObjectID);
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

    
    public TargetUnit GetTargetUnit(int gameObjectID) {
        TargetUnit tu = new TargetUnit(gameObjectID);
        return tu;
    }
    
    
    public ActiveObjectPool<Effectable> ActiveEffectables = new ActiveObjectPool<Effectable>();
    public ActiveObjectPool<GenericProjectile> ActiveGenericProjectiles = new ActiveObjectPool<GenericProjectile>();

    
    public ActiveObjectPool<PathDiscoveryPoint> ActivePathDiscoveryPoints = new ActiveObjectPool<PathDiscoveryPoint>();
    public ActiveObjectPool<SplinePathController> ActiveSplines = new ActiveObjectPool<SplinePathController>();
    
    public void RemoveObjectFromAllPools(int GameObjectID, string callerName) {
        ActiveEffectables.RemoveObjectFromPool(GameObjectID);
        ActiveGenericProjectiles.RemoveObjectFromPool(GameObjectID);
        ActiveGenericProjectiles.RemoveObjectFromPool(GameObjectID);
        OnObjectDisable(GameObjectID);
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
