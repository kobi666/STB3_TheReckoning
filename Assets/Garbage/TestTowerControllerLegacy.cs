using UnityEngine;

public class TestTowerControllerLegacy : TowerControllerLegacy
{
    
    public SpriteRenderer SR;
    // Start is called before the first frame update
        public void NorthFunc() {
        Debug.Log("1");
        SR.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
    }

    public override TowerSlotAction NorthAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"place test tower 1",null,NorthFunc);
        return tc;
    }

    public override TowerSlotAction EastAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"",null,EastFunc);
        return tc;
    }

    public override TowerSlotAction SouthAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"",null,SouthFunc);
        return tc;
    }

    public override TowerSlotAction WestAction() {
        TowerSlotAction tc = new TowerSlotAction(null,"",null,WestFunc);
        return tc;
    }


    public override bool NorthExecutionCondition(TowerComponent tc) {
        return true;
    }
    public override bool EastExecutionCondition(TowerComponent tc) {
        return true;
    }

    public override bool SouthExecutionCondition(TowerComponent tc) {
        return true;
    }
    public override bool WestExecutionCondition(TowerComponent tc) {
        return true;
    }

    public void EastFunc() {
       Debug.Log("2");
       Debug.LogWarning("Testing, source GO:" + gameObject.name);
    }
    public void SouthFunc() {
        Debug.Log("3");
        Debug.LogWarning("Testing, source GO:" + gameObject.name);
    }
    public void WestFunc() {
        Debug.Log("4");
        Debug.LogWarning("Testing, source GO:" + gameObject.name);
    }

   public override void PostAwake() {


        SR = GetComponent<SpriteRenderer>();
        // TowerActions.ButtonNorth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower1, TowerArsenal.arsenal.TestTower1.TowerSprite, NorthFunc);
        
        // TowerActions.ButtonEast = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.EmptyTowerSlot, TowerArsenal.arsenal.TestTower2.TowerSprite, EastFunc);
        
        // TowerActions.ButtonSouth = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower3, TowerArsenal.arsenal.TestTower3.TowerSprite, SouthFunc);
        
        // TowerActions.ButtonWest = new TowerSlotAction("Place tower : " + TowerArsenal.arsenal.TestTower4, TowerArsenal.arsenal.TestTower4.TowerSprite, WestFunc);
        
    }


    // Start is called before the first frame update
    public override void PostStart() {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
