using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicEnemyMeleeUnit : EnemyUnitController
{
    public override bool IsTargetable() {
        return EnemyUnitUtils.StandardIsTargetable(this);
    }
    public override event Action<EnemyUnitController> onAttack;
    public override void OnAttack() {
        onAttack?.Invoke(this);
    }
    public override event Action onBattleInitiate;
    public override void OnBattleInitiate() {
        onBattleInitiate?.Invoke();
    }
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

    public override IEnumerator OnEnterInDirectBattle() {
        
        yield break;
    }

    public override IEnumerator OnExitInDirectBattle() {
        yield break;
    }

    public override IEnumerator OnEnterPostBattle() {
        yield break;
    }

    public override IEnumerator OnExitPostBattle() {
        yield break;
    }

    public override IEnumerator OnEnterDeath() {
        yield break;
    }

    public override IEnumerator OnExitDeath() {
        yield break;
    }

    public override void LateStart() {
        SM.SetState(States.Default);
        onBattleInitiate += delegate { SM.SetState(States.InDirectBattle);};
    }
}
