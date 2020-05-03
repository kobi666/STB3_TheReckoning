using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitController
{
    // Start is called before the first frame update
    public float Proximity {
        get => Walker.ProximityToEndOfSplineFunc();
    }
    
}
