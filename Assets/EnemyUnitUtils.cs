using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnitUtils : MonoBehaviour
{
    public static bool StandardCheckIfMorePlayerUnitsAreFightingme(EnemyUnitController self) {
        if (self.Data.PlayerUnitsFightingMe.Count > 0) {
            return true;
        }
        return false;
    }
    static IEnumerator meleeAttackCoroutineAndInvokeAction(EnemyUnitController self) {
        float maxCounter = 1.0f;
        while (self.Target?.IsTargetable() ?? false) {
            if (maxCounter >= 1.0f) {
            self.OnAttack();
            maxCounter = 0.0f;
            }
            maxCounter += ((StaticObjects.instance.DeltaGameTime * (PlayerUnitAfterEffects.instance.MeleeAttackRateWithMultiplier(self.Data.AttackRate)) / 10.0F));
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
        self.Target?.LifeManager.DamageToUnit(UnityEngine.Random.Range(self.Data.DamageRange.min,self.Data.DamageRange.max), self.Data.damageType);
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
