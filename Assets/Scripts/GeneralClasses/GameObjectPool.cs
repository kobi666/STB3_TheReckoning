using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameObjectPool : MonoBehaviour
{
    public Dictionary<string, PoolObjectQueue<RangeDetector>> RangeDetectorObjectPoolQueue = new Dictionary<string, PoolObjectQueue<RangeDetector>>();
    public Dictionary<string, PoolObjectQueue<Projectile>> ProjectilesObjectPoolQueue = new Dictionary<string, PoolObjectQueue<Projectile>>();
    public Dictionary<string, PoolObjectQueue<UnitController>> UnitObjectPoolQueue = new Dictionary<string, PoolObjectQueue<UnitController>>();
    public Dictionary<string, PoolObjectQueue<PathDiscoveryPoint>> PathDiscoveryPointObjectPoolQueue = new Dictionary<string, PoolObjectQueue<PathDiscoveryPoint>>();

    public Dictionary<string, PoolObjectQueue<GenericProjectile>> GenericProjectilesObjectPoolQueue = new Dictionary<string, PoolObjectQueue<GenericProjectile>>();
    
    public Dictionary<string, PoolObjectQueue<AreaOfEffectController>> AOEObjectPoolQueue = new Dictionary<string, PoolObjectQueue<AreaOfEffectController>>();
    public Dictionary<string,PoolObjectQueue<SingleAnimationObject>> SingleAnimationPoolQueue = new Dictionary<string, PoolObjectQueue<SingleAnimationObject>>();


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

    public PoolObjectQueue<Projectile> GetProjectileQueue(Projectile prefab) {
        if (ProjectilesObjectPoolQueue.ContainsKey(prefab.name)) {
            return ProjectilesObjectPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<Projectile>(ProjectilesObjectPoolQueue, prefab);
            return ProjectilesObjectPoolQueue[prefab.name];
        }
    }
    
    public PoolObjectQueue<GenericProjectile> GetGenericProjectileQueue(GenericProjectile prefab) {
        if (ProjectilesObjectPoolQueue.ContainsKey(prefab.name)) {
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

    public PoolObjectQueue<UnitController> GetUnitQueue(UnitController prefab) {
        if (UnitObjectPoolQueue.ContainsKey(prefab.name)) {
            return UnitObjectPoolQueue[prefab.name];
        }
        else {
            CreateNewObjectQueue<UnitController>(UnitObjectPoolQueue, prefab);
            return UnitObjectPoolQueue[prefab.name];
        }
    }
    public TargetUnit GetTargetUnit(string targetName) {
        TargetUnit tu = new TargetUnit(targetName);
        return tu;
    }

    public ActiveObjectPool<UnitController> ActiveUnitPool = new ActiveObjectPool<UnitController>();
    
    public ActiveObjectPool<Effectable> ActiveEffectables = new ActiveObjectPool<Effectable>();
    public ActiveObjectPool<GenericProjectile> ActiveGenericProjectiles = new ActiveObjectPool<GenericProjectile>();

    public ActiveObjectPool<Projectile> ActiveProjectiles = new ActiveObjectPool<Projectile>();
    public ActiveObjectPool<PathDiscoveryPoint> ActivePathDiscoveryPoints = new ActiveObjectPool<PathDiscoveryPoint>();
    
    public void RemoveObjectFromAllPools(string objectName, string callerName) {
        ActiveEffectables.RemoveObjectFromPool(objectName);
        ActiveUnitPool.RemoveObjectFromPool(objectName);
        ActiveProjectiles.RemoveObjectFromPool(objectName);
        ActiveGenericProjectiles.RemoveObjectFromPool(objectName);
        OnObjectDisable(objectName);
    }



    



    public static GameObjectPool Instance {get ; private set;}

    private void Awake() {
        Instance = this;
        
    }
}
