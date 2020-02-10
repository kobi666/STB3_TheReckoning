using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitUtils
{
    public static void FindIfTargetIsLeftOrRightOfSelf () {

    }
    
    //Battle Position Finder
    public static void FindPositionNextToUnit(GameObject _target) {
        Sprite TargetSprite = _target.GetComponent<SpriteRenderer>().sprite;
        Vector2 TargetPos = _target.transform.position;

    }
    
}
