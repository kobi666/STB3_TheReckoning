using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnitUtils
{
    
    public static IEnumerator ReturnToBattlePosition(PlayerUnitController self) {
        yield return self.SM.StartCoroutine(MoveToTargetAndInvokeAction(self, self.Data.SetPosition, self.Data.speed, false, null));
        yield break;
    }

    public static IEnumerator StandardPostBattleCheck(PlayerUnitController pc) {
        if (pc.TargetBank.TargetExists()) {
            if (pc.CanEnterNewBattle()) {
                pc.Data.EnemyTarget = pc.TargetBank.FindSingleTargetNearestToEndOfSpline();
                pc.SM.SetState(pc.States.PreBattle);
            }
        }
        else {
            pc.SM.SetState(pc.States.Default);
        }
        yield break;
    }

    public static void AttackEnemyUnit(PlayerUnitController self) {
        self.Target?.LifeManager.DamageToUnit(UnityEngine.Random.Range(self.Data.DamageRange.min,self.Data.DamageRange.max), self.Data.damageType);
    }
    
    public static bool StandardIsTargetable(PlayerUnitController pc) {
        if (pc.CurrentState != pc.States.Death) {
            return true;
        }
        return false;
    }

    public static bool StandardConditionToAttack(PlayerUnitController pc) {
        
        if (pc.CurrentState == pc.States.InDirectBattle || pc.CurrentState == pc.States.JoinBattle) {
            if (pc.Target?.IsTargetable() ?? false) {
                return true;
            }
        }
        return false;
    }
    public static bool StandardEnterDirectBattleCondition(ObjectState us, EnemyUnitController ec, NormalUnitStates states) {
        if (us == states.Default || us == states.PostBattle) {
                return true;
        }
        return false;
    }

    static IEnumerator attackCoroutineAndInvokeAction(PlayerUnitController self, bool stopCondition, float attackRate, Action attackAction) {
        float maxCounter = 1.0f;
        while (self.Target?.IsTargetable() ?? false) {
            if (maxCounter >= 1.0f) {
            attackAction?.Invoke();
            maxCounter = 0.0f;
            }
            maxCounter += ((StaticObjects.instance.DeltaGameTime * (PlayerUnitAfterEffects.instance.MeleeAttackRateWithMultiplier(attackRate)) / 10.0F));
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }


    public static IEnumerator AttackCoroutineAndInvokeAction(PlayerUnitController self, bool stopCondition, float attackRate, Action attackAction) {
        self.SM.InitilizeAttackCoroutine(attackCoroutineAndInvokeAction(self, stopCondition, attackRate, attackAction));
        yield return self.SM.StartCoroutine(self.SM.AttackCoroutine);
        yield break;
    }
    
    public static IEnumerator MoveToTargetAndInvokeAction(UnitController self, Vector2 targetPos, float speed, bool stopCondition, Action action) {
        self.SM.InitilizeMovementCoroutine(moveToTargetAndInvokeAction(self.transform, targetPos, speed, stopCondition, action));
        yield return self.SM.StartCoroutine(self.SM.MovementCoroutine);
        yield break;
    }

    static IEnumerator moveToTargetAndInvokeAction(Transform self, Vector2 targetPos, float speed, bool stopCondition, Action action) {
         while((Vector2)self.position != targetPos && stopCondition == false) {
            self.position = Vector2.MoveTowards(self.position, targetPos, speed * StaticObjects.instance.DeltaGameTime);
            yield return new WaitForFixedUpdate();
        }
        action?.Invoke();
        yield break;
    }
    public static bool CheckIfEnemyIsInBattleWithOtherUnit(EnemyUnitController ec) {
        if (ec.SM.CurrentState == ec.States.PreBattle || ec.SM.CurrentState == ec.States.InDirectBattle) {
            return true;
        }
        else {
            return false;
        }
    }

    public static IEnumerator TellEnemyToPrepareFor1on1battleWithMe(EnemyUnitController ec, PlayerUnitController pc) {
        if (ec.CurrentState == ec.States.Default) {
            ec.Data.PlayerTarget = pc;
            ec.SM.SetState(ec.States.PreBattle);
        }
        yield break;
    }

    public static string FindIfTargetIsLeftOrRightOfSelf ( GameObject _self, GameObject _target) {
        if (_self.transform.position.x < _target.transform.position.x ) {
            return "left";
        }
        else 
        { 
            return "right";
        }
    }

    public static string FindIfTargetIsLeftOrRightOfSelf ( PlayerUnitController pc, EnemyUnitController ec) {
        if (pc.transform.position.x < ec.transform.position.x ) {
            return "left";
        }
        else 
        { 
            return "right";
        }
    }

    public static Vector2 FindJoinBattlePosition(SpriteRenderer selfSR, SpriteRenderer targetSR) {
        Vector2 TargetSpriteExtent = targetSR.sprite.bounds.extents;
        Vector2 SelfSpriteExtent = selfSR.sprite.bounds.extents;
        SelfSpriteExtent.x *= selfSR.transform.localScale.x;
        SelfSpriteExtent.y *= selfSR.transform.localScale.y;
        TargetSpriteExtent.x *= targetSR.transform.localScale.x;
        TargetSpriteExtent.y *= targetSR.transform.localScale.y;
        Vector2 TargetGOPos = targetSR.transform.position;
        Vector2 SelfPos = selfSR.transform.position;
        string LoR = (FindIfTargetIsLeftOrRightOfSelf(selfSR.gameObject, targetSR.gameObject));
        float randY = UnityEngine.Random.Range(-0.3f, 0.3f);
        float randX = UnityEngine.Random.Range(-0.1f, 0.1f);
        Vector2 pos = SelfPos;
        if (LoR == "left") {
            pos.x = (TargetGOPos.x - TargetSpriteExtent.x - 0.1f - SelfSpriteExtent.x + randX);
            pos.y = (TargetGOPos.y - TargetSpriteExtent.y + SelfSpriteExtent.y + randY);
        }
        else if (LoR == "right") {
            pos.x = (TargetGOPos.x + TargetSpriteExtent.x + 0.1f + SelfSpriteExtent.x + randX);
            pos.y = (TargetGOPos.y - TargetSpriteExtent.y + SelfSpriteExtent.y + randY);
        }
        else {
            Debug.Log("Couldn't find if object is left or right.. Default to self transform.position...");
            pos = selfSR.transform.position;
        }
        return pos;
    }
    
    //Battle Position Finder
    public static Vector2 FindDirectBattlePosition(SpriteRenderer selfSR, SpriteRenderer targetSR) {
        Vector2 TargetSpriteExtent = targetSR.sprite.bounds.extents;
        Vector2 SelfSpriteExtent = selfSR.sprite.bounds.extents;
        SelfSpriteExtent.x *= selfSR.transform.localScale.x;
        SelfSpriteExtent.y *= selfSR.transform.localScale.y;
        TargetSpriteExtent.x *= targetSR.transform.localScale.x;
        TargetSpriteExtent.y *= targetSR.transform.localScale.y;
        Vector2 TargetGOPos = targetSR.transform.position;
        Vector2 SelfPos = selfSR.transform.position;
        string LoR = (FindIfTargetIsLeftOrRightOfSelf(selfSR.gameObject, targetSR.gameObject));

        Vector2 pos = SelfPos;
        if (LoR == "left") {
            pos.x = (TargetGOPos.x - TargetSpriteExtent.x - 0.1f - SelfSpriteExtent.x);
            pos.y = (TargetGOPos.y - TargetSpriteExtent.y + SelfSpriteExtent.y);
        }
        else if (LoR == "right") {
            pos.x = (TargetGOPos.x + TargetSpriteExtent.x + 0.1f + SelfSpriteExtent.x);
            pos.y = (TargetGOPos.y - TargetSpriteExtent.y + SelfSpriteExtent.y);
        }
        else {
            Debug.Log("Couldn't find if object is left or right.. Default to self transform.position...");
            pos = selfSR.transform.position;
        }
        return pos;
        }

        public static Vector2 FindPositionNextToUnit(PlayerUnitController pc, EnemyUnitController ec) {
        Vector2 TargetSpriteExtent = ec.SR.sprite.bounds.extents;
        Vector2 SelfSpriteExtent = pc.SR.sprite.bounds.extents;
        SelfSpriteExtent.x *= pc.transform.localScale.x;
        SelfSpriteExtent.y *= pc.transform.localScale.y;
        TargetSpriteExtent.x *= ec.transform.localScale.x;
        TargetSpriteExtent.y *= ec.transform.localScale.y;
        Vector2 TargetGOPos = ec.transform.position;
        Vector2 SelfPos = pc.transform.position;
        string LoR = (FindIfTargetIsLeftOrRightOfSelf(pc, ec));

        Vector2 pos = SelfPos;
        if (LoR == "left") {
            pos.x = (TargetGOPos.x - TargetSpriteExtent.x - 0.1f - SelfSpriteExtent.x);
            pos.y = (TargetGOPos.y - TargetSpriteExtent.y + SelfSpriteExtent.y);
        }
        else if (LoR == "right") {
            pos.x = (TargetGOPos.x + TargetSpriteExtent.x + 0.1f + SelfSpriteExtent.x);
            pos.y = (TargetGOPos.y - TargetSpriteExtent.y + SelfSpriteExtent.y);
        }
        else {
            Debug.Log("Couldn't find if object is left or right.. Default to self transform.position...");
            pos = pc.transform.position;
        }
        return pos;
        }


        
        
    
    
    
    
    
    
    
    
    
    
    
    
    }
    

