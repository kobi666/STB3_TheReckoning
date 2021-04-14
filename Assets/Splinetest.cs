using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using DegreeUtils;
using UnityEngine;

public class Splinetest : MonoBehaviour
{
    private BGCcMath parentCurve;
    void Start()
    {
        parentCurve = GetComponent<BGCcMath>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
