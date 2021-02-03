using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerPlayerUnitSpawner : TowerComponent
{
    public bool ExternalSpawningLock = false;
    public virtual bool CanSpawn() {
        if (ExternalSpawningLock != true) {
            return true;
        }
        return false;
    }

    private Vector2 FoundRallyPointPosition;

    public void FindRallyPointPosition()
    {
        FoundRallyPointPosition = ParentTowerSlot.lowestProximityPointFound;
    }

    public event Action<string,string> onUnitDeath;
    public void OnUnitDeath(string unitName,string callerName) {
        onUnitDeath?.Invoke(unitName,name);
    }

    void invokeSpawnOnUnitDeath(string unitName, string callerName) {
        if (unitsIndex.ContainsKey(unitName)) {
            OnUnitSpawn(unitsIndex[unitName].Item2);
        }
    }
    public event Action<int> onUnitSpawn;
    public void OnUnitSpawn(int unitBaseIndex) {
        onUnitSpawn?.Invoke(unitBaseIndex);
    }

    public Dictionary<string, (PlayerUnitController,int)> unitsIndex = new Dictionary<string, (PlayerUnitController, int)>();

    void AddUnitToIndex(PlayerUnitController puc) {
        if (unitsIndex.Count < Data.SpawnerData.maxUnits) {
            if (CanSpawn()) {
                unitsIndex.Add(puc.name,(puc,puc.UnitBaseIndex));
            }
        }
    }

    public void StopSpwaningCoroutines() {
        foreach (var item in SpawningCoroutines)
        {
            try {
                StopCoroutine(item.Value);
            }
            catch (Exception e) {
                Debug.LogWarning(e.Message);
            }
        }
    }

    public void SpawnUnitOnDeath(int unitBaseIndex) {
        if (SpawningCoroutines.ContainsKey(unitBaseIndex)) {
            SpawningCoroutines.Remove(unitBaseIndex);
        }
        SpawningCoroutines.Add(unitBaseIndex, SpawnPlayerUnitAfterCounterAndAddToIndexWithCounter(unitBaseIndex));
        StartCoroutine(SpawningCoroutines[unitBaseIndex]);
    }

    public void RemoveUnitFromIndex(string unitName,string callerName) {
        if (unitsIndex.ContainsKey(unitName)) {
            unitsIndex.Remove(unitName);
        }
    }

    Dictionary<int,IEnumerator> SpawningCoroutines = new Dictionary<int, IEnumerator>();
    IEnumerator SpawnPlayerUnitAfterCounterAndAddToIndexWithCounter(int unitBaseIndex) {
        /*float counter = 0;
        while (this != null) {
            counter += StaticObjects.Instance.DeltaGameTime;
            if (counter >= Data.SpawnerData.playerUnitSpawnTime) {
                PlayerUnitController puc = PlayerUnitSpawnerUtils.SpawnPlayerUnitFromSpawner(this, unitBaseIndex);
                try {
                AddUnitToIndex(puc);
                }
                catch (Exception e) {
                    Debug.LogWarning(e.Message);
                }
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }*/
        return null;
    }

    public void SpawnPlayerUnitAndAddToIndex(int unitBaseIndex) {
            /*//PlayerUnitController puc = PlayerUnitSpawnerUtils.SpawnPlayerUnitFromSpawner(this,unitBaseIndex);
            rallyPoint.transform.position = 
            puc.Data.SetPosition = GetRallyPoint(unitBaseIndex);
            try {
            AddUnitToIndex(puc);
            }
            catch (Exception e) {
                Debug.LogWarning(e.Message);
            }*/
    }


    public PlayerUnitRallyPoint RallyPoint {get => rallyPoint; private set { rallyPoint = value;}}
    PlayerUnitRallyPoint rallyPoint;
    public Vector2 GetRallyPoint(int unitBaseIndex) {
        return rallyPoint.GetRallyPoint(unitBaseIndex, Data.SpawnerData.maxUnits);
    }
    public Vector2 SpawningPointPosition {get {
        if (SpawningPointComponent == null) {
            return SpawningPointComponent.transform.position;
        }
        return transform.position;
    }}


    public override void InitComponent()
    {
        
    }

    public override void PostAwake()
    {
        
    }

    protected void Awake()
    {
        base.Awake();
        InitPools();
    }

    public PlayerUnitSpawningPoint SpawningPointComponent;
    //Name, index, counter, Prefab
//    Dictionary<string, (int,float,PlayerUnitController)> Units;
    
    protected void Start() {
        base.Start();
        onUnitDeath += invokeSpawnOnUnitDeath;
        onUnitSpawn += SpawnUnitOnDeath;
        DeathManager.Instance.onPlayerUnitDeath += OnUnitDeath;
        onUnitDeath += RemoveUnitFromIndex;
        RallyPoint = GetComponentInChildren<PlayerUnitRallyPoint>() ?? null;
        RallyPoint.transform.position = ParentTowerSlot?.lowestProximityPointFound ?? transform.position;
        SpawningPointComponent = GetComponentInChildren<PlayerUnitSpawningPoint>() ?? null;
        for (int i = 0 ; i < Data.SpawnerData.maxUnits ; i++) {
            SpawnPlayerUnitAndAddToIndex(i);
        }
    }
    
    public List<PlayerUnitPoolCreationData> units = new List<PlayerUnitPoolCreationData>();
    public List<PlayerUnitPoolCreationData> Units { get => units;
        set => units = value;
    }
    public Dictionary<string, PoolObjectQueue<PlayerUnitController>> UnitPools { get; set; } = new Dictionary<string, PoolObjectQueue<PlayerUnitController>>();
    public void InitPools()
    {
        foreach (var unitdata in Units)
        {
            GameObject ph = new GameObject();
            ph.name = "placeholder_" + unitdata.UnitPrefabBase.name;
            ph.transform.parent = this.transform;
            PlayerUnitController unitInstance = GameObject.Instantiate(unitdata.UnitPrefabBase);
            unitInstance.transform.position = new Vector2(9999, 9999);
            unitInstance.Data = unitdata.UnitData;
            UnitPools.Add(unitdata.UnitPrefabBase.name, new PoolObjectQueue<PlayerUnitController>(unitInstance,5, ph));
        }
    }

    public override List<Effect> GetEffectList()
    {
        return null;
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        
    }

    public override List<TagDetector> GetRangeDetectors()
    {
        return null;
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        base.UpdateRange(RangeSizeDelta, detectors);
    }
}
