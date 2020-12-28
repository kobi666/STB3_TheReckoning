using System;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class test17 : MonoBehaviour
{
    public PlayerInput PlayerControl;
    public BGCurve curve;
    
    private void OnEnable() {
        PlayerControl.TestButtons.Enable();
    }

    void removePointC()
    {
        Debug.LogWarning("asdasdasdasd");
        curve.Delete(curve.Points[curve.Points.Length -1]);
    }

    void removePointP()
    {
        Debug.LogWarning("asdasdasdasd");
        curve.Points[curve.Points.Length].Curve.Delete(curve.Points[curve.Points.Length -1]);
    }

    private void Awake()
    {
        PlayerControl = new PlayerInput();
        PlayerControl.TestButtons.J.performed += ctx => removePointC();
        
        PlayerControl.TestButtons.T.performed += ctx => removePointP();
    }

    void Start()
    {
        curve = GetComponent<BGCurve>();
    }

    
}
