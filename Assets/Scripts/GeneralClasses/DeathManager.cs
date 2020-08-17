using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathManager : MonoBehaviour
{
    public event Action<string, string> onUnitDeath;
    public event Action<string,string> onEnemyUnitDeath;
    public event Action<string,string> onPlayerUnitDeath;

    public void OnUnitDeath(string unitTag, string unitName) {
        if (unitTag == "Player_Unit") {
            onPlayerUnitDeath?.Invoke(unitName,name);
        }
        if (unitTag == "Enemy") {
            onEnemyUnitDeath?.Invoke(unitName,name);
        }
        onUnitDeath?.Invoke(unitTag, unitName);
    }
    public static DeathManager instance;

    private void Awake() {
        instance = this;
        
    }
    

    void Start()
    {
        onPlayerUnitDeath += GameObjectPool.Instance.RemoveObjectFromAllPools;
        onEnemyUnitDeath += GameObjectPool.Instance.RemoveObjectFromAllPools;
    }
    
}
