using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[DefaultExecutionOrder(-10)]
public class LevelManager : MonoBehaviour
{
    [ShowInInspector]
    public Dictionary<int,(Vector2,TowerSlotController)> LevelTowerSlots = new Dictionary<int, (Vector2,TowerSlotController)>();

    [HideInInspector] public bool LevelFinished = false;
    
    
    
    [ShowInInspector]
    private int totalUnitsInLevel = 0;

    public int TotalUnitsInLevel
    {
        get => totalUnitsInLevel;
        set
        {
            totalUnitsInLevel += value;
            CurrentUnitsInLevel += value;
        }
    }

    [ShowInInspector]
    public int CurrentUnitsInLevel
    {
        get => currentUnitsInLevel;
        set
        {
            currentUnitsInLevel = value;
            if (currentUnitsInLevel <= 0)
            {
                AllUnitsFinished?.Invoke();
            }
        }
    }

    public event Action AllUnitsFinished;

    private int currentUnitsInLevel;



    [Required] public resourcesManager ResourcesManager;
    
    public int InitialMoney = 0;
    public int InitialLife = 20;

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
            if (!LevelTowerSlots.ContainsKey(tsc.MyGameObjectID))
            {
                LevelTowerSlots.Add(tsc.MyGameObjectID,(tsc.transform.position,tsc));
                onTowerSlotReclaculate += tsc.OnTowerPositionCalculation;
            }
            else
            {
                LevelTowerSlots[tsc.MyGameObjectID] = (tsc.transform.position, tsc);
            }
        }
        else
        {
            if (LevelTowerSlots.ContainsKey(tsc.MyGameObjectID))
            {
                LevelTowerSlots.Remove(tsc.MyGameObjectID);
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
