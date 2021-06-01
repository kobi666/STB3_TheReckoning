using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class LevelManager : MonoBehaviour
{
    [ShowInInspector]
    public Dictionary<string,(Vector2,TowerSlotController)> LevelTowerSlots = new Dictionary<string, (Vector2,TowerSlotController)>();

    [Required] public resourcesManager ResourcesManager; 
    

    public event Action<TowerSlotController, bool> onTowerSlotUpdate;

    public event Action onTowerSlotReclaculate;

    public void OnTowerSlotRecalculate()
    {
        onTowerSlotReclaculate?.Invoke();
    }

    public void OnTowerSlotUpdate(TowerSlotController tsc, bool addAndUpdateOrRemove)
    {
        onTowerSlotUpdate?.Invoke(tsc,addAndUpdateOrRemove);
    }

    private void UpdateTowerSlot(TowerSlotController tsc, bool addAndUpdateOrRemove)
    {
        if (addAndUpdateOrRemove)
        {
            if (!LevelTowerSlots.ContainsKey(tsc.name))
            {
                LevelTowerSlots.Add(tsc.name,(tsc.transform.position,tsc));
                onTowerSlotReclaculate += tsc.OnTowerPositionCalculation;
            }
            else
            {
                LevelTowerSlots[tsc.name] = (tsc.transform.position, tsc);
            }
        }
        else
        {
            if (LevelTowerSlots.ContainsKey(tsc.name))
            {
                LevelTowerSlots.Remove(tsc.name);
                onTowerSlotReclaculate -= tsc.OnTowerPositionCalculation;
            }
        }
        
        OnTowerSlotRecalculate();
    }

    protected void Awake()
    {
        onTowerSlotUpdate += UpdateTowerSlot;
    }
}
