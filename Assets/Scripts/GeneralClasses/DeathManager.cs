﻿using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DeathManager : MonoBehaviour
{
    public event Action<int> onUnitDeath;
    public event Action<string,string> onEnemyUnitDeath;
    public event Action<string,string> onPlayerUnitDeath;
    [Required]
    private GameManager GameManager;

    public void OnUnitDeath(int unitGameObjectID) {
        onUnitDeath?.Invoke(unitGameObjectID);
    }

    public static DeathManager instance = null;
    static bool instantiated = false;

    public static DeathManager Instance
    {
        get
        {
            if (instantiated == false)
            {
               GameObject g = new GameObject();
               g.AddComponent<DeathManager>();
               g.name = "DeathManager";
               Instance = g.GetComponent<DeathManager>();
            }

            return instance;
        }
        set
        {
            instance = value;
            instantiated = true;
        }
    }

    public void AddMoney(int money)
    {
        
    }

    private void Awake() {
        Instance = this;
        
    }
    

    void Start()
    {
        GameManager = GameManager.Instance;
    }
    
}
