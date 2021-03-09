﻿public class EffectableUnit : Effectable
{
    public UnitController unitController;
    public GenericUnitController GenericUnitController;
    private UnitStateMachine StateMachine
    {
        get => GenericUnitController.StateMachine;
    }
    private bool _targetable = false;
    public bool targetable
    {
        get
        {
            return _targetable;
        }
        set
        {
            if (value != _targetable)
            {
                _targetable = value;
                OnTargetStateChange(value);
            }
        }
    }
    
    

    public bool ExternalTargetableLock { get; set; }
    public override bool IsTargetable()
    {
        if (gameObject.activeSelf == true) {
            if (ExternalTargetableLock == false)
            {
                if (StateMachine.CurrentState.stateName != UnitStates.Death)
                {
                    targetable = true;
                    return true;
                }
            }
        }

        targetable = false;
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
        unitController = unitController ?? GetComponent<UnitController>();
        GenericUnitController = GenericUnitController ?? GetComponent<GenericUnitController>();
        //StateMachine = GenericUnitController.StateMachine;
        IsTargetable();
    }
}
