using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public abstract class BeamWeapon : WeaponController
{
    public LineRenderer lineRenderer;
    public float laserDuration;
    public float laserDurationTimer;
    public float startFireCounter;
    public float startFireCounterMax;
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

    public abstract void RenderLineFunction(LineRenderer lineRenderer, Vector2 originPos, Vector2 targetPos);
    
    public void RenderBeam()
    {
        bool laserEnabled;
        
        
        if (laserDurationTimer > 0)
        { 
            BeamSwitch = true;
            lineRenderer.SetPosition(0, ProjectileExitPoint);
            lineRenderer.SetPosition(1, ProjectileFinalPointV2);
            laserDurationTimer -= Time.deltaTime;
        }

        if (laserDurationTimer <= 0)
        {
            BeamSwitch = false;
        }

        startFireCounter += StaticObjects.instance.DeltaGameTime;
        if (startFireCounter >= startFireCounterMax)
        {
            laserDurationTimer = laserDuration;
            startFireCounter = 0;
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
        base.Start();
        if (Data.BeamData.beamWidth == 0f)
        {
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }
    }

}
