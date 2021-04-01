using System.Collections;
using UnityEngine;

public class EnemyUnitUtils : MonoBehaviour
{
    

    

// public static IEnumerator TellEnemyToPrepareFor1on1battleWithMe(EnemyUnitController ec, PlayerUnitController pc) {
//         if (ec.CurrentState == ec.States.Default) {
//             ec.Data.PlayerTarget = pc;
//             ec.SM.SetState(ec.States.PreBattle);
//         }
//         yield break;
//     }
    

    
    

    

    
    // Start is called before the first frame update
    public static IEnumerator StopWalkingOnPath(BezierSolution.UnitWalker walker) {
        walker.StopWalking();
        yield break;
    }
}
