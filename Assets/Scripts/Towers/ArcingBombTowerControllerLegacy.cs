public class ArcingBombTowerControllerLegacy : TowerControllerLegacy
{
    // Start is called before the first frame update
    public override TowerSlotAction NorthAction()
    {
        return new TowerSlotAction();
    }

    public override bool NorthExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override TowerSlotAction EastAction()
    {
        return new TowerSlotAction();
    }

    public override bool EastExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override TowerSlotAction SouthAction()
    {
        return new TowerSlotAction();
    }

    public override bool SouthExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override TowerSlotAction WestAction()
    {
        return new TowerSlotAction();
    }

    public override bool WestExecutionCondition(TowerComponent tc)
    {
        return true;
    }

    public override void PostStart()
    {
        
    }

    void Start()
    {
        base.Start();
    }

    public override void PostAwake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
