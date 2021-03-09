using System;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class SplineController : MonoBehaviour,IQueueable<SplineController>
{
    public BGCurve BgCurve;
    public EffectableTargetBank TargetBank;
    public SplineDetector Detector;
    public ProjectileFinalPoint FinalPoint;
    public ProjectileExitPoint ExitPoint;

    public BGCurvePointI[] points
    {
        get => BgCurve?.Points;
    }
    
    public LineRenderer LineRenderer;

    protected void Awake()
    {
        BgCurve = GetComponent<BGCurve>();
        TargetBank = TargetBank ?? GetComponent<EffectableTargetBank>();
        Detector = Detector ?? GetComponent<SplineDetector>();
        LineRenderer = GetComponent<LineRenderer>();
        FinalPoint = FinalPoint ?? GetComponentInChildren<ProjectileFinalPoint>();
        ExitPoint = ExitPoint ?? GetComponentInChildren<ProjectileExitPoint>();
    }

    public Type QueueableType { get; set; }
    public PoolObjectQueue<SplineController> QueuePool { get; set; }
    public void OnEnqueue()
    {
        gameObject.SetActive(false);
    }

    public void OnDequeue()
    {
        
    }
}
