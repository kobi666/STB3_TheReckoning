using System;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public BGCcMath BgCcMath;
    public BGCc Spline;

    private void Awake()
    {
        
    }


    private float distance;
    private Vector3 tagent;
    void Update()
    {
        Vector2 TargetPos = BgCcMath.CalcPositionByDistance(distance);
        transform.position = TargetPos;
        distance += 0.1f * Time.deltaTime;
    }
}
