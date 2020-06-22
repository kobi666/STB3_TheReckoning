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

    public event Action<string> onUnitDeath;
    public void OnUnitDeath(string unitName) {
        onUnitDeath?.Invoke(unitName);
    }

    void invokeSpawnOnUnitDeath(string unitName) {
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
        if (unitsIndex.Count < Data.MaxUnits) {
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

    public void RemoveUnitFromIndex(string unitName) {
        if (unitsIndex.ContainsKey(unitName)) {
            unitsIndex.Remove(unitName);
        }
    }

    Dictionary<int,IEnumerator> SpawningCoroutines = new Dictionary<int, IEnumerator>();
    IEnumerator SpawnPlayerUnitAfterCounterAndAddToIndexWithCounter(int unitBaseIndex) {
        float counter = 0;
        while (this != null) {
            counter += StaticObjects.instance.DeltaGameTime;
            if (counter >= Data.PlayerUnitSpawnTime) {
                PlayerUnitController puc = PlayerUnitSpawnerUtils.SpawnPlayerUnit(Data.PlayerUnitPrefab, GetRallyPoint(unitBaseIndex), unitBaseIndex);
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
            PlayerUnitController puc = PlayerUnitSpawnerUtils.SpawnPlayerUnit(Data.PlayerUnitPrefab, SpawningPointPosition, unitBaseIndex);
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
        return rallyPoint.GetRallyPoint(unitBaseIndex, Data.MaxUnits);
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
    private void Start() {
        onUnitDeath += invokeSpawnOnUnitDeath;
        onUnitSpawn += SpawnUnitOnDeath;
        DeathManager.instance.onPlayerUnitDeath += OnUnitDeath;
        onUnitDeath += RemoveUnitFromIndex;
        RallyPoint = GetComponentInChildren<PlayerUnitRallyPoint>() ?? null;
        SpawningPointComponent = GetComponentInChildren<PlayerUnitSpawningPoint>() ?? null;
        for (int i = 0 ; i < Data.MaxUnits ; i++) {
            SpawnPlayerUnitAndAddToIndex(i);
        }
        PostStart();
    }
    
}
