using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerController : TowerController
{
    public SpriteRenderer SR;
    // Start is called before the first frame update
        public void ChangeColor() {
        Debug.Log("1");
        SR.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
    }

    public void DebugWarning1() {
       Debug.Log("2");
       Debug.LogWarning("Testing, source GO:" + gameObject.name);
    }
    public void PlaceTestTower3() {
        Debug.Log("3");
        Debug.LogWarning("Testing, source GO:" + gameObject.name);
    }
    public void PlaceTestTower4() {
        Debug.Log("4");
        Debug.LogWarning("Testing, source GO:" + gameObject.name);
    }

    private void Awake() {
        SR = GetComponent<SpriteRenderer>();
        TowerActions.ButtonNorth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower1, TowerArsenal.arsenal.TestTower1.TowerSprite);
        TowerActions.ButtonNorth.ActionFunctions += ChangeColor;
        TowerActions.ButtonEast = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.EmptyTowerSlot, TowerArsenal.arsenal.TestTower2.TowerSprite);
        TowerActions.ButtonEast.ActionFunctions += DebugWarning1;
        TowerActions.ButtonSouth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower3, TowerArsenal.arsenal.TestTower3.TowerSprite);
        TowerActions.ButtonSouth.ActionFunctions += PlaceTestTower3;
        TowerActions.ButtonWest = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower4, TowerArsenal.arsenal.TestTower4.TowerSprite);
        TowerActions.ButtonWest.ActionFunctions += PlaceTestTower4;
    }


    // Start is called before the first frame update
    public override void PostStart() {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
