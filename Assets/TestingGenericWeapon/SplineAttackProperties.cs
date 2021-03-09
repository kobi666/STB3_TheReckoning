using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;
using System;
using BansheeGz.BGSpline.Curve;

[System.Serializable]
public abstract class SplineAttackProperties : AttackProperties
{
    public Effectable MainTarget;
    public Vector2 TargetPosition;
    
    [SerializeField] [GUIColor(1, 0.6f, 0.4f)]
    public List<SplineBehavior> SplineBehaviors = new List<SplineBehavior>();
    public SplineBehavior SplineBehavior0
    {
        get => SplineBehaviors[0];
    }

    public SplineController SplineController0
    {
        get => SplineBehavior0.SplineController;
    }

    
    public float AttackDuration = 2;
    private float AttackProgressCounter = 0f;

    public virtual void SpecificPropertiesInit()
    {
        
    }

    public void InitializeAttackProperties(GenericWeaponController parentWeapon)
    {
        ParentWeapon = parentWeapon;
        InitlizeProperties();
    }
    public void InitlizeProperties()
    {
        if (SplineBehaviors.Count > 0) {
            foreach (var sb in SplineBehaviors)
            {
                if (sb != null)
                {
                  sb.Init();
                  onAttackStart += sb.OnBehaviorStart;
                  onAttackEnd += sb.OnBehaviorEnd;
                }
            }
        }
        
        /*if (SplineBehavior0 != null)
            {
                SplineBehavior0.Init();
                
            }*/
    }
    
    [ShowInInspector]
    private bool asyncAttackInProgress = false;

    [ShowInInspector]
    public bool AsyncAttackInProgress
    {
        get => asyncAttackInProgress;
        set
        {
            asyncAttackInProgress = value;
        }
    }

    public virtual void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition)
    {
        
    }


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
                AttackProgressCounter += StaticObjects.DeltaGameTime;
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

[System.Serializable]
public class OneSplineFromOriginToTarget : SplineAttackProperties
{
    public override void SpecificPropertiesInit()
    {
        
    }

    public override void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition)
    {
        SplineBehavior0.InvokeSplineBehavior(targetEffectable, TargetPosition);
    }
}

[System.Serializable]
public class LeapingSplines : SplineAttackProperties
{
    public bool TrackingAllTargets;
    [Required]
    public RangeDetector ChainTargetDiscoveryDetector;
    public float ExtraDiscoveryRange;
    public float MinimumLeapDistance;
    public int Leaps = 2;
    [SerializeReference]
    public Dictionary<string,(Effectable,Vector2)> LeapingTargets = new Dictionary<string, (Effectable, Vector2)>();
    [Required]
    public EffectableTargetBank ExtraRangeTargetBank;

    private void SetInitialTargetTarget(Effectable ef, Vector2 pos)
    {
        LeapingTargets.Add(ef.name,(ef,pos));
    }
    public override void SpecificPropertiesInit()
    {
        LeapingTargets = new Dictionary<string, (Effectable, Vector2)>();
        if (ExtraRangeTargetBank.Detector == null)
        {
            ExtraRangeTargetBank.Detector = ChainTargetDiscoveryDetector;
            ExtraRangeTargetBank.InitRangeDetectorEvents();
        }
        ChainTargetDiscoveryDetector.UpdateSize(ParentWeapon.Data.componentRadius + ExtraDiscoveryRange);
        onAttackStart += SetInitialTargetTarget;
        onAttackEnd += delegate { LeapingTargets.Clear(); };
        onAttackEnd += delegate { TargetCounter = 0; };
        leapingPosisitions = new List<Vector2>();
    }

    private int TargetCounter = 0;
    private List<Vector2> leapingPosisitions = new List<Vector2>();

    public float DelayBetweenLeaps;
    private float DelayCounter = 0;
    public override void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition)
    {
        SplineBehavior0.InvokeSplineBehavior(LeapingTargets.Values.ToArray()[TargetCounter].Item1,
                LeapingTargets.Values.ToArray()[TargetCounter].Item2);
            if (SplineBehavior0.SplineMovement.TargetPositionReached)
            {
                DelayCounter += StaticObjects.DeltaGameTime;
                if (DelayCounter >= DelayBetweenLeaps) {
                    if (TargetCounter < Leaps)
                    {
                        string NextTargetName =
                            ExtraRangeTargetBank.TryToGetTargetClosestToPosition(
                                LeapingTargets.Values.ToArray()[TargetCounter].Item1.transform.position,
                                LeapingTargets.Keys.ToArray());
                        if (NextTargetName != String.Empty)
                        {
                            LeapingTargets.Add(NextTargetName, (ExtraRangeTargetBank.Targets[NextTargetName].Item1,
                                ExtraRangeTargetBank.Targets[NextTargetName].Item1.transform.position));
                            TargetCounter += 1;
                            SplineBehavior0.SplineMovement.TargetPositionReached = false;
                            BGCurvePointI lastPoint = SplineController0.BgCurve.Points.Last();
                            int pointsCount = SplineController0.BgCurve.Points.Length;
                            leapingPosisitions.Add(lastPoint.PositionLocal);
                            SplineController0.BgCurve.AddPoint(
                                new BGCurvePoint(SplineController0.BgCurve, lastPoint.PositionLocal,
                                    BGCurvePoint.ControlTypeEnum.Absent), pointsCount - 1);
                        }
                    }

                    DelayCounter = 0;
                }

                if (TrackingAllTargets)
                {
                    for (int i = 0; i < LeapingTargets.Count - 1; i++)
                    {
                        if (LeapingTargets.Values.ToArray()[i].Item1 != null)
                        {
                            leapingPosisitions[i] = LeapingTargets.Values.ToArray()[i].Item1.transform.position;
                        }

                        SplineController0.BgCurve.Points[i + 1].PositionLocal = leapingPosisitions[i];
                    }
                }
            }
        
    }
}


[System.Serializable]
public class testSplineAttackProperties : SplineAttackProperties
{
    public override void SpecificPropertiesInit()
    {
        
    }

    public override void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition)
    {
        
    }
}

