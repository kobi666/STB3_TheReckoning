using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitUtils : MonoBehaviour
{
    // Start is called before the first frame update
    public static IEnumerator StopWalkingOnPath(BezierSolution.UnitWalker walker) {
        walker.StopWalking();
        yield break;
    }

    public static bool StandardIsTargetable(EnemyUnitController ec) {
        if (ec.CurrentState != ec.States.Death) {
            return true;
        }
        return false;
    }
}
