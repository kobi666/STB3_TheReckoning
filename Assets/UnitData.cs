using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[System.Serializable]
public class UnitData
{
    // Start is called before the first frame update
    public bool CanBeStoppedByOtherUnit;
    public Dictionary<string, GameObject> TargetsInRange;
    public int AttackRate;
    public DamageRange DamageRange;
    public int HP;
    public int Armor;
    public int SpecialArmor;
    public int speed;
    public Vector2 SetPosition;
    public GameObject Target;
    public PlayerUnitController PlayerTarget;
    public EnemyUnitController EnemyTarget;
    public UnitType unitType;
    public Damage_Type damageType = new Damage_Type("normal");
    Dictionary<string, PlayerUnitController> playerUnitsFightingMe = new Dictionary<string, PlayerUnitController>();

    public void RemovePlayerUnitTarget(string unitName) {
        if (PlayerTarget?.name == unitName) {
            PlayerTarget = null;
        }
        RemoveUnitFromPlayerUnitsFightingDictionary(unitName);
    }

    public void SetEnemyTargetToNull(string unitName) {
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
