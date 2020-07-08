using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class BasicEnemyMeleeUnit : EnemyUnitController
{
    public override bool IsTargetable() {
        return EnemyUnitUtils.StandardIsTargetable(this);
    }
    public override void Test2() { Debug.LogWarning("TESTTT");}
    public override bool CannotInitiateBattleWithThisUnit() {
        return false;
    }
    // Start is called before the first frame update
    public override IEnumerator OnEnterPreBattle() {
        animationController.OnIdle();
        yield return StartCoroutine(EnemyUnitUtils.StopWalkingOnPath(Walker));
    }


    public override IEnumerator OnExitPreBattle() {
        yield break;
    }

    public override IEnumerator OnEnterDefault() {
        animationController.OnWalking();
        Walker.ReturnWalking();
        yield break;
    }

    public override IEnumerator OnExitDefault() {
        yield break;
    }

    public override IEnumerator OnEnterInDirectBattle() {
        animationController.OnIdle();
        yield return StartCoroutine(EnemyUnitUtils.MeleeAttackCoroutineAndInvokeAction(this));
        SM.SetState(States.PostBattle);
        yield break;
    }

    public override IEnumerator OnExitInDirectBattle() {
        yield break;
    }

    public override IEnumerator OnEnterPostBattle() {
        yield return StartCoroutine(EnemyUnitUtils.StandardPostBattleCheck(this));
        yield break;
    }

    public override IEnumerator OnExitPostBattle() {
       // yield return StartCoroutine(EnemyUnitUtils.StandardPostBattleCheck(this));
        yield break;
    }

    public override IEnumerator OnEnterDeath() {
        UnitCollider.enabled = false;
        yield return StartCoroutine(EnemyUnitUtils.StopWalkingOnPath(Walker));
        animationController.OnDeath();
        yield return StartCoroutine(DieAfterTwoSeconds());
        yield break;
    }

    public override IEnumerator OnExitDeath() {
        yield break;
    }

    public override void LateStart() {
        SM.SetState(States.Default);
        onBattleInitiate += delegate { SM.SetState(States.InDirectBattle);};
        onAttack += EnemyUnitUtils.AttackPlayerUnit;
    }
}

