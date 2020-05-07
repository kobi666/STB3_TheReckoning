using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicMeleePlayerUnit : PlayerUnitController
{
    public override bool CanEnterBattle() {
        return PlayerUnitUtils.StandardEnterBattleCondition(CurrentState, Data.EnemyTarget, States);
    }
    public override IEnumerator OnEnterPreBattle() {
        if (PlayerUnitUtils.CheckIfEnemyIsInBattleWithOtherUnit(Target) == false) {
            SM.SetState(States.InDirectBattle);
        }
        else {
            SM.SetState(States.JoinBattle);
        }
        yield break;
    }

    public override IEnumerator OnEnterJoinBattle() {
        yield break;
    }

    public override IEnumerator OnExitJoinBattle() {
        yield break;
    }

    public override IEnumerator OnExitPreBattle() {
        yield break;
    }

    public override IEnumerator OnEnterDefault() {
        yield break;
    }

    public override IEnumerator OnExitDefault() {
        yield break;
    }

    public override IEnumerator OnEnterInBattle() {
        yield break;
    }

    public override IEnumerator OnExitInBattle() {
        yield break;
    }

    public override IEnumerator OnEnterPostBattle() {
        yield break;
    }

    public override IEnumerator OnExitPostBattle() {
        yield break;
    }

    public override void Test2() { Debug.LogWarning("TESTTTT") ;}
    
    // Start is called before the first frame update
    public override void LateStart() {
        SM.SetState(States.Default);
    }
}
