using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[System.Serializable]
public abstract class BeamWeapon : WeaponController
{
    public override void MainAttackFunction()
    {
        RenderBeamForDuration();
    }

    public LineRenderer lineRenderer;
    public float BeamDurationTimer;
    public float startFireCounter;
    
    public bool isOscilating;
    
    private bool m_BeamSwitch;
    public bool BeamSwitch
    {
        get => m_BeamSwitch;
        set
        {
            if (value != m_BeamSwitch)
            {
                if (value == true)
                {
                    lineRenderer.enabled = true;
                }
                if (value == false)
                {
                    lineRenderer.enabled = false;
                }
            }
            m_BeamSwitch = value;
        }
    }

    private bool _widthDirection;

    void OscilateBeam()
    {
        if (CurrentBeamWidth <= CurrentBeamWidth - Data.BeamData.OscliatingBeamWidthMinMax.min)
        {
            _widthDirection = true;
        }
        else if (CurrentBeamWidth >= CurrentBeamWidth + Data.BeamData.OscliatingBeamWidthMinMax.max)
        {
            _widthDirection = false;
        }

        if (_widthDirection == true)
        {
            CurrentBeamWidth += Time.deltaTime * Data.BeamData.BeamOsciliationSpeed * Data.BeamData.BeamDuration;
        }
        else
        {
            CurrentBeamWidth -= Time.deltaTime * Data.BeamData.BeamOsciliationSpeed * Data.BeamData.BeamDuration;
        }
    }

    private float currentBeamWidth;
    public virtual float CurrentBeamWidth
    {
        get => currentBeamWidth;
        set
        {
            currentBeamWidth = value;
            lineRenderer.startWidth = value;
            lineRenderer.endWidth = value;
        }
    }

    public BeamRenderingFunction BeamRenderingFunction;

    private void RenderLine()
    {
        if (BeamSwitch == false)
        {
            BeamSwitch = true;
        }
        BeamRenderingFunction.Invoke(lineRenderer,ProjectileExitPoint,Target.transform.position, Data.BeamData.BeamMovementSpeed);
    }

    public event Action onBeamRender;

    public void OnBeamRender()
    {
        onBeamRender?.Invoke();
    }
    
    private void RenderBeamForDuration() {
        if (BeamDurationTimer > 0)
        {
            RenderLine();
            BeamDurationTimer -= Time.deltaTime;
        }

        if (BeamDurationTimer <= 0)
        {
            BeamSwitch = false;
            startFireCounter = 0;
        }

        startFireCounter += StaticObjects.instance.DeltaGameTime;
        if (BeamSwitch == false)
        {
            if (startFireCounter <= Data.BeamData.CooldownTime)
            startFireCounter += StaticObjects.instance.DeltaGameTime;
            if (startFireCounter >= Data.BeamData.CooldownTime)
            {
                BeamDurationTimer = Data.BeamData.BeamDuration;
            }
        }
    }
    
    protected void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>() ?? null;
        lineRenderer.material = Data.BeamData.BeamMaterial ?? null;
    }
    protected void Start()
    {
        onBeamRender += RenderLine;
        if (Data.BeamData.IsOscillating)
        {
            onBeamRender += OscilateBeam;
        }
        onBeamRender += delegate { Debug.DrawLine(transform.position,Target.transform.position); };
        
        base.Start();
        if (Data.BeamData.beamWidth == 0f)
        {
            CurrentBeamWidth = 0.1f;
        }
        else
        {
            CurrentBeamWidth = Data.BeamData.beamWidth;
        }
    }

}
