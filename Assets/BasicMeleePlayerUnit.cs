using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BasicMeleePlayerUnit : PlayerUnitController
{
    


    public override bool IsTargetable() {
        return PlayerUnitUtils.StandardIsTargetable(this);
    }
    public override event Action<PlayerUnitController> onAttack;
    public override void OnAttack() {
        onAttack?.Invoke(this);
    }
    public override bool CanEnterNewBattle() {
        if (CurrentState == States.Default || CurrentState == States.JoinBattle || CurrentState == States.PostBattle ) {
            return true;
        }
        return false;
    }
    public override IEnumerator OnEnterPreBattle() {
        if (Target != null) {
            if (!PlayerUnitUtils.CheckIfEnemyIsInBattleWithOtherUnit(Target)) {
                yield return StartCoroutine(PlayerUnitUtils.TellEnemyToPrepareFor1on1battleWithMe(Target, this));
                yield return StartCoroutine(PlayerUnitUtils.MoveToTargetAndInvokeAction(this, PlayerUnitUtils.FindDirectBattlePosition(SR, Target.SR),
                Data.speed, (Target == null), Target.OnBattleInitiate));
                SM.SetState(States.InDirectBattle);
            }
            else {
                yield return StartCoroutine(PlayerUnitUtils.MoveToTargetAndInvokeAction(this, PlayerUnitUtils.FindJoinBattlePosition(SR, Target.SR),
                Data.speed, (Target == null), null));
                SM.SetState(States.JoinBattle);
            }
        
        yield break;
        }
        else {
            SM.SetState(States.Default);
        }
    }

    public override void OnTargetEnteredRange(EnemyUnitController ec) {
        if (CanEnterNewBattle()) {
            Data.EnemyTarget = ec;
            SM.SetState(States.PreBattle);
        }
    }

    public override IEnumerator OnEnterJoinBattle() {
        yield return StartCoroutine(PlayerUnitUtils.AttackCoroutineAndInvokeAction(this, !PlayerUnitUtils.StandardConditionToAttack(this), Data.AttackRate, OnAttack));
        SM.SetState(States.PostBattle);
        yield break;
    }

    public override IEnumerator OnExitJoinBattle() {
        yield break;
    }

    public override IEnumerator OnExitPreBattle() {
        yield break;
    }

    public override IEnumerator OnEnterDefault() {
        Data.EnemyTarget = null;
        yield return StartCoroutine(PlayerUnitUtils.ReturnToBattlePosition(this));
        yield break;
    }

    public override IEnumerator OnExitDefault() {
        yield break;
    }

    public override IEnumerator OnEnterDeath() {
        yield return StartCoroutine(DieAfterTwoSeconds());
        yield break;
    }

    public override IEnumerator OnExitDeath() {
        yield break;
    }

    public override IEnumerator OnEnterInDirectBattle() {
        Target?.OnBattleInitiate();
        if (Target != null) {
        yield return StartCoroutine(PlayerUnitUtils.TellEnemyToPrepareFor1on1battleWithMe(Data.EnemyTarget, this));
        // yield return StartCoroutine(PlayerUnitUtils.MoveToTargetAndInvokeAction(this, PlayerUnitUtils.FindPositionNextToUnit(SR, Target.SR),
        //                             Data.speed, (Target == null), Target.OnBattleInitiate));
        yield return StartCoroutine(PlayerUnitUtils.AttackCoroutineAndInvokeAction(this, !PlayerUnitUtils.StandardConditionToAttack(this), Data.AttackRate, OnAttack));
        }
        SM.SetState(States.PostBattle);
        yield break;
    }


    public override IEnumerator OnExitInDirectBattle() {
        yield break;
    }

    public override IEnumerator OnEnterPostBattle() {
        yield return StartCoroutine(PlayerUnitUtils.StandardPostBattleCheck(this));
        yield break;
    }

    public override IEnumerator OnExitPostBattle() {
        yield break;
    }

    public override void Test2() { Debug.LogWarning("TESTTTT") ;}
    
    // Start is called before the first frame update
    public override void LateStart() {
        SM.SetState(States.Default);
        onAttack += PlayerUnitUtils.AttackEnemyUnit;
        Data.SetPosition = transform.position;
    }
}
