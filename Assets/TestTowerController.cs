using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerController : TowerController
{
    // Start is called before the first frame update

    

    public void Test() {
        Debug.Log("asdasdasd");
    }
    public override void PostStart()
    {
        TowerActions = TowerUtils.DefaultSlotActions;
        TowerSpriteRenderer = GetComponent<SpriteRenderer>();
        TowerActions.ButtonNorth = new TowerSlotAction("asdfasd", TowerSprite);
        TowerActions.ButtonNorth.ActionFunctions += Test;
    }

    
    
}
