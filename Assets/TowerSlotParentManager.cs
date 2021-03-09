using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlotParentManager : MonoBehaviour
{
    public static TowerSlotParentManager instance;

    public Dictionary<string, (Vector2, TowerSlotController)> TowerslotControllers
    {
        get
        {
            return towerSlotControllers;
        }
        set
        {
            towerSlotControllers = value;
        }
    }
    
    public Dictionary<Vector2,TowerSlotController> V2_Tsc = new Dictionary<Vector2, TowerSlotController>();

    public Dictionary<Vector2, TowerSlotController> TowerSlotControllersNoNameString()
    {
        Dictionary<Vector2, TowerSlotController> dict = new Dictionary<Vector2, TowerSlotController>();
        foreach (var v2tsc in TowerslotControllers.Values)
        {
            dict.Add(v2tsc.Item1, v2tsc.Item2);
        }

        return dict;
    }

    void ConvertDict()
    {
        V2_Tsc = TowerSlotControllersNoNameString();
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
        onGetTowerSlotsWithPositions += ConvertDict;
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

            if (TowerslotControllers.ContainsKey(tsc.name))
            {
                continue;
            }
            TowerslotControllers.Add(tsc.name, (tsc.transform.position,tsc ));
        }
    }



}
