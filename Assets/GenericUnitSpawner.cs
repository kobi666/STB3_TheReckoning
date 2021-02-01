using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUnitSpawner : TowerComponent
{
    public List<PlayerUnitController> PlayerUnitPrefabs = new List<PlayerUnitController>();
    public override void InitComponent()
    {
        
    }

    public override void PostAwake()
    {
        
    }

    void Start()
    {
        
    }

    public override List<Effect> GetEffectList()
    {
        return null;
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        
    }

    public override List<TagDetector> GetRangeDetectors()
    {
        return null;
    }

    
}
