using System;
using UnityEngine;

[Serializable]
public class TestPlayerPlayerUnitSpawner : PlayerPlayerUnitSpawner
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


    public override void InitComponent()
    {
        
    }

    public override void PostAwake() {

    }

    
}
