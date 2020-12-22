using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System.Threading.Tasks;
using System;

public abstract class SplineAttackProperties : AttackProperties
{
    public Effectable MainTarget;
    public Vector2 TargetPosition;
    [OdinSerialize] [GUIColor(1, 0.6f, 0.4f)] public List<SplineBehavior> Splines {get; set;} 
    public float AttackDuration = 2;
    private float AttackProgressCounter = 0f;
    public abstract void SpecificPropertiesInit();

    public void InitializeAttackProperties(GenericWeaponController parentWeapon)
    {
        ParentWeapon = parentWeapon;
        InitlizeProperties();
    }
    public void InitlizeProperties()
    {
        foreach (var spline in Splines)
        {
            if (spline != null)
            {
                spline.Init();
                spline.onBehaviorStart += onAttackStart;
                spline.onBehaviorEnd += onAttackEnd;
            }
        }
    }
    
    [ShowInInspector]
    private bool asyncAttackInProgress = false;

    public bool AsyncAttackInProgress
    {
        get => asyncAttackInProgress;
        set
        {
            asyncAttackInProgress = value;
        }
    }

    public abstract void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition);


    public event Action<Effectable,Vector2> onAttackStart;
    public event Action onAttackEnd;
    public void StopAttack()
    {
        asyncAttackInProgress = false;
    }
    public async void StartAsyncSplineAttack(Effectable targetEffectable, Vector2 targetPosition)
    {
        MainTarget = targetEffectable;
        TargetPosition = targetPosition;
        AttackProgressCounter = 0;
        if (AsyncAttackInProgress == false)
        {
            AsyncAttackInProgress = true;
            onAttackStart?.Invoke(targetEffectable,targetPosition);
            /*foreach (var spline in Splines)
            {
                spline.OnBehaviorStart(targetEffectable,targetPosition);
            }*/
            while (AttackProgressCounter < AttackDuration && AsyncAttackInProgress == true && MainTarget != null)
            {
                AttackProgressCounter += StaticObjects.Instance.DeltaGameTime;
                SplineAttackFunction(MainTarget, TargetPosition);
                await Task.Yield();
            }

            AsyncAttackInProgress = false;
            onAttackEnd?.Invoke();
            /*foreach (var sp in Splines)
            {
                sp.OnBehaviorEnd();
            }*/
            MainTarget = null;
        }
    }

}


public class OneSplineFromOriginToTarget : SplineAttackProperties
{
    public override void SpecificPropertiesInit()
    {
        
    }

    public override void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition)
    {
        Splines[0].InvokeSplineBehavior(targetEffectable, TargetPosition);
    }
}

public class ChainSplines : SplineAttackProperties
{
    public bool TrackingAllTargets;
    [Required]
    public RangeDetector ChainTargetDiscoveryDetector;
    public float ExtraDiscoveryRange;
    public int Leaps = 2;
    public List<(Effectable,Vector2)> LeapingTargets = new List<(Effectable,Vector2)>();
    [Required]
    public EffectableTargetBank ExtraRangeTargetBank;

    private void SetInitialTargetTarget(Effectable ef, Vector2 pos)
    {
        LeapingTargets[0] = (ef,pos);
    }
    public override void SpecificPropertiesInit()
    {
        if (ExtraRangeTargetBank.Detector == null)
        {
            ExtraRangeTargetBank.Detector = ChainTargetDiscoveryDetector;
            ExtraRangeTargetBank.InitRangeDetectorEvents();
        }
        ChainTargetDiscoveryDetector.SetSize(ParentWeapon.Data.componentRadius + ExtraDiscoveryRange);
        onAttackStart += SetInitialTargetTarget;
        onAttackEnd += delegate { LeapingTargets.Clear(); };
    }

    private int TargetCounter = 0;
    
    

    public override void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition)
    {
        Splines[0].InvokeSplineBehavior(LeapingTargets[TargetCounter].Item1,LeapingTargets[TargetCounter].Item2);
        if (Splines[0].SplineMovement.TargetPositionReached)
        {
            if (TargetCounter < Leaps)
            {
                string NextTargetName =
                    ExtraRangeTargetBank.TryToGetTargetClosestToPosition(LeapingTargets[TargetCounter].Item1.transform.position);
                if (NextTargetName != String.Empty)
                {
                    LeapingTargets[TargetCounter + 1] = (ExtraRangeTargetBank.Targets[NextTargetName],
                        ExtraRangeTargetBank.Targets[NextTargetName].transform.position);
                    TargetCounter += 1;
                    Splines[0].SplineMovement.TargetPositionReached = false;
                }
            }
        }
    }
}

