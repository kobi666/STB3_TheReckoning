using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BansheeGz.BGSpline.Components;
using Sirenix.OdinInspector;


[System.Serializable]
public class UnitData
{
    public UnitMetaData MetaData = new UnitMetaData();
    public UnitDynamicData DynamicData = new UnitDynamicData();
}

[Serializable]
public class UnitMetaData
{
    public int HP;
    public float MovementSpeed = 1;
    public float AttackSpeed;
    public float DiscoveryRadius = 1;
}

[Serializable]
public class UnitDynamicData
{
    public Vector2 TargetPosition;
    [ShowInInspector]
    public Vector2? BasePosition;
    public BGCcMath Spline;
    public float DistanceOnPath;
}


[System.Serializable]
public class UnitDataLegacy
{
    public bool CanBeStoppedByOtherUnit;
    public Dictionary<string, GameObject> TargetsInRange;
    public int AttackRate;
    public DamageRange DamageRange;
    public int HP;
    public int Armor;
    public int SpecialArmor;
    public int speed;
    public Vector2 SetPosition;

    public Effectable EffectableTarget;
    public GameObject Target;
    private PlayerUnitController playerTarget;
    public PlayerUnitController PlayerTarget {
        get {
            if (playerTarget?.gameObject.activeSelf == false) {
                return null;
            }
            return playerTarget;
        }
        set {
            playerTarget = value;
        }
    }
    private EnemyUnitController enemyTarget;
    public EnemyUnitController EnemyTarget {
        get {
            if (enemyTarget != null) {
                if (enemyTarget.gameObject.activeSelf == false) {
                    return null;
                }
            }
            return enemyTarget;
        }
        set {
            enemyTarget = value;
        }
    }
    public UnitType unitType;
    public Damage_Type damageType = new Damage_Type("normal");
    Dictionary<string, PlayerUnitController> playerUnitsFightingMe = new Dictionary<string, PlayerUnitController>();

    public void RemovePlayerUnitTarget(string unitName,string callerName) {
        if (PlayerTarget?.name == unitName) {
            PlayerTarget = null;
            EffectableTarget = null;
        }
        RemoveUnitFromPlayerUnitsFightingDictionary(unitName);
    }

    public void SetEnemyTargetToNull(string unitName,string callerName) {
        if (EnemyTarget != null) {
            if (EnemyTarget.name == unitName) {
                EnemyTarget = null;
            }
        }
    }

    public Dictionary<string, PlayerUnitController> PlayerUnitsFightingMe {
        get {
            foreach (var item in playerUnitsFightingMe)
            {
                if(item.Value == null) {
                    playerUnitsFightingMe.Remove(item.Key);
                }
            }
            return playerUnitsFightingMe;
        }
    }

    public bool AreTherePlayerUnitsFightingMe() {
        if (PlayerUnitsFightingMe.Count > 0) {
            return true;
        }
        return false;
    }

    public PlayerUnitController GetFirstPlayerUnitControllerFromList() {
        foreach (var item in PlayerUnitsFightingMe)
        {
            if (item.Value != null && item.Value.IsTargetable()) {
                return item.Value;
            }
        }
        return null;
    }

    public void AddUnitToPlayerUnitsFightingDictionary(PlayerUnitController pc) {
        if (!PlayerUnitsFightingMe.ContainsKey(pc.name)) {
            PlayerUnitsFightingMe.Add(pc.name, pc);
        }
    }

    public void RemoveUnitFromPlayerUnitsFightingDictionary(PlayerUnitController pc) {
        try {
            PlayerUnitsFightingMe.Remove(pc.name);
        }
        catch {
            Debug.LogWarning("No unit name " + pc?.name ?? "NULL" + " in dictionary");
        }
    }

    public void RemoveUnitFromPlayerUnitsFightingDictionary(string unitName) {
        try {
            PlayerUnitsFightingMe.Remove(unitName);
        }
        catch {
            Debug.LogWarning("No unit name " + unitName ?? "NULL" + " in dictionary");
        }
    }
}
