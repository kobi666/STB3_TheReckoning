﻿using System.Collections;
using UnityEngine;


public class BasicMeleePlayerUnit : PlayerUnitController
{
    public override bool IsTargetable() {
        return PlayerUnitUtils.StandardIsTargetable(this);
    }
    
    public override bool CanEnterNewBattle() {
        if (CurrentStateLegacy == States.Default || CurrentStateLegacy == States.JoinBattle || CurrentStateLegacy == States.PostBattle ) {
            return true;
        }
        return false;
    }
    public override IEnumerator OnEnterPreBattle() {
        if (Target != null) {
            if (!PlayerUnitUtils.CheckIfEnemyIsInBattleWithOtherUnit(Target)) {
                yield return StartCoroutine(PlayerUnitUtils.TellEnemyToPrepareFor1on1battleWithMe(Target, this));
                animationController.OnWalking();
                yield return StartCoroutine(PlayerUnitUtils.MoveToTargetAndInvokeAction(this, PlayerUnitUtils.FindDirectBattlePosition(SR, Target.SR),
                dataLegacy.speed, (Target == null), Target.OnBattleInitiate));
                SM.SetState(States.InDirectBattle);
            }
            else {
                animationController.OnWalking();
                yield return StartCoroutine(PlayerUnitUtils.MoveToTargetAndInvokeAction(this, PlayerUnitUtils.FindJoinBattlePosition(SR, Target.SR),
                dataLegacy.speed, (Target == null), null));
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
            dataLegacy.EnemyTarget = ec;
            dataLegacy.EffectableTarget = GameObjectPool.Instance.ActiveEffectables.Pool[ec.name];
            SM.SetState(States.PreBattle);
        }
    }

    public override IEnumerator OnEnterJoinBattle() {
        if (Target != null) {
        if (Target.transform.position.x > transform.position.x) {
            SetXdirection(false);
        }
        if (Target.transform.position.x < transform.position.x) {
            SetXdirection(true);
        }
        animationController.OnIdle();
        yield return StartCoroutine(PlayerUnitUtils.AttackCoroutineAndInvokeAction(this, !PlayerUnitUtils.StandardConditionToAttack(this), dataLegacy.AttackRate, OnAttack));
        }
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
        dataLegacy.EnemyTarget = null;
        dataLegacy.EffectableTarget = null;
        animationController.OnWalking();
        yield return StartCoroutine(PlayerUnitUtils.ReturnToBattlePosition(this));
        animationController.OnIdle();
        yield break;
    }

    public override IEnumerator OnExitDefault() {
        yield break;
    }

    public override IEnumerator OnEnterDeath() {
        animationController.OnDeath();
        QueuePool.ObjectQueue.Enqueue(this);
        yield return StartCoroutine(DieAfterTwoSeconds());
        
        yield break;
    }

    public override IEnumerator OnExitDeath() {
        yield break;
    }

    public override IEnumerator OnEnterInDirectBattle() {
        Target?.OnBattleInitiate();
        if (Target != null) {
        if (Target.transform.position.x > transform.position.x) {
            SetXdirection(false);
        }
        if (Target.transform.position.x < transform.position.x) {
            SetXdirection(true);
        }
        animationController.OnIdle();
        yield return StartCoroutine(PlayerUnitUtils.TellEnemyToPrepareFor1on1battleWithMe(dataLegacy.EnemyTarget, this));
        yield return StartCoroutine(PlayerUnitUtils.AttackCoroutineAndInvokeAction(this, !PlayerUnitUtils.StandardConditionToAttack(this), dataLegacy.AttackRate, OnAttack));
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
        //Data.SetPosition = transform.position;
    }
}
