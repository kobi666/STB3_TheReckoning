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
    public event Action onUnitSpawn;
    public void OnUnitSpawn() {
        onUnitSpawn?.Invoke();
    }

    Dictionary<string, (PlayerUnitController,int)> unitsIndex;

    public void AddUnitToIndex(PlayerUnitController puc) {
        if (unitsIndex.Count < Data.MaxUnits) {
            if (CanSpawn()) {
                unitsIndex.Add(puc.name,(puc,puc.UnitBaseIndex));
            }
        }
    }

    public void RemoveUnitFromIndex(string unitName) {
        if (unitsIndex.ContainsKey(unitName)) {
            unitsIndex.Remove(unitName);
        }
    }

    IEnumerator SpawnPlayerUnitAfterCounterAndAddToIndex(int unitBaseIndex) {
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
        DeathManager.instance.onPlayerUnitDeath += OnUnitDeath;
        rallyPoint = GetComponentInChildren<PlayerUnitRallyPoint>() ?? null;
        SpawningPointComponent = GetComponentInChildren<PlayerUnitSpawningPoint>() ?? null;
        PostStart();
    }
    
}
