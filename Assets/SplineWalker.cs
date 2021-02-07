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

    /*void moveEffectCounter()
    {
        moveEffectCounter().dissolve += 5;
    }


    IEnumerator StartEffect()
    {
        float counter = 0;
        while (counter < 1)
        {
            counter += Time.deltaTime;
            moveEffectCounter
            yield return null;
        }
        yield break;
        
    }*/
    
    

   

    private float distance;
    private Vector3 tagent;
    void Update()
    {
        Vector2 TargetPos = BgCcMath.CalcPositionAndTangentByDistance(distance, out tagent);
        transform.position = TargetPos;
        distance += 0.1f * Time.deltaTime;
    }
}
