using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnitUtils
{
     

    public static bool StandardEnterBattleCondition(UnitState us, EnemyUnitController ec, NormalUnitStates states) {
        if (us == states.Default || us == states.PostBattle) {
                return true;
        }
        return false;
    }

    
    public static IEnumerator MoveToTargetAndInvokeAction(UnitController self, Vector2 targetPos, float speed, bool stopCondition, Action action) {
        self.SM.InitilizeMovementCoroutine(moveToTargetAndInvokeAction(self.transform, targetPos, speed, stopCondition, action));
        yield return self.SM.StartCoroutine(self.SM.MovementCoroutine);
        yield break;
    }

    static IEnumerator moveToTargetAndInvokeAction(Transform self, Vector2 targetPos, float speed, bool stopCondition, Action action) {
         while((Vector2)self.position != targetPos && stopCondition == false) {
            Debug.LogWarning("SALKDJASLKDJASLKJD");
            self.position = Vector2.MoveTowards(self.position, targetPos, speed * StaticObjects.instance.DeltaGameTime);
            yield return new WaitForFixedUpdate();
        }
        action.Invoke();
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
    
    //Battle Position Finder
    public static Vector2 FindPositionNextToUnit(GameObject _self, GameObject _target) {
        Vector2 TargetSpriteExtent = _target.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        Vector2 SelfSpriteExtent = _self.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        SelfSpriteExtent.x *= _self.transform.localScale.x;
        SelfSpriteExtent.y *= _self.transform.localScale.y;
        TargetSpriteExtent.x *= _target.transform.localScale.x;
        TargetSpriteExtent.y *= _target.transform.localScale.y;
        Vector2 TargetGOPos = _target.transform.position;
        Vector2 SelfPos = _self.transform.position;
        string LoR = (FindIfTargetIsLeftOrRightOfSelf(_self, _target));

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
            pos = _self.transform.position;
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
    

