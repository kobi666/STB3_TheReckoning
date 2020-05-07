using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicEnemyMeleeUnit : EnemyUnitController
{
    public override void Test2() { Debug.LogWarning("TESTTT");}
    public override bool CannotInitiateBattleWithThisUnit() {
        return false;
    }
    // Start is called before the first frame update
    public override IEnumerator OnEnterPreBattle() {
        yield return StartCoroutine(EnemyUnitUtils.StopWalkingOnPath(Walker));
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

    public override void LateStart() {

    }
}
