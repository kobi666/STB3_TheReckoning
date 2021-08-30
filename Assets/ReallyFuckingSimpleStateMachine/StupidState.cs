using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidState
{
    public StupidStateNames StateName;
    
    
    
}


public abstract class StupidBehavior
{
    
    
    
    
}








public enum StupidStateNames
{
    GoToRallyPoint,
    WaitForTargetToEnterMeleeRange,
    GoToTarget,
    StartMeleeAttack,
    GoBackToRallyPoint,
    SpecialState,
    GoToBeginingOfSpline,
    WalkAlongSpline,
    WalkAlongSplineAndWaitForTargetToEnterMeleeRange,
    OnArrivalToPlayerBase,
    Death
}
