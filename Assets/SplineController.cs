using System;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using Sirenix.OdinInspector;
using UnityEngine;

public class SplineController : MonoBehaviour
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
}
