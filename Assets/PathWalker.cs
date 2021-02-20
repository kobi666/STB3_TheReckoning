﻿using System;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PathWalker : MonoBehaviour
{
    public BGCcMath spline;
    [HideInInspector]
    public bool SplineAttached;

    private void OnEnable()
    {
        EndOfPathReached = false;
    }

    public float ProximityToPathEnd
    {
        get => SplineTotalLength - CurrentDistanceOnSpline;
    }
    public BGCcMath Spline
    {
        get => spline;
        set
        {
            spline = value;
            if (spline != null)
            {
                SplineAttached = true;
            }
            else
            {
                SplineAttached = false;
            }
        }
    }

    public float SplineTotalLength = 0;
    public Transform ObjectTransform;

    public event Action onPathEnd;

    public event Action<float> onPathMovement;

    public void OnPathMovement(float distanceDelta)
    {
        onPathMovement?.Invoke(distanceDelta);
    }

    public bool OnPath;
    public event Action<bool> onPathShift;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == Spline.name)
        {
            onPathShift?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == Spline.name)
        {
            onPathShift?.Invoke(false);
        }
    }


    private float currentDistanceOnSpline = 999f;

    private bool EndOfPathReached;
    
    [ShowInInspector]
    public float CurrentDistanceOnSpline { 
        get => currentDistanceOnSpline;
        set
        {
            if (CurrentDistanceOnSpline < SplineTotalLength) {
            currentDistanceOnSpline = value;
            }
            if (currentDistanceOnSpline >= SplineTotalLength)
            {
                /*if (OnPath)
                {*/
                    if (!EndOfPathReached) {
                    onPathEnd?.Invoke();
                    EndOfPathReached = true;
                    }
                //}
            }
        }
    }

    private Vector2 TargetPosition
    {
        get => Spline.CalcPositionByDistance(CurrentDistanceOnSpline);
    }

    public void MoveAlongSpline(float distancedelta)
    {
        CurrentDistanceOnSpline += distancedelta;
        ObjectTransform.position = TargetPosition;
    }
    

    private void Start()
    {
        currentDistanceOnSpline = 0;
        SplineTotalLength = Spline.GetDistance();
        onPathShift += delegate(bool b) { OnPath = b; };
        onPathMovement += MoveAlongSpline;
        ObjectTransform = transform.parent;
        Spline = Spline;
    }
}