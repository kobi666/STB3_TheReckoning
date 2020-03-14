using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerController : TowerController
{
    // Start is called before the first frame update

    public void TestChangeColor(GameObject self, GameObject parent) {
        TowerSpriteRenderer.color = new Color
            (
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
            );
    }

    public void Test(GameObject game, GameObject gasd) {
        Debug.Log("asdasdasd");
    }
    public override void PostStart()
    {
        TowerSpriteRenderer = GetComponent<SpriteRenderer>();
        TowerActions.ButtonSouth = new TowerSlotAction("Change Color", TowerSprite);
        TowerActions.ButtonSouth.ActionFunctions += TestChangeColor;
        TowerActions.ButtonNorth = new TowerSlotAction("asdfasd", TowerSprite);
        TowerActions.ButtonNorth.ActionFunctions += Test;
    }

    
    
}
