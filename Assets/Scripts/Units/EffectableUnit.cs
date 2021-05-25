public class EffectableUnit : Effectable
{
    
    public GenericUnitController GenericUnitController;
    private UnitStateMachine StateMachine
    {
        get => GenericUnitController.StateMachine;
    }
    private bool targetable = false;
    public bool Targetable
    {
        get
        {
            return targetable;
        }
        set
        {
            if (value != targetable)
            {
                targetable = value;
                OnTargetStateChange(value);
            }
        }
    }
    
    

    public bool ExternalTargetableLock { get; set; }
    public override bool IsTargetable()
    {
        if (ExternalTargetableLock == false)
            {
                if (StateMachine.CurrentState.TargetableState)
                {
                    Targetable = true;
                    if (!GameObjectPool.Instance.Targetables.Contains(MyGameObjectID))
                    {
                        GameObjectPool.Instance.Targetables.Add(MyGameObjectID);
                    }
                    return true;
                }
            }
        

        if (GameObjectPool.Instance.Targetables.Contains(MyGameObjectID))
        {
            GameObjectPool.Instance.Targetables.Remove(MyGameObjectID);
        }
        Targetable = false;
        return false;
    }
    

    

    public override void ApplyFreeze(float FreezeAmount, float TotalFreezeProbability) {

    }

    public override void ApplyDamage(int damageAmount) {
        GenericUnitController.UnitLifeManager.DamageToUnit(damageAmount);
    }

    public override void ApplyExplosion(float explosionValue) {

    }

    public override void ApplyPoision(int poisionAmount, float poisionDuration) {

    }

    protected void Start() {
        base.Start();
        GenericUnitController = GenericUnitController ?? GetComponent<GenericUnitController>();
        //StateMachine = GenericUnitController.StateMachine;
        IsTargetable();
    }
}
