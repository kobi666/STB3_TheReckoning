using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerUnitSpawner : PlayerUnitSpawner
{
    private void Update() {
        for (int i = 0; i < Data.SpawnerData.maxUnits ; i++) {
            Debug.DrawLine(RallyPoint.transform.position, GetRallyPoint(i));
        }

    }


    protected void Start()
    {
        base.Start();
        
        
    }

    public override List<Effect> GetEffectList()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        throw new System.NotImplementedException();
    }

    public override List<TagDetector> GetRangeDetectors()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        throw new System.NotImplementedException();
    }


    public override void InitComponent()
    {
        throw new System.NotImplementedException();
    }

    public override void PostAwake() {

    }

    public override void PostStart() {
        
    }
}
