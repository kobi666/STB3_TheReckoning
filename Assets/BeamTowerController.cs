using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTowerController : TowerController
{
    // Start is called before the first frame update
    public override TowerSlotAction NorthAction()
    {
        return null;
    }

    public override bool NorthExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override TowerSlotAction EastAction()
    {
        return null;
    }

    public override bool EastExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override TowerSlotAction SouthAction()
    {
        return null;
    }

    public override bool SouthExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override TowerSlotAction WestAction()
    {
        return null;
    }

    public override bool WestExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override void PostStart()
    {
        
    }

    protected void Start()
    {
        
    }

    public override void PostAwake()
    {
        
    }

    // Update is called once per frame
    
}
