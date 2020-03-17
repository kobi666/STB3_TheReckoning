using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerController : TowerController
{
    // Start is called before the first frame update

    

    public void Test() {
        Debug.Log("asdasdasd");
    }

    public void TestChangeColor() {
        Debug.Log("STAMMM");
        TowerSpriteRenderer.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
    }
    public override void PostStart()
    {
        
        
    }

    private void Awake() {
        TowerActions = TowerUtils.DefaultSlotActions;
        TowerSpriteRenderer = GetComponent<SpriteRenderer>();
        TowerActions.ButtonNorth = new TowerSlotAction("asdfasd", TowerSprite);
        TowerActions.ButtonNorth.ActionFunctions += Test;
        TowerActions.ButtonSouth = new TowerSlotAction("Change Color", TowerSprite);
        TowerActions.ButtonSouth.ActionFunctions += TestChangeColor;
    }

    
    
}
