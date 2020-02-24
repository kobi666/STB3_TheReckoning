using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitUtils
{


    public static string FindIfTargetIsLeftOrRightOfSelf ( GameObject _self, GameObject _target) {
        if (_self.transform.position.x < _target.transform.position.x ) {
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

        
    
    
    
    
    
    
    
    
    
    
    
    
    }
    

