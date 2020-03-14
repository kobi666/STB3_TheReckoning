using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerController : TowerController
{
    // Start is called before the first frame update

    public void TestChangeColor() {
        TowerSpriteRenderer.color = new Color
            (
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
            );
    }
    public override void PostStart()
    {
        TowerSpriteRenderer = GetComponent<SpriteRenderer>();
        TowerActions.ButtonSouth = new TowerSlotAction("Change Color", TowerSpriteRenderer.sprite);
    }

    
    
}
