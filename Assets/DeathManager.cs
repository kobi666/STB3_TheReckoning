using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathManager : MonoBehaviour
{
    public event Action<string, string> onUnitDeath;
    public event Action<string> onEnemyUnitDeath;
    public event Action<string> onPlayerUnitDeath;

    public void OnUnitDeath(string unitTag, string unitName) {
        if (unitTag == "Player") {
            onPlayerUnitDeath?.Invoke(unitName);
        }
        if (unitTag == "Enemy") {
            onEnemyUnitDeath?.Invoke(unitName);
        }
    }
    public static DeathManager instance;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    
}
