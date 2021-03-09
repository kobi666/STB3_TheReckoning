using UnityEngine;
using System;

public class DeathManager : MonoBehaviour
{
    public event Action<string> onUnitDeath;
    public event Action<string,string> onEnemyUnitDeath;
    public event Action<string,string> onPlayerUnitDeath;

    public void OnUnitDeath(string unitName) {
        onUnitDeath?.Invoke(unitName);
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

    private void Awake() {
        Instance = this;
        
    }
    

    void Start()
    {
        onPlayerUnitDeath += GameObjectPool.Instance.RemoveObjectFromAllPools;
        onEnemyUnitDeath += GameObjectPool.Instance.RemoveObjectFromAllPools;
    }
    
}
