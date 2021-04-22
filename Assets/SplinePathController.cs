﻿using System;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using Sirenix.OdinInspector;
using UnityEngine;

public class SplinePathController : MyGameObject,IActiveObject<SplinePathController>,IhasGameObjectID
{
    public MainPathController parentPath;
    [ShowInInspector]
    public SortedList<int,Vector2> splinePoints = new SortedList<int,Vector2>();

    public float GetPointIndexAccordingToRation(int pointIndex)
    {
        float p = pointIndex / (float)splinePoints.Count;
        return p;
    }
    private void OnEnable()
    {
        ActivePool.AddObjectToActiveObjectPool(this);
    }

    private void OnDisable()
    {
        ActivePool.RemoveObjectFromPool(MyGameObjectID);
    }

    public SplineTypes SplineType;
    [SerializeField]
    public BGCurve BgCurve;
    
    [SerializeField]
    public BGCcMath BgCcMath;
    [SerializeField]
    public EdgeCollider2D EdgeCollider2D;
    [SerializeField]
    public SplineDetector SplineDetector;

    protected void Awake()
    {
        //GameObjectID = IDGenerator.Instance.GetGameObjectID();
        ActivePool = GameObjectPool.Instance.ActiveSplines;
        EdgeCollider2D =  GetComponent<EdgeCollider2D>();
        SplineDetector =  GetComponent<SplineDetector>();
        OnPathUpdate += GetSplinePoints;
        EdgeCollider2D.enabled = false;
    }

    public event Action OnPathUpdate;

    public void GetSplinePoints()
    {
        splinePoints.Clear();
        Vector2[] points = EdgeCollider2D.points;
        int i = 0;
        foreach (var p in points)
        {
            splinePoints.Add(i,transform.TransformPoint(p));
            i++;
        }
    }

    protected void Start()
    {
        BgCurve = BgCurve ?? GetComponent<BGCurve>();
        OnPathUpdate?.Invoke();
    }

    public ActiveObjectPool<SplinePathController> ActivePool { get; set; }
}

public enum SplineTypes
{
    Top = 0,
    MiddleTop = 1,
    Main_Middle = 2,
    MiddleBottom = 3,
    Bottom = 4
}
