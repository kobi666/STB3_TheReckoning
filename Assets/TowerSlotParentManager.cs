using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TowerSlotParentManager : SerializedMonoBehaviour
{
    public static TowerSlotParentManager instance;

    public Dictionary<string, (Vector2, TowerSlotController)> TowerslotControllers
    {
        get
        {
            OnGetTowerSlotsWithPositions();
            return towerSlotControllers;
        }
        set
        {
            towerSlotControllers = value;
        }
    }
    Dictionary<string,(Vector2,TowerSlotController)> towerSlotControllers = new Dictionary<string, (Vector2,TowerSlotController)>();
    public event Action onGetTowerSlotsWithPositions;

    public void OnGetTowerSlotsWithPositions()
    {
        onGetTowerSlotsWithPositions?.Invoke();
    }

    private void Awake()
    {
        instance = this;
        onGetTowerSlotsWithPositions += getTowerSlotsWithPositions;
    }

    void getTowerSlotsWithPositions()
    {
        TowerSlotController[] TowerSlotControllersArray = GetComponentsInChildren<TowerSlotController>();
        foreach (var tsc in TowerSlotControllersArray)
        {
            if (tsc == null)
            {
                continue;
            }
            TowerslotControllers.Add(tsc.name, (tsc.transform.position,tsc ));
        }
    }

    void Start()
    {
        
    }

    
    
}
