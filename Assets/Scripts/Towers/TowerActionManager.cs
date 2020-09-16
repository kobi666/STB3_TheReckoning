using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerActionManager : MonoBehaviour
{
    private TowerController TowerController;
    
    public TowerSlotAction NorthAction();
    public bool NorthExecutionCondition(TowerComponent tc);
    public TowerSlotAction EastAction();
    public bool EastExecutionCondition(TowerComponent tc);
    public TowerSlotAction SouthAction();
    public bool SouthExecutionCondition(TowerComponent tc);
    public TowerSlotAction WestAction();
    public bool WestExecutionCondition(TowerComponent tc);
    
    
    protected void Awake()
    {
        TowerController = GetComponent<TowerController>();
    }
}
