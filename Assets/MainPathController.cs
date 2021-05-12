using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using Sirenix.OdinInspector;
using UnityEngine;

public class MainPathController : SplinePathController
{
    [Required]
    public CustomBGCBoxCollider CustomBgcBoxCollider;

    [Required]
    public SeconderyPathController SeconderyPathControllerPrefab;

    public NumberOfPaths NumberOfPaths;
    public float singlePathWidth = 0.5f;
    public Dictionary<SplineTypes,SplinePathController> SplinePaths = new Dictionary<SplineTypes,SplinePathController>();
    
    [ShowInInspector]
    private float TotalPathWidth
    {
        get => (float)NumberOfPaths * singlePathWidth;
    }

    protected void Awake()
    {
        base.Awake();
        
        CustomBgcBoxCollider = GetComponent<CustomBGCBoxCollider>();
        InitPath();
        SplineType = SplineTypes.Main_Middle;
        SplinePaths.Add(SplineType,this);
    }
    
    
    public void InitPath()
    {
        if (NumberOfPaths != NumberOfPaths.One) {
            foreach (var pathPositionDelta in GetPathPositionDelta())
            {
                var sp = CreateSeconderyPath(pathPositionDelta.Item1);
                sp.SplineType = pathPositionDelta.Item2;
                SplinePaths.Add(sp.SplineType,sp);
            }
        }
        
    }

    public (float,SplineTypes)[] GetPathPositionDelta()
    {
        (float,SplineTypes)[] floatArray = null;

        if (NumberOfPaths == NumberOfPaths.Three)
        {
            floatArray = new (float,SplineTypes)[2]
            {
                (singlePathWidth, SplineTypes.MiddleTop), (-singlePathWidth,SplineTypes.MiddleBottom)
            };
        }

        if (NumberOfPaths == NumberOfPaths.Five)
        {
            floatArray = new (float,SplineTypes)[4]
            {
                (singlePathWidth, SplineTypes.MiddleTop), (-singlePathWidth,SplineTypes.MiddleBottom), (singlePathWidth * 2f, SplineTypes.Top), (-singlePathWidth * 2f,SplineTypes.Bottom)
            };
        }
        return floatArray;
    }

    SeconderyPathController CreateSeconderyPath(float pathPositionDelta)
    {
        SeconderyPathController seconderyPathController = Instantiate(SeconderyPathControllerPrefab, transform);
        seconderyPathController.parentPath = this;
        seconderyPathController.transform.position = transform.position + new Vector3(0, pathPositionDelta, 0);
        for (int i = 0; i < BgCurve.Points.Length; i++)
        {
            seconderyPathController.BgCurve.AddPoint(BgCurve.Points[i]
                .Clone(seconderyPathController.BgCurve, new Vector3(0, pathPositionDelta, 0)));
        }

        return seconderyPathController;
    }

}


public enum NumberOfPaths
{
    One = 1,
    Three = 3,
    Five = 5
}
