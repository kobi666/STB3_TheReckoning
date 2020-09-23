using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerUnitSpawner : TowerComponent
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
        float counter = 0;
        while (this != null) {
            counter += StaticObjects.instance.DeltaGameTime;
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
        }
    }

    public void SpawnPlayerUnitAndAddToIndex(int unitBaseIndex) {
            PlayerUnitController puc = PlayerUnitSpawnerUtils.SpawnPlayerUnit(Data.SpawnerData.playerUnitPrefab, SpawningPointPosition, unitBaseIndex);
            rallyPoint.transform.position = 
            puc.Data.SetPosition = GetRallyPoint(unitBaseIndex);
            try {
            AddUnitToIndex(puc);
            }
            catch (Exception e) {
                Debug.LogWarning(e.Message);
            }
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


    public PlayerUnitSpawningPoint SpawningPointComponent;
    //Name, index, counter, Prefab
//    Dictionary<string, (int,float,PlayerUnitController)> Units;
    public abstract void PostStart();
    protected void Start() {
        
        onUnitDeath += invokeSpawnOnUnitDeath;
        onUnitSpawn += SpawnUnitOnDeath;
        DeathManager.instance.onPlayerUnitDeath += OnUnitDeath;
        onUnitDeath += RemoveUnitFromIndex;
        RallyPoint = GetComponentInChildren<PlayerUnitRallyPoint>() ?? null;
        RallyPoint.transform.position = ParentTowerSlot.lowestProximityPointFound;
        SpawningPointComponent = GetComponentInChildren<PlayerUnitSpawningPoint>() ?? null;
        for (int i = 0 ; i < Data.SpawnerData.maxUnits ; i++) {
            SpawnPlayerUnitAndAddToIndex(i);
        }
        PostStart();
    }
    
}
