using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class BeamWeaponAsync : WeaponController
{
    private TowerComponentBeamData BeamData
    {
        get => Data.BeamData;
    }

    public bool SingleTargetAttack;
    public bool AreaAttack;
    public bool ContinuousAttack;
    private bool attackOnceLock = false;
    

    private Effectable[] TempEffectables;
    
    
    private LineRenderer LineRenderer;
    
    public bool beamAttackInprogress = false;

    private bool BeamAttackInprogress
    {
        get => beamAttackInprogress;
        set
        {
            beamAttackInprogress = value;
            if (beamAttackInprogress == true)
            {
                LineRenderer.enabled = true;
            }
            else if (beamAttackInprogress == false)
            {
                LineRenderer.enabled = false;
            }
        }
    }
    private float beamWidth;
    private float BeamWidth
    {
        get => beamWidth;
        set
        {
            beamWidth = value;
            LineRenderer.startWidth = beamWidth;
            LineRenderer.endWidth = beamWidth;
        }
    }


    private bool _widthDirection;
    private (float,float) BeamWidthMinMax
    {
        get
        {
            return (BeamData.beamWidth - BeamData.OscliatingBeamWidthMinMax.min, BeamData.beamWidth +
                BeamData.OscliatingBeamWidthMinMax.max);
        }
    }

    void OscilateBeamWidth()
    {
        if (BeamWidth <= BeamWidthMinMax.Item1)
        {
            _widthDirection = true;
        }
        else if (BeamWidth >= BeamWidthMinMax.Item2)
        {
            _widthDirection = false;
        }

        if (_widthDirection == true)
        {
            BeamWidth += StaticObjects.instance.DeltaGameTime * BeamData.BeamOsciliationSpeed * BeamData.BeamDuration;
        }
        else
        {
            BeamWidth -= StaticObjects.instance.DeltaGameTime * BeamData.BeamOsciliationSpeed * BeamData.BeamDuration;
        }
    }
    

    public async void StartAsyncBeam()
    {
        
        if (BeamAttackInprogress == false)
        {
            BeamAttackInprogress = true;
            BeamWidth = BeamData.beamWidth;
            int maxPosition = LineRenderer.positionCount;
        }
        
        if (ContinuousAttack) {
            while (beamDurationCounter > 0 && Target != null)
            {
                if (BeamAttackInprogress == true)
                {
                        beamDurationCounter -= StaticObjects.instance.DeltaGameTime;
                        OnContinuousBeamAttack();
                        await Task.Yield();
                    }
            }
        }

        if (SingleTargetAttack)
        {
            staticTarget = Target;
            while (beamDurationCounter > 0)
            {
                if (beamAttackInprogress == true)
                {
                    if (staticTarget != null) {
                    OnSingleBeamAttack();
                    beamDurationCounter -= StaticObjects.instance.DeltaGameTime;
                    await Task.Yield();
                    }
                }
            }
            attackOnceLock = false;
            staticTarget = null;
        }

            
        BeamAttackInprogress = false;
    }

    private float coolDownTimer;

    public float CoolDownTimer
    {
        get => coolDownTimer;
        set
        {
            coolDownTimer = value;
            if (coolDownTimer >= Data.BeamData.CooldownTime)
            {
                
                beamDurationCounter = Data.BeamData.BeamDuration;
                
                coolDownTimer = 0;
            }
        }
    }

    void moveBeamFinalPointInstant()
    {
        ProjectileFinalPointTransform.position = Target.transform.position;
    }

    void moveBeamFinalPointOverTime()
    {
        ProjectileFinalPointTransform.position = Vector2.MoveTowards(ProjectileFinalPointV2, Target.transform.position,
            BeamData.BeamMovementSpeed);
    }


    void renderBeam()
    {
        BeamRenderingFunction.Invoke(LineRenderer,ProjectileExitPoint,ProjectileFinalPointV2, BeamData.BeamMovementSpeed);
    }

    public BeamRenderingFunction BeamRenderingFunction;
    [SerializeField] private float beamDurationCounter = 1;

    public event Action onContinuousBeamAttack;

    public void OnContinuousBeamAttack()
    {
        onContinuousBeamAttack?.Invoke();
    }

    public event Action onSingleBeamAttack;

    public void OnSingleBeamAttack()
    {
        onSingleBeamAttack?.Invoke();
    }


    private TargetUnit staticTarget;


    protected void Awake()
    {
        
        base.Awake();
        LineRenderer = GetComponent<LineRenderer>() ?? null;
        onContinuousBeamAttack += renderBeam;
       
        if (BeamData.beamWidth != 0)
        {
            BeamWidth = BeamData.beamWidth;
        }
        
        if (BeamData.IsOscillating)
        {
            onSingleBeamAttack += OscilateBeamWidth;
            onContinuousBeamAttack += OscilateBeamWidth;
        }

        onContinuousBeamAttack += moveBeamFinalPointOverTime;

        if (SingleTargetAttack)
        {
            
            onSingleBeamAttack += delegate
            {
                if (staticTarget != null)
                {
                    ProjectileFinalPointTransform.position = staticTarget.transform.position;
                }
            };
            onSingleBeamAttack += renderBeam;

        }
        
        if (AreaAttack) {
            onContinuousBeamAttack += delegate
            {
                if (TempEffectables != null)
                {
                    BeamUtilities.BeamDamageOnArea(projectileFinalPoint.EffectableTargetBank,
                        Data.damageRange.RandomDamage(), ref TempEffectables);
                }

                ;
            };
        }

        if (!AreaAttack)
        {
            onContinuousBeamAttack += delegate
            {
                if (attackOnceLock == false) {
                Target.Effectable.ApplyDamage(Data.damageRange.RandomDamage());
                attackOnceLock = true;
                }
            };
            onSingleBeamAttack += delegate
            {
                if (attackOnceLock == false) {
                    Target.Effectable.ApplyDamage(Data.damageRange.RandomDamage());
                    attackOnceLock = true;
                }
            };
        }
    }


    public override void MainAttackFunction()
    {
        StartAsyncBeam();
    }

    public override void PostStart()
    {
        
    }

    protected void Start()
    {
        base.Start();
        projectileFinalPoint.RangeDetector.SetRangeRadius(BeamWidth / 2);
        if (AreaAttack) {
        projectileFinalPoint.EffectableTargetBank.onTargetsUpdate += delegate
        {
            TempEffectables = projectileFinalPoint.EffectableTargetBank.Targets.Values.ToArray(); };
        }
    }

    protected void Update()
    {
        base.Update();
        if (BeamAttackInprogress == false)
        {
            CoolDownTimer += StaticObjects.instance.DeltaGameTime;
        }
    }
}
