using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitUtils : MonoBehaviour
{
    

    public static PlayerUnitController GetRandomPlayerUnitFromList(EnemyUnitController ec) {
        return ec.Data.GetFirstPlayerUnitControllerFromList();
    }

// public static IEnumerator TellEnemyToPrepareFor1on1battleWithMe(EnemyUnitController ec, PlayerUnitController pc) {
//         if (ec.CurrentState == ec.States.Default) {
//             ec.Data.PlayerTarget = pc;
//             ec.SM.SetState(ec.States.PreBattle);
//         }
//         yield break;
//     }
    public static IEnumerator StandardPostBattleCheck(EnemyUnitController self) {
        PlayerUnitController pc = self.Data.GetFirstPlayerUnitControllerFromList();
        if (pc != null) {
            yield return self.StartCoroutine(TellPlayerUnitToInitiateForDirectBattleWithMe(self.Data.GetFirstPlayerUnitControllerFromList()));
            self.Target = pc;
            self.Data.EffectableTarget = GameObjectPool.Instance.ActiveEffectables.Pool[pc.name];
            self.SM.SetState(self.States.PreBattle);
        }
        else {
            self.SM.SetState(self.States.Default);
        }
        yield break;
    }

    public static IEnumerator TellPlayerUnitToInitiateForDirectBattleWithMe(PlayerUnitController pc) {
        pc.SM.SetState(pc.States.PreBattle);
        yield break;
    }
    static IEnumerator meleeAttackCoroutineAndInvokeAction(EnemyUnitController self) {
        float maxCounter = 1.0f;
        while (self.Target?.IsTargetable() ?? false) {
            if (maxCounter >= 1.0f) {
            self.OnAttack();
            maxCounter = 0.0f;
            }
            maxCounter += ((StaticObjects.Instance.DeltaGameTime / 10.0F));
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    public static IEnumerator MeleeAttackCoroutineAndInvokeAction(EnemyUnitController self) {
        self.SM.InitilizeAttackCoroutine(meleeAttackCoroutineAndInvokeAction(self));
        yield return self.SM.StartCoroutine(self.SM.AttackCoroutine);
        yield break;
    }

    public static void AttackPlayerUnit(EnemyUnitController self) {
        self.Data.EffectableTarget.ApplyDamage(self.Data.DamageRange.RandomDamage());
        //self.Target?.LifeManager.DamageToUnit(UnityEngine.Random.Range(self.Data.DamageRange.min,self.Data.DamageRange.max), self.Data.damageType);
    }
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
